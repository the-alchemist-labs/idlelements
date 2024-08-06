using System.Collections.Generic;
using UnityEngine;

public class NotificationManager : MonoBehaviour
{
    public static NotificationManager Instance;
    public List<Notification> Notifications { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        Notifications = new List<Notification>();
    }

    public void PostNotification(string message)
    {
        Notifications.Add(new Notification(message));
        GameEvents.NewNotification();
    }
}