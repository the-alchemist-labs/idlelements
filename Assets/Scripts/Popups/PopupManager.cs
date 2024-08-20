using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum PopupId
{
    Background,
    AfkGains,
    SelectPartyMember,
    SelectSkill,
    Evolve,
    CelebrateEvolution,
    Deck,
    FriendRequest,
    Settings,
    PlayerInfo,
}

public class PopupManager : MonoBehaviour
{
    public static PopupManager Instance;

    private List<BasePopup> _popupList;
    private Image _background;
    private BasePopup _currentPopup;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Init();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public T OpenPopUp<T>(PopupId popupId)
    {
        _background.gameObject.SetActive(true);
        BasePopup popup = _popupList.Single(p => p.Id == popupId);
        _currentPopup?.gameObject.SetActive(false);
        popup.gameObject.SetActive(true);
        _currentPopup = popup;
        SubscribeToClickOutsideEvent(_currentPopup);

        return popup.GetComponent<T>();
    }

    public void OpenPopUp(PopupId popupId)
    {
        OpenPopUp<MonoBehaviour>(popupId);
    }

    public void ClosePopup(PopupId popupId)
    {
        BasePopup popup = _popupList.Single(p => p.Id == popupId);
        popup.gameObject.SetActive(false);

        if (_currentPopup?.Id == popupId)
        {
            _background.gameObject.SetActive(false);
            _currentPopup = null;
        }
    }

    public void ClosePopup()
    {
        if (_currentPopup != null)
        {
            _background.gameObject.SetActive(false);
            _currentPopup?.gameObject?.SetActive(false);
            _currentPopup = null;
        }
    }

    private void Init()
    {
        Canvas canvas = transform.GetComponentInChildren<Canvas>();
        _popupList = canvas.transform.Cast<Transform>().Select(t => t.GetComponent<BasePopup>()).Where(p => p != null).ToList();
        _background = _popupList.Single(p => p.Id == PopupId.Background).gameObject.GetComponent<Image>();
        TabManager.OnTabChanged += ClosePopup;
    }

    private void SubscribeToClickOutsideEvent(BasePopup popup)
    {
        EventSystem.current.SetSelectedGameObject(popup.gameObject);
        EventTrigger trigger = popup.GetComponent<EventTrigger>();
        if (trigger == null)
        {
            trigger = popup.gameObject.AddComponent<EventTrigger>();
        }

        EventTrigger.Entry entry = new EventTrigger.Entry { eventID = EventTriggerType.PointerClick };
        entry.callback.AddListener((data) =>
        {
            PointerEventData pointerData = (PointerEventData)data;
            if (!RectTransformUtility.RectangleContainsScreenPoint(
                    popup.GetComponent<RectTransform>(),
                    pointerData.position,
                    Camera.main
                ))
            {
                ClosePopup(popup.Id);
            }
        });

        trigger.triggers.Add(entry);
    }
}