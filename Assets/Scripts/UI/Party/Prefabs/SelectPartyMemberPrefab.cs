using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectPartyMemberPrefab : MonoBehaviour
{
    [SerializeField] int slot;
    [SerializeField] GameObject partyMemberImagePrefab;
    [SerializeField] Image skillImageA;
    [SerializeField] Image skillImageB;

    private ElementalId _elementalId;
    void Awake()
    {
        GameEvents.OnPartyUpdated += UpdateUI;
    }

    void Start()
    {
        UpdateUI();
    }

    void OnDestroy()
    {
        GameEvents.OnPartyUpdated -= UpdateUI;
    }

    private void UpdateUI()
    {
        _elementalId = Player.Instance.Party.GetPartyMember(slot);
        
        partyMemberImagePrefab.GetComponent<PartyMemberImagePrefab>().Init(_elementalId);
        List<SkillId> skillIds = ElementalManager.Instance.GetSkills(_elementalId);
        skillImageA.sprite = GetSkillSprite((skillIds.Count > 0) ? skillIds[0] : SkillId.None);
        skillImageB.sprite = GetSkillSprite((skillIds.Count > 1) ? skillIds[1] : SkillId.None);
    }

    public void OnElementalClicked()
    {
        SelectPartyMemberPopup popup = PopupManager.Instance.OpenPopUp<SelectPartyMemberPopup>(PopupId.SelectPartyMember);
        popup.OpenPanel(slot);
    }

    public void OnSkillClicked(int slot)
    {
        SelectSkillPopup popup = PopupManager.Instance.OpenPopUp<SelectSkillPopup>(PopupId.SelectSkill);
        popup.OpenPanel(_elementalId, slot);
    }

    private Sprite GetSkillSprite(SkillId? skillId)
    {
        string spritePath = skillId != SkillId.None ? $"Sprites/Skills/{skillId}" : "Sprites/UI/AddCircle";
        return Resources.Load<Sprite>(spritePath);
    }
}
