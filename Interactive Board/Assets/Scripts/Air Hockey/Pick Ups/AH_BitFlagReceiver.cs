using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used to mark what signlas have already been received when receiving signals
/// </summary>
public class AH_BitFlagReceiver : MonoBehaviour
{
    /// Private Data Members
    // Bit flag tracking unsigned int
    private uint m_flags = 0;

    /// <summary>
    /// Checks whether or not a specific flag has been received or is already receiving
    /// </summary>
    /// <param name="flag">The inquiring flag value</param>
    /// <returns>A boolean describing whether or not the bit flag receiver has the flag in question</returns>
    public bool Contains(uint flag)
    {
        return (m_flags & flag) == flag;
    }

    /// <summary>
    /// Adds the flag to the bit flag tracking registry
    /// </summary>
    /// <param name="flag">The flag to add</param>
    public void AddFlag(uint flag)
    {
        m_flags += flag;
    }

    /// <summary>
    /// Unregisters a bit flag from the registry
    /// </summary>
    /// <param name="flag">The flag to remove</param>
    public void RemoveFlag(uint flag)
    {
        m_flags -= flag;
    }
} 