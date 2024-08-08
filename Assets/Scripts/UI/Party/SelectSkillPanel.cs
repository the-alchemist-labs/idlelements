using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectSkillPanel : MonoBehaviour
{
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private Transform scrollViewContent;
    [SerializeField] private GameObject skillPrefab;

    [SerializeField] private GameObject infoSection;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private TMP_Text typeText;
    [SerializeField] private TMP_Text targetText;
    [SerializeField] private TMP_Text powerText;
    [SerializeField] private TMP_Text speedText;

    private Elemental _elemental;
    private int _skillSlot;
    private SkillId _selectedSkill;
    private List<SkillId> _equippedSkills;

    public void OpenPanel(ElementalId elementalId, int slot)
    {
        gameObject.SetActive(true);

        _skillSlot = slot;
        _elemental = ElementalCatalog.Instance.GetElemental(elementalId);
        _equippedSkills = ElementalManager.Instance.GetSkills(_elemental.Id);
        _selectedSkill = _equippedSkills[slot];

        UpdateScrollView();
        UpdateInfo();
    }

    void UpdateScrollView()
    {
        scrollViewContent.Cast<Transform>().ToList().ForEach(child => Destroy(child.gameObject));

        foreach (SkillByLevel skillByLevel in _elemental.Skills)
        {
            GameObject skill = Instantiate(skillPrefab, scrollViewContent);
            if (skill.TryGetComponent(out SkillPrefab item))
            {
                item.Init(
                    skillByLevel.SkillId,
                    skillByLevel.Level,
                    _selectedSkill == skillByLevel.SkillId,
                    _equippedSkills.Contains(skillByLevel.SkillId)
                );
                item.OnSkillChanged += SkillChanged;
            }
        }
    }

    void UpdateInfo()
    {
        bool isSkillSelected = _selectedSkill != SkillId.None;
        
        infoSection.SetActive(isSkillSelected);
        if (!isSkillSelected) return;
        
        Skill skill = ElementalCatalog.Instance.GetSkill(_selectedSkill);
        nameText.text = skill.Name;
        descriptionText.text = skill.Description;
        typeText.text = $"Type: {skill.Type}";
        targetText.text = $"Target: {skill.AttackTarget}";
        powerText.text = $"Power: {skill.ImpactValue}";
        speedText.text = $"Speed: {skill.SkillSpeed}";
    }

    public void OnSelectClicked()
    {
        ElementalManager.Instance.EquipSkill(_elemental.Id, _skillSlot, _selectedSkill!);
        SoundManager.Instance.PlaySystemSFX(SystemSFXId.Click);

        gameObject.SetActive(false);
    }

    private void SkillChanged(SkillId skillId)
    {
        _selectedSkill = skillId;
        _equippedSkills[_skillSlot] = skillId;

        UpdateScrollView();
        UpdateInfo();
    }
}
