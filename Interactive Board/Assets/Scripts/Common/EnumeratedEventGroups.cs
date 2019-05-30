using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public struct EventWithName
{
    public string name;
    public UnityEvent events;
}

public class EnumeratedEventGroups : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI m_display = null;

    public List<EventWithName> m_eventGroups = new List<EventWithName>();

    private int m_lastIndex = 0;

    public void RunGroup(int index)
    {
        m_eventGroups[index].events.Invoke();
        m_lastIndex = index;
    }

    public void Up()
    {
        int index = m_lastIndex + 1;
        index %= m_eventGroups.Count;
        RunGroup(index);
    }

    public void Down()
    {
        int index = m_lastIndex - 1 + m_eventGroups.Count;
        index %= m_eventGroups.Count;
        RunGroup(index);
    }
}
