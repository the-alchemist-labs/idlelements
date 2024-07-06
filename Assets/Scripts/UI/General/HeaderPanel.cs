using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeaderBannerManager : MonoBehaviour
{
    public TMP_Text levelText;
    public TMP_Text expText;
    public Slider expSlider;
    public TMP_Text essenseText;
    public TMP_Text goldText;

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

    void Start()
    {
        GameEvents.OnElementalCaught += UpdateUI;
        UpdateUI();
    }

    void OnDestroy()
    {
        GameEvents.OnElementalCaught -= UpdateUI;
    }

    void UpdateUI()
    {
        levelText.text = TextUtil.NumberFormatter(State.level);
        expSlider.value = GetExpPercent();
        expText.text = GetExpDisplay();
        essenseText.text = TextUtil.NumberFormatter(State.essence);
        goldText.text = TextUtil.NumberFormatter(State.gold);
    }

    private static float GetExpPercent()
    {
        if (State.IsMaxLevel()) return 1;
        if (State.experience == 0) return 0;
        return (float)State.experience / State.ExpToLevelUp(State.level);
    }

    private static string GetExpDisplay()
    {
        return State.IsMaxLevel()
        ? "Max Level"
        : $"{TextUtil.NumberFormatter(State.experience)}/{TextUtil.NumberFormatter(State.ExpToLevelUp(State.level))}";
    }
}
