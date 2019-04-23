using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used when attempting to send a signal to a receiver.
/// </summary>
public class AH_BitFlagBroadcaster : MonoBehaviour
{
    // Disabled, but this is useful if the system was better.
    //[SerializeField]
    //[Tooltip("Broadcast Name")]
    //private string m_broadcastName;

    // There are several things wrong with this implementation
    [SerializeField]
    [Tooltip("Broadcast ID")]
    private uint m_broadcastID = 0;

    /// <summary>
    /// Sends out the broadcast ID
    /// </summary>
    /// <returns></returns>
    public uint Broadcast()
    {
        return m_broadcastID;
    }
}
