using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AH_BitFlagReceiver : MonoBehaviour
{
    private uint m_flags = 0;

    public bool CheckForFlag(uint flag)
    {
        return (m_flags & flag) == flag;
    }

    public void AddFlag(uint flag)
    {
        m_flags += flag;
    }

    public void RemoveFlag(uint flag)
    {
        m_flags -= flag;
    }
} 