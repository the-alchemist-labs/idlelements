using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectPartyMemberPrefab : MonoBehaviour
{
    [SerializeField] int slot;
    [SerializeField] GameObject partyMemberImagePrefab;
    [SerializeField] Image skillImageA;
    [SerializeField] Image skillImageB;

    private SelectPartyMemberPanel _selectPartyMemberPanel;
    private SelectSkillPanel _selectSkillPanel;
    private ElementalId _elementalId;
    void Awake()
    {
        GameEvents.OnPartyUpdated += UpdateUI;
    }

    void Start()
    {
        _selectPartyMemberPanel = MainManager.Instance.SelectPartyMemberPanel.GetComponent<SelectPartyMemberPanel>();
        _selectSkillPanel = MainManager.Instance.SelectSkillPanel.GetComponent<SelectSkillPanel>();
        UpdateUI();
    }

    void OnDestroy()
    {
        GameEvents.OnPartyUpdated -= UpdateUI;
    }

    private void UpdateUI()
    {
        _elementalId = Player.Instance.Party.GetPartyMember(slot);
        List<SkillId?> skillIds = ElementalManager.Instance.GetSkills(_elementalId);
        partyMemberImagePrefab.GetComponent<PartyMemberImagePrefab>().Init(_elementalId);
        
        skillImageA.sprite = GetSkillSprite((skillIds.Count > 0) ? skillIds[0] : null);
        skillImageB.sprite = GetSkillSprite((skillIds.Count > 1) ? skillIds[1] : null);
    }

    public void OnElementalClicked()
    {
        _selectPartyMemberPanel.OpenPanel(slot);
    }

    public void OnSkillClicked(int slot)
    {
        _selectSkillPanel.OpenPanel(_elementalId, slot);
    }

    private Sprite GetSkillSprite(SkillId? skillId)
    {
        string spritePath = skillId != null ? $"Sprites/Skills/{skillId}" :  "Sprites/UI/AddCircle";
        return Resources.Load<Sprite>(spritePath);
    }
}
