using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// A class that has static references to singletons for accessibility reasons.
/// </summary>
public class ABR_GlobalInfo : MonoBehaviour
{
    [Header("External Reference")]
    [Tooltip("An external reference to the bullet manager's object pool manager script.")]
    [SerializeField]
    ObjectPoolManager m_bulletManager = null;
    [Tooltip("An external reference to the weapon pick up manager's object pool manager script.")]
    [SerializeField]
    ObjectPoolManager m_weaponPickupManager = null;
    
    /// <summary>
    /// A public static reference to the bullet manager's object pool manager.
    /// </summary>
    public static ObjectPoolManager BulletManager;
    /// <summary>
    /// A public static reference to the weapon pick up manager's object pool manager.
    /// </summary>
    public static ObjectPoolManager WeaponPickupManager;

    /// <summary>
    /// Sets up manager references for use globally.
    /// </summary>
    private void Awake()
    {
        BulletManager = m_bulletManager;
        WeaponPickupManager = m_weaponPickupManager;
    }

    /// <summary>
    /// Returns the user to the arcade menu.
    /// </summary>
	public void ReturntoAracde()
	{
		Time.timeScale = 1.0f;
		SceneManager.LoadScene("ArcadeMenu");
	}
}