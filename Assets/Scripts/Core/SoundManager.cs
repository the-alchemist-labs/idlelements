using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;

public enum Soundtrack
{
    MainBG,
}

public enum SystemSFXId
{
    Celebration,
    Click,
    Coins,
    CoinsDroped
}

public class SoundManager : MonoBehaviour
{
    private const int SYSTEM_SFX_PRIORITY = 100;
    private const int BATTLE_SFX_PRIORITY = 200;

    public static SoundManager Instance;
    public float SFXVolume { get; private set; }
    public float BGMVolume { get; private set; }

    [SerializeField] AudioSource audioSourcePerfab;
    private ObjectPool<AudioSource> _pool;
    private Dictionary<SystemSFXId, AudioClip> _systemSfxAudioClips;
    private Dictionary<SkillId, AudioClip> _skillSfxAudioClips;
    private AudioSource _backgroundMusic;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Initialize();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        PlayBackgroundMusic(Soundtrack.MainBG);
    }

    private void Initialize()
    {
        SFXVolume = PlayerPrefs.GetFloat(PlayerPrefKeys.SFX_Volume, 0.5f);
        BGMVolume = PlayerPrefs.GetFloat(PlayerPrefKeys.BGM_Volume, 0.5f);

        _pool = new ObjectPool<AudioSource>(
              createFunc: CreateAudioSource,
              actionOnGet: GetAudioSource,
              actionOnRelease: ReleaseAudioSource,
              actionOnDestroy: obj => Destroy(obj),
              collectionCheck: true,
              defaultCapacity: 5,
              maxSize: 10
          );

        InitAudioClipDictionaries();
    }

    public void UpdateSFXVolume(float volume)
    {
        PlayerPrefs.SetFloat(PlayerPrefKeys.SFX_Volume, volume);
        SFXVolume = volume;
    }

    public void UpdateBGVolume(float volume)
    {
        PlayerPrefs.SetFloat(PlayerPrefKeys.BGM_Volume, volume);
        _backgroundMusic.volume = volume;
    }

    public void PlaySystemSFX(SystemSFXId soundId)
    {
        PlaySFX(soundId, _systemSfxAudioClips, SYSTEM_SFX_PRIORITY);
    }

    public void PlaySkillSFX(SkillId soundId)
    {
        if (MainManager.Instance.TabManager.ActiveTab != MainSceneTab.Main) return;
        PlaySFX(soundId, _skillSfxAudioClips, BATTLE_SFX_PRIORITY);
    }

    private System.Collections.IEnumerator ReleaseAudioSourceAfterPlay(AudioSource audioSource)
    {
        yield return new WaitUntil(() => !audioSource.isPlaying);
        _pool.Release(audioSource);
    }

    private AudioSource CreateAudioSource()
    {
        GameObject audioSourceObject = Instantiate(audioSourcePerfab.gameObject);
        audioSourceObject.SetActive(false);
        return audioSourceObject.GetComponent<AudioSource>();
    }

    private void GetAudioSource(AudioSource audioSource)
    {
        audioSource.gameObject.SetActive(true);
        audioSource.volume = SFXVolume;
    }

    private void ReleaseAudioSource(AudioSource audioSource)
    {
        audioSource.Stop();
        audioSource.clip = null;
        audioSource.gameObject.SetActive(false);
    }

    private void InitAudioClipDictionaries()
    {
        _systemSfxAudioClips = InitializeAudioClipDictionary<SystemSFXId>("Audio/SFX/System");
        _skillSfxAudioClips = InitializeAudioClipDictionary<SkillId>("Audio/SFX/IdleBattle");
    }

    private Dictionary<TEnum, AudioClip> InitializeAudioClipDictionary<TEnum>(string path) where TEnum : Enum
    {
        return Enum.GetValues(typeof(TEnum))
            .Cast<TEnum>()
            .ToDictionary(
                id => id,
                id => Resources.Load<AudioClip>($"{path}/{id}")
            );
    }

    private void PlaySFX<TEnum>(TEnum soundId, Dictionary<TEnum, AudioClip> audioClips, int priority) where TEnum : Enum
    {
        if (audioClips.TryGetValue(soundId, out AudioClip clip))
        {
            AudioSource audio = _pool.Get();
            audio.clip = clip;
            audio.priority = priority;
            audio.Play();
            StartCoroutine(ReleaseAudioSourceAfterPlay(audio));
        }
    }

    private void PlayBackgroundMusic(Soundtrack soundtrack)
    {
        _backgroundMusic = gameObject.AddComponent<AudioSource>().GetComponent<AudioSource>();
        _backgroundMusic.clip = Resources.Load<AudioClip>($"Audio/Soundtrack/{soundtrack}");
        _backgroundMusic.volume = BGMVolume;
        _backgroundMusic.Play();
    }
}
