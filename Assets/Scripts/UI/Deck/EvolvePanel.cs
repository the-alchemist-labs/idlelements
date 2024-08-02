using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EvolvePanel : MonoBehaviour
{
    public TMP_Text evolveToText;
    public TMP_Text idleBonusText;

    public HorizontalLayoutGroup tokensContainer;
    public HorizontalLayoutGroup essenceContainer;
    public TMP_Text tokenCostText;
    public TMP_Text essenceCostText;

    public Button evolveButton;
    public CelebateEvolutionPanel celebrateEvolutionPanel;

    Elemental elemental;

    void Start()
    {
        GameEvents.OnTokensUpdated += UpdateEvolveButton;
        GameEvents.OnEssenceUpdated += UpdateEvolveButton;
    }

    void OnDestroy()
    {
        GameEvents.OnTokensUpdated -= UpdateEvolveButton;
        GameEvents.OnEssenceUpdated -= UpdateEvolveButton;
    }

    public void DisplayPanel(Elemental elemental)
    {
        this.elemental = elemental;

        gameObject.SetActive(true);
        Elemental evolutionElemental = ElementalCatalog.Instance.GetElemental(elemental.Evolution.evolveTo);
        IdleBonus idleBonus = evolutionElemental.IdleBonus;

        evolveToText.text = $"Evlove to {evolutionElemental.name} (#{evolutionElemental.Id})";
        idleBonusText.text = idleBonus != null ? $"Idle bonus: +{idleBonus.amount * 100} {idleBonus.resource}" : "";

        tokenCostText.text = TextUtil.NumberFormatter(elemental.Evolution.tokensCost);
        essenceCostText.text = TextUtil.NumberFormatter(elemental.Evolution.essenceCost);
        LayoutRebuilder.ForceRebuildLayoutImmediate(essenceContainer.GetComponent<RectTransform>());
        LayoutRebuilder.ForceRebuildLayoutImmediate(tokenCostText.GetComponent<RectTransform>());

        UpdateEvolveButton();
    }

    void UpdateEvolveButton()
    {
        evolveButton.interactable = ElementalManager.Instance.CanEvolve(elemental.Id);
    }

    public void Evolve()
    {
        ElementalManager.Instance.Evolve(elemental.Id);
        celebrateEvolutionPanel.DisplayPanel(elemental);
        gameObject.SetActive(false);
    }
}
