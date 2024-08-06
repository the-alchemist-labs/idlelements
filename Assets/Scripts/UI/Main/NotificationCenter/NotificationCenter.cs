using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class NotificationCenter : MonoBehaviour
{
    [SerializeField] ScrollRect scrollRect;
    [SerializeField] Transform scrollViewContent;
    [SerializeField] GameObject notificationPrefab;

    void Start()
    {
        GameEvents.OnNewNotification += UpdateUI;
    }

    void OnDestroy()
    {
        GameEvents.OnNewNotification -= UpdateUI;
    }

    private void UpdateUI()
    {
        scrollViewContent.Cast<Transform>().ToList().ForEach(child => Destroy(child.gameObject));

        foreach (Notification notification in NotificationManager.Instance.Notifications)
        {
            GameObject newEntry = Instantiate(notificationPrefab, scrollViewContent);
            if (newEntry.TryGetComponent(out NotificationPrefab item))
            {
                item.SetNotification(notification.Date, notification.Message);
            }
        }
    }
}
