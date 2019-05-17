using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowRotation : MonoBehaviour
{
    [SerializeField]
    private Transform m_target = null;

    void Update()
    {
        transform.rotation = m_target.rotation;
    }
}
