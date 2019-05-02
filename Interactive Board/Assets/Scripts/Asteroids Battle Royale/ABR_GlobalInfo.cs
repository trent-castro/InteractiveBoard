using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ABR_GlobalInfo : MonoBehaviour
{
    [SerializeField]
    ObjectPoolManager m_bulletManager = null;

    public static ObjectPoolManager BulletManager;

    private void Awake()
    {
        BulletManager = m_bulletManager;
    }
}
