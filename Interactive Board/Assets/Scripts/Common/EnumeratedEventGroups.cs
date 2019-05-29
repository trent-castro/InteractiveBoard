using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnumeratedEventGroups : MonoBehaviour
{
    public List<UnityEvent> m_eventGroups = new List<UnityEvent>();

    public void RunGroup(int index)
    {
        m_eventGroups[index].Invoke();
    }
}
