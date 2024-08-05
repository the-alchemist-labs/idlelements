using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectSkillPanel : MonoBehaviour
{
    [SerializeField] ScrollRect scrollRect;
    [SerializeField] Transform scrollViewContent;
    [SerializeField] GameObject prefub;

    [SerializeField] TMP_Text nameText;
    [SerializeField] TMP_Text descriptionText;
    [SerializeField] TMP_Text typeText;
    [SerializeField] TMP_Text targetText;
    [SerializeField] TMP_Text powerText;
    [SerializeField] TMP_Text speedText;

    private Elemental _elemental;
    private int _skillSlot;
    private SkillId _selectedSkill;
    private List<SkillId?> _equipedSkills;

    public void OpenPanel(ElementalId elementalId, int slot)
    {
        gameObject.SetActive(true);

        _skillSlot = slot;
        _elemental = ElementalCatalog.Instance.GetElemental(elementalId);
        _equipedSkills = ElementalManager.Instance.GetSkills(_elemental.Id);

        UpdateScrollView();
        UpdateInfo();
    }

    void UpdateScrollView()
    {
        scrollViewContent.Cast<Transform>().ToList().ForEach(child => Destroy(child.gameObject));
        
        foreach (SkillByLevel skillByLevel in _elemental.Skills)
        {
            GameObject newEntry = Instantiate(prefub, scrollViewContent);
            if (newEntry.TryGetComponent(out SkillPrefab item))
            {
                item.Init(
                    skillByLevel.SkillId,
                    skillByLevel.Level,
                    _selectedSkill == skillByLevel.SkillId,
                    _equipedSkills.Contains(skillByLevel.SkillId)
                );
                item.OnSkillChanged += SkillChanged;
            }
        }
    }

    void UpdateInfo()
    {
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
        ElementalManager.Instance.EquipSkill(_elemental.Id, _skillSlot, _selectedSkill);
        SoundManager.Instance.PlaySystemSFX(SystemSFXId.Click);

        gameObject.SetActive(false);
    }

    private void SkillChanged(SkillId skillId)
    {
        _selectedSkill = skillId;
        _equipedSkills[_skillSlot] = skillId;

        UpdateScrollView();
        UpdateInfo();
    }
}
