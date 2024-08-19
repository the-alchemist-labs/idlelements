using UnityEngine;

public class DisplayPopup : MonoBehaviour
{
    [SerializeField] PopupId popupId;

    public void OpenPanel()
    {
        PopupManager.Instance.OpenPopUp(popupId);
    }

    public void ClosePanel()
    {
        PopupManager.Instance.ClosePopup(popupId);
    }
}
