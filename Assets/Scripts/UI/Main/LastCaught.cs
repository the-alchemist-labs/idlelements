using TMPro;
using UnityEngine;

public class LastCaught : MonoBehaviour
{
    public TMP_Text elementalNameText;

    void Start()
    {
        GameEvents.OnElementalCaught += UpdateLastCaught;
        UpdateLastCaught();
    }

    void OnDestroy()
    {
        GameEvents.OnElementalCaught -= UpdateLastCaught;
    }

    public void UpdateLastCaught()
    {
        elementalNameText.text = State.Elementals.GetElement(State.lastCaught)?.name ?? "None";
    }
}