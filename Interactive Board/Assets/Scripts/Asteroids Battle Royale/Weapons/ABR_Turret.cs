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

    private AudioSource m_audioSource = null;
    private ABR_Weapon m_weapon = null;
    private Rigidbody2D m_rigidbody = null;
    private bool m_isOkayToFire = true;

    private float m_fireTimer = 0.0f;
    private float m_fireTimeElapsed = 0.0f;

    private void Start()
    {
        if (m_audioSource == null)
        {
            m_audioSource = GetComponent<AudioSource>();
        }
        m_rigidbody = GetComponentInParent<Rigidbody2D>();
        m_weapon = GetComponent<ABR_Weapon>();
        m_weapon.m_bulletSpawnLocation = m_spawnLocation;
        m_fireTimer = m_weapon.GetFireDelay();
    }

    private void Update()
    {
        if (!m_isOkayToFire)
        {
            m_fireTimeElapsed += Time.deltaTime;
            if (m_fireTimeElapsed >= m_fireTimer)
            {
                Reload();
            }
        }
    }

    /// <summary>
    /// Resets the isOkayToFire boolean and the associated timer
    /// </summary>
    private void Reload()
    {
        m_isOkayToFire = true;
        m_fireTimeElapsed = 0.0f;
    }

    /// <summary>
    /// Will fire a bullet in the direction of the spawn location
    /// </summary>
    public void FireBullet()
    {
        //fireDirection is the vector from the turret position to the spawnlocation
        Vector3 fireDirection = m_spawnLocation.position - gameObject.transform.position;
        if (m_isOkayToFire)
        {
            //checks if the weapon successfully fired
            if (m_weapon.Fire(m_rigidbody.velocity))
            {
                //Plays Shot Audio
                m_audioSource.Play();
            }
            m_isOkayToFire = false;
        }
        if (DebugMode)
        {
            Debug.Log("Weapon Type: " + m_weapon.GetType());
        }

    }

    public void SwitchWeapons(eBulletType bulletType)
    {
        //Weapon reference to replace m_weapon
        ABR_Weapon newWeapon = null;

        //Determine Which weapon is going to be added and then add them
        switch (bulletType)
        {
            case eBulletType.BASIC:
                //New weapon will be a basic weapon
                newWeapon = gameObject.AddComponent<ABR_BasicWeapon>();
                break;
            case eBulletType.SHOTGUN:
                //newWeapon will be a shotgun weapon
                newWeapon = gameObject.AddComponent<ABR_ShotgunWeapon>();
                break;
            case eBulletType.EXPLOSION:
                //new weapon will be an explosion weapon
                newWeapon = gameObject.AddComponent<ABR_ExplosionWeapon>();
                break;
            case eBulletType.PIERCE:
                //new weapon will be a piece weapon
                newWeapon = gameObject.AddComponent<ABR_PierceWeapon>();
                break;
            case eBulletType.LASER:
                //new weapon will be a laser weapon
                newWeapon = gameObject.AddComponent<ABR_LaserWeapon>();
                break;
        }
        //set the fire timer to the weapons delay
        m_fireTimer = newWeapon.GetFireDelay();
        //set the bullet spawn location of the new weapon to the current spawn location
        newWeapon.m_bulletSpawnLocation = m_weapon.m_bulletSpawnLocation;
        //remove old weapon component
        Destroy(m_weapon);
        //set the m_weapon reference to the new Weapon
        m_weapon = newWeapon;
    }
}