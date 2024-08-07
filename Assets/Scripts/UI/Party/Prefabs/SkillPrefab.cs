using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillPrefab : MonoBehaviour
{
    public event Action<SkillId> OnSkillChanged;
    
    [SerializeField] private Button button;
    [SerializeField] private Image skillImage;
    [SerializeField] private Image selectedIndicator;
    [SerializeField] private GameObject lockedIndicator;
    [SerializeField] private GameObject equipedIndicator;
    [SerializeField] private TMP_Text levelToUnlockText;
    [SerializeField] private Material blackAndWhiteMaterial;

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

        selectedIndicator.color = isSelected ? Color.white : Color.grey;
        skillImage.sprite = Resources.Load<Sprite>($"Sprites/Skills/{id}");
        skillImage.material = _basicMaterial;
        lockedIndicator.SetActive(isLocked);
        equipedIndicator.SetActive(isEquiped && !isSelected);

        skillImage.material = (isLocked) ? blackAndWhiteMaterial : _basicMaterial;
        button.interactable = !isLocked && ! isEquiped;
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
