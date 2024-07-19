using System;
using UnityEngine;

public class SpawnLevelUpToast : MonoBehaviour
{
    public GameObject toastPrefub;

    void Start()
    {
        GameEvents.OnLevelUp += DisplayToast;
    }

    void OnDestroy()
    {
        GameEvents.OnTriggerElementalToast -= DisplayToast;
    }

    void DisplayToast()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();

        RectTransform prefabRectTransform = toastPrefub.GetComponent<RectTransform>();
        Vector3 newPosition = rectTransform.position;
        newPosition.x -= prefabRectTransform.rect.width;

        GameObject toast = Instantiate(toastPrefub, newPosition, Quaternion.identity, transform.parent);
        toast.GetComponent<CatchToastPrefab>()?.DisplayToast(ElementalsData.Instance.GetElemental(ElementalsData.Instance.lastCaught));
    }
}
