using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct NotificationAction
{
    public string notificationType;
    public GameObject UINotification;
}

public class NotificationReciever : MonoBehaviour
{
    [SerializeField]
    private List<NotificationAction> m_notificationActions = new List<NotificationAction>();

    public void Notify(string type, bool activateNotification = true)
    {
        foreach(NotificationAction notificationAction in m_notificationActions)
        {
            if (notificationAction.notificationType == type)
            {
                notificationAction.UINotification.SetActive(activateNotification);
            }
        }
    }
}
