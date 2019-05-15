using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ABR_GlobalInfo : MonoBehaviour
{
    [SerializeField]
    ObjectPoolManager m_bulletManager = null;
    [SerializeField]
    ObjectPoolManager m_weaponPickupManager = null;


    public static ObjectPoolManager BulletManager;
    public static ObjectPoolManager WeaponPickupManager;

    private void Awake()
    {
        BulletManager = m_bulletManager;
        WeaponPickupManager = m_weaponPickupManager;
    }
}
