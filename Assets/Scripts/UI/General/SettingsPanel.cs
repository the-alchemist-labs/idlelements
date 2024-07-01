using UnityEngine;
using UnityEngine.UI;

public class SettingsPanel : MonoBehaviour
{
    private string BGM_VOLUME_KEY = "BGM_Volume";
    private string DISCORD_URL ="https://discord.gg/QQJmPV6s";

    public AudioSource bgmAudio;
    public Slider bgmSlider;

    void Start()
    {
        float savedVolume = PlayerPrefs.GetFloat(BGM_VOLUME_KEY, 0.5f);
        bgmSlider.value = savedVolume;
        bgmSlider.onValueChanged.AddListener(SetVolume);
    }

    public void SetVolume(float volume)
    {
        bgmAudio.volume = volume;
        PlayerPrefs.SetFloat(BGM_VOLUME_KEY, volume);
    }

    public void OpenDiscord()
    {
        Application.OpenURL(DISCORD_URL);
    }
}