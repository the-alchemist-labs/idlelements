using UnityEngine;
using UnityEngine.UI;

public class SettingsPanel : MonoBehaviour
{
    [SerializeField]
    Slider bGMSlider;
    [SerializeField]
    Slider sFXSlider;

    void Start()
    {
        bGMSlider.value = SoundManager.Instance.BGMVolume;
        bGMSlider.onValueChanged.AddListener(SoundManager.Instance.UpdateBGVolume);

        sFXSlider.value = SoundManager.Instance.SFXVolume;
        sFXSlider.onValueChanged.AddListener(SoundManager.Instance.UpdateSFXVolume);
    }

    public void OpenDiscord()
    {
        Application.OpenURL(Consts.DiscordUrl);
    }
}