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

    void Start()
    {
        GameEvents.OnElementalCaught += UpdateUI;
        GameEvents.OnGoldUpdated += UpdateUI;
        GameEvents.OnEssenceUpdated += UpdateUI;
        UpdateUI();
    }

    void OnDestroy()
    {
        GameEvents.OnElementalCaught -= UpdateUI;
        GameEvents.OnGoldUpdated -= UpdateUI;
        GameEvents.OnEssenceUpdated -= UpdateUI;
    }

    void UpdateUI()
    {
        levelText.text = TextUtil.NumberFormatter(Player.Instance.Level);
        expSlider.value = GetExpPercent();
        expText.text = GetExpDisplay();
        essenseText.text = TextUtil.NumberFormatter(ResourcesData.Instance.Essence);
        goldText.text = TextUtil.NumberFormatter(ResourcesData.Instance.Gold);
    }

    public void OpenPlayerPanel()
    {
        GameManager gameManger = GameObject.FindGameObjectWithTag(Tags.GameManager).GetComponent<GameManager>();
        gameManger.playerInfoPanel?.SetActive(true);
        gameManger.playerInfoPanel?.GetComponent<PlayerInfoPanel>().Init(Player.Instance.GetPlayerInfo());
    }

    private static float GetExpPercent()
    {
        if (Player.Instance.IsMaxLevel()) return 1;
        if (Player.Instance.Experience == 0) return 0;
        return (float)Player.Instance.Experience / Player.Instance.ExpToLevelUp(Player.Instance.Level);
    }

    private static string GetExpDisplay()
    {
        return Player.Instance.IsMaxLevel()
        ? "Max Level"
        : $"{TextUtil.NumberFormatter(Player.Instance.Experience)}/{TextUtil.NumberFormatter(Player.Instance.ExpToLevelUp(Player.Instance.Level))}";
    }
}
