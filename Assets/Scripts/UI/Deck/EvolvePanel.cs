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
        Elemental evolutionElemental = ElementalsData.Instance.GetElemental(elemental.evolution.evolveTo);
        IdleBonus idleBonus = evolutionElemental.idleBonus;

        evolveToText.text = $"Evlove to {evolutionElemental.name} (#{evolutionElemental.id})";
        idleBonusText.text = idleBonus != null ? $"Idle bonus: +{idleBonus.amount * 100} {idleBonus.resource}" : "";

        tokenCostText.text = TextUtil.NumberFormatter(elemental.evolution.tokensCost);
        essenceCostText.text = TextUtil.NumberFormatter(elemental.evolution.essenceCost);
        LayoutRebuilder.ForceRebuildLayoutImmediate(essenceContainer.GetComponent<RectTransform>());
        LayoutRebuilder.ForceRebuildLayoutImmediate(tokenCostText.GetComponent<RectTransform>());

        UpdateEvolveButton();
    }

    void UpdateEvolveButton()
    {
        evolveButton.interactable = ElementalsData.Instance.CanEvolve(elemental.id);
    }

    public void Evolve()
    {
        ElementalsData.Instance.Evolve(elemental.id);
        celebrateEvolutionPanel.DisplayPanel(elemental);
        gameObject.SetActive(false);
    }
}
