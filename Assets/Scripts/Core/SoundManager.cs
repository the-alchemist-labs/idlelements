using UnityEngine;

enum SoundGroup
{
    BGM,
    SFX
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    public float SFXVolume;
    public float BGMVolume;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        SFXVolume = PlayerPrefs.GetFloat(PlayerPrefKeys.SFX_Volume, 0.5f);
        BGMVolume = PlayerPrefs.GetFloat(PlayerPrefKeys.BGM_Volume, 0.5f);

        ChangeAudioSounds(SoundGroup.SFX, SFXVolume);
        ChangeAudioSounds(SoundGroup.BGM, BGMVolume);
    }

    public void PlaySFXFromPrefab(AudioSource sound)
    {
        sound.volume = SFXVolume;
        sound.Play();
    }

    public void UpdateSFXVolume(float volume)
    {
        PlayerPrefs.SetFloat(PlayerPrefKeys.SFX_Volume, volume);
        ChangeAudioSounds(SoundGroup.SFX, volume);
    }

    public void UpdateBGMVolume(float volume)
    {
        PlayerPrefs.SetFloat(PlayerPrefKeys.BGM_Volume, volume);
        ChangeAudioSounds(SoundGroup.BGM, volume);
    }

    private void ChangeAudioSounds(SoundGroup soundGroup, float volume)
    {
        GameObject childObject = transform.Find(soundGroup.ToString()).gameObject;

        foreach (Transform child in childObject.transform)
        {
            child.GetComponent<AudioSource>().volume = volume;
        }
    }
}
