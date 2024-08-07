using System.Linq;
using TMPro;
using UnityEngine;

public class Catches : MonoBehaviour
{
    public TMP_Text catchesText;

    void Start()
    {
        GameEvents.OnElementalCaught += UpdateCatches;
        UpdateCatches();
    }

    void OnDestroy()
    {
        GameEvents.OnElementalCaught -= UpdateCatches;
    }

    void UpdateCatches()
    {
        catchesText.text = $"Caught {ElementalManager.Instance.ElementalCaught}/{ElementalCatalog.Instance.Count}";
    }

}
