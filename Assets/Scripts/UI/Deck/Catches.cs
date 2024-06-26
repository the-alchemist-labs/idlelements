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
        int caughtNumber = State.Elementals.entries.Count(entry => entry.isCaught);
        catchesText.text = $"Caught {caughtNumber}/{State.Elementals.all.Count}";
    }

}
