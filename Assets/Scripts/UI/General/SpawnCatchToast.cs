using System;
using UnityEngine;

public class SpawnCatchToast : MonoBehaviour
{
    public GameObject toastPrefub;

    void Start()
    {
        GameEvents.OnTriggerElementalToast += DisplayToast;
    }

    void OnDestroy()
    {
        GameEvents.OnTriggerElementalToast -= DisplayToast;
    }

    void DisplayToast()
    {
        // Don't show toast for idle encounter
        if ((DateTime.Now - ElementalsData.Instance.lastEncounterDate).TotalSeconds >= 1) return;

        RectTransform rectTransform = GetComponent<RectTransform>();

        RectTransform prefabRectTransform = toastPrefub.GetComponent<RectTransform>();
        Vector3 newPosition = rectTransform.position;
        newPosition.x -= prefabRectTransform.rect.width;

        GameObject toast = Instantiate(toastPrefub, newPosition, Quaternion.identity, transform.parent);
        toast.GetComponent<CatchToastPrefab>().DisplayToast(ElementalsData.Instance.GetElemental(ElementalsData.Instance.lastCaught));
    }
}
