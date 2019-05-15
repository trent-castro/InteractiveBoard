using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

	public void ReturntoAracde()
	{
		Time.timeScale = 1.0f;
		SceneManager.LoadScene("ArcadeMenu");
	}
}
