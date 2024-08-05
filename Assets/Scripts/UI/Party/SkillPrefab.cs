using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillPrefab : MonoBehaviour
{
    public event Action<SkillId> OnSkillChanged;
    
    [SerializeField] Image skillImage;
    [SerializeField] Image selectedIndicator;
    [SerializeField] GameObject lockedIndicator;
    [SerializeField] GameObject equipedIndicator;
    [SerializeField] TMP_Text levelToUnlockText;
    [SerializeField] Material blackAndWhiteMaterial;

    private SkillId _skillId;
    private Material _basicMaterial;

    void Start()
    {
        _basicMaterial = skillImage.material;
    }

    public void Init(SkillId id, int requiredLevel, bool isSelected = false, bool isEquiped = false)
    {
        _skillId = id;
        bool isLocked = requiredLevel > Player.Instance.Level;
        bool isEquipeAndNotSelected = isEquiped && !isSelected;

        selectedIndicator.color = isSelected ? Color.white : Color.grey;
        skillImage.sprite = Resources.Load<Sprite>($"Sprites/Skills/{id}");
        skillImage.material = _basicMaterial;
        lockedIndicator.SetActive(isLocked);
        equipedIndicator.SetActive(isEquipeAndNotSelected);

        skillImage.material = (isLocked || isEquipeAndNotSelected) ? blackAndWhiteMaterial : _basicMaterial;

        if (isLocked)
        {
            levelToUnlockText.text = requiredLevel.ToString();
        }
    }

    public void SkillClicked()
    {
        OnSkillChanged?.Invoke(_skillId);
    }

}
