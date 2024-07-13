using UnityEngine;
using UnityEngine.UI;

public class SettingsPanel : MonoBehaviour
{
    public AudioSource bgmAudio;
    public Slider bgmSlider;

    void Start()
    {
        float savedVolume = PlayerPrefs.GetFloat(PlayerPrefKeys.BGM_Volume, 0.5f);
        bgmSlider.value = savedVolume;
        bgmSlider.onValueChanged.AddListener(SetVolume);
    }

    public void SetVolume(float volume)
    {
        bgmAudio.volume = volume;
        PlayerPrefs.SetFloat(PlayerPrefKeys.BGM_Volume, volume);
    }

    public void OpenDiscord()
    {
        Application.OpenURL(Consts.DiscordUrl);
    }
}