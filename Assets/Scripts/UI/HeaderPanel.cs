using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeaderBannerManager : MonoBehaviour
{
    public TMP_Text levelText;
    public TMP_Text expText;
    public Slider expSlider;
    public TMP_Text essenseText;
    public TMP_Text orbsText;

    private static HeaderBannerManager instance;
    public static HeaderBannerManager Instance { get { return instance; } }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        levelText.text = TextUtil.NumberFormatter(State.level);
        expSlider.value = GetExpPercent();
        expText.text = $"{TextUtil.NumberFormatter(State.experience)}/{TextUtil.NumberFormatter(State.requiredExpToLevelUp[State.level])}";
        essenseText.text = TextUtil.NumberFormatter(State.essence);
        orbsText.text = TextUtil.NumberFormatter(State.orbs);
    }

    private static float GetExpPercent()
    {
        if (State.level == 1) return 1;
        if (State.experience == 0) return 0;
        return (float)State.experience / (float)State.requiredExpToLevelUp[State.level];
    }
}

