using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectMemberInfo : MonoBehaviour
{
    [SerializeField] TMP_Text typeText;
    [SerializeField] TMP_Text nameText;
    [SerializeField] TMP_Text hpText;
    [SerializeField] TMP_Text attackText;
    [SerializeField] TMP_Text defenseText;

    [SerializeField] TMP_Text speedText;
    [SerializeField] ScrollRect scrollRect;
    [SerializeField] Transform scrollViewContent;
    [SerializeField] GameObject skillPrefub;

    public void Init(ElementalId elementalId)
    {
        print(elementalId);
        Elemental elemental = ElementalCatalog.Instance.GetElemental(elementalId);
        int level = Player.Instance.Level;
        
        typeText.text = $"Type: {elemental.Type}";
        nameText.text = elemental.Name;
        hpText.text = $"{elemental.Stats.Hp * level}";
        attackText.text = $"{elemental.Stats.Attack + level}";
        defenseText.text = $"{elemental.Stats.Defense + level}";
        speedText.text = $"{elemental.Stats.Speed + level}";
    
        UpdateSkillView(elemental);
    }

    void UpdateSkillView(Elemental elemental)
    {
        scrollViewContent.Cast<Transform>().ToList().ForEach(child => Destroy(child.gameObject));

        foreach (SkillByLevel skillByLevel in elemental.Skills)
        {
            GameObject newEntry = Instantiate(skillPrefub, scrollViewContent);
            if (newEntry.TryGetComponent(out SkillPrefab item))
            {
                item.Init(skillByLevel.SkillId, skillByLevel.Level);
            }
        }

        scrollRect.verticalNormalizedPosition = 1f;
    }
}
