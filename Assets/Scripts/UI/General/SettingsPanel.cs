using UnityEngine;
using UnityEngine.UI;

public class SettingsPanel : MonoBehaviour
{
    public Slider BGMSlider;
    public Slider SFXSlider;

    void Start()
    {
        BGMSlider.value = SoundManager.Instance.BGMVolume;
        BGMSlider.onValueChanged.AddListener(SoundManager.Instance.UpdateBGMVolume);

        SFXSlider.value = SoundManager.Instance.SFXVolume;
        SFXSlider.onValueChanged.AddListener(SoundManager.Instance.UpdateSFXVolume);
    }

    public void OpenDiscord()
    {
        Application.OpenURL(Consts.DiscordUrl);
    }
}