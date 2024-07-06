using System;
using UnityEngine;

public class SpawnCatchToast : MonoBehaviour
{
    public GameObject toastPrefub;

    void Start()
    {
        GameEvents.OnElementalCaught += DisplayToast;
    }

    void OnDestroy()
    {
        GameEvents.OnElementalCaught -= DisplayToast;
    }

    void DisplayToast()
    {
        // Don't show toast for idle encounter
        if ((DateTime.Now - State.lastEncounterDate).TotalSeconds >= 1) return;

        RectTransform rectTransform = GetComponent<RectTransform>();

        RectTransform prefabRectTransform = toastPrefub.GetComponent<RectTransform>();
        Vector3 newPosition = rectTransform.position;
        newPosition.x -= prefabRectTransform.rect.width;

        GameObject toast = Instantiate(toastPrefub, newPosition, Quaternion.identity, transform.parent);
        toast.GetComponent<CatchToastPrefab>().DisplayToast(State.Elementals.GetElemental(State.lastCaught));
    }
}
