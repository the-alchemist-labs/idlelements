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
        GameEvents.OnLevelUp -= DisplayToast;
    }

    void DisplayToast()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();

        RectTransform prefabRectTransform = toastPrefub.GetComponent<RectTransform>();
        Vector3 newPosition = rectTransform.position;
        newPosition.x -= prefabRectTransform.rect.width;

        GameObject toast = Instantiate(toastPrefub, newPosition, Quaternion.identity, transform.parent);
        toast.GetComponent<LevelUpToastPrefab>()?.DisplayToast();
    }
}
