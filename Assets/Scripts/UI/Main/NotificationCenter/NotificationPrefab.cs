using System;
using TMPro;
using UnityEngine;

public class NotificationPrefab : MonoBehaviour
{
    [SerializeField] TMP_Text timeText;
    [SerializeField] TMP_Text messageText;

    public void SetNotification(DateTime time, string message)
    {
        timeText.text = $"{time.ToString("HH:mm:ss")} -";
        messageText.text = message;
    }
}
