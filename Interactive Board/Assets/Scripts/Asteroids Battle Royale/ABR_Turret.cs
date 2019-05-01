using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ABR_Turret : MonoBehaviour
{
    [Header("Configuration")]
    [Tooltip("The Spawn Location for the bullets")]
    [SerializeField] Transform m_spawnLocation = null;
    [Tooltip("Refference to the respecive players bullet pool")]

    [Header("Debug")]
    [SerializeField] bool DebugMode = false;
    [SerializeField] eBulletType bulletType = eBulletType.BASIC;

    private AudioSource m_audioSource = null;
    private Weapon m_weapon = new BasicWeapon();

    private void Start()
    {
        if (m_audioSource == null)
        {
            m_audioSource = GetComponent<AudioSource>();
        }
        if (DebugMode)
        {
            switch (bulletType)
            {
                case eBulletType.BASIC:
                    m_weapon = new BasicWeapon();
                    break;
                case eBulletType.SHOTGUN:
                    m_weapon = new Shotgun();
                    break;
                case eBulletType.EXPLOSION:
                    m_weapon = new Explosion();
                    break;
                case eBulletType.PIERCE:
                    m_weapon = new Pierce();
                    break;
                case eBulletType.LASER:
                    m_weapon = new Laser();
                    break;
            }
        }
    }
    /// <summary>
    /// Will fire a bullet in the direction that the spawn location
    /// </summary>
    public void FireBullet()
    {
        Vector3 fireDirection = m_spawnLocation.position - gameObject.transform.position;
        Debug.Log(m_weapon.GetType());
        m_weapon.Fire(m_spawnLocation.position, fireDirection);
        //Plays Shot Audio
        m_audioSource.Play();

    }
}