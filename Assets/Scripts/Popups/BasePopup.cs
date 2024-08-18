using System;
using UnityEngine;
using UnityEngine.UI;

public abstract class BasePopup : MonoBehaviour
{
    public abstract PopupId Id { get; }
    private GameObject _background;

    void OnEnable()
    {
        _background?.SetActive(true);
    }

    void OnDisable()
    {
        _background?.SetActive(false);
    }
    protected void SetupCloseableBackground (bool isTainted = false)
    {    Canvas rootCanvas = GetComponentInParent<Canvas>();

        _background = new GameObject("Popup_background");
        _background.transform.SetParent(rootCanvas.transform, false); 
        _background.transform.SetAsFirstSibling();

        RectTransform rectTransform = _background.AddComponent<RectTransform>();
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.one;
        rectTransform.offsetMin = Vector2.zero;
        rectTransform.offsetMax = Vector2.zero;

        _background.AddComponent<CanvasRenderer>();
        Image image = _background.AddComponent<Image>();
        image.color = new Color(0, 0, 0,  isTainted ? 0.5f : 0f);

        Button backgroundButton = _background.AddComponent<Button>();
        backgroundButton.onClick.AddListener(ClosePopup);
    }
    
    protected virtual void ClosePopup()
    {
        _background.SetActive(false);
        PopupManager.Instance.ClosePopup(Id);
    }
}