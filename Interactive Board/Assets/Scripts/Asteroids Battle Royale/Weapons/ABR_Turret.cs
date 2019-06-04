using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ABR_Turret : MonoBehaviour
{
    [Header("Configuration")]
    [Tooltip("The Spawn Location for the bullets")]
    [SerializeField] Transform m_spawnLocation = null;
    [Tooltip("Different Types of weapon Sounds 0:Basic 1:Shotgun 2:Explosion 3:Pierce 4:Laser")]
    [SerializeField] AudioClip[] m_audioClips = new AudioClip[5];

    [Header("Debug")]
    [SerializeField] bool DebugMode = false;

    private AudioSource m_audioSource = null;
    private ABR_Weapon m_weapon = null;
    private Rigidbody2D m_rigidbody = null;

    //Properties
    public bool ContinuousFire { get; set; }
    public bool IsOkayToFire { get; private set; } = true;

    //private member variables
    private float m_fireTimer = 0.0f;
    private float m_fireTimeElapsed = 0.0f;

    /// <summary>
    /// 
    /// </summary>
    /// <returns>What percent of the relode has occured</returns>
    public float getReloadPercent()
    {
        return m_fireTimeElapsed / m_fireTimer;
    }


    private void Awake()
    {
        GetSiblingComponents();
        m_weapon.m_bulletSpawnLocation = m_spawnLocation;
        m_fireTimer = m_weapon.GetFireDelay();
    }
    /// <summary>
    /// Gets the sibling components needed for this component
    /// </summary>
    private void GetSiblingComponents()
    {
        if (m_audioSource == null)
        {
            m_audioSource = GetComponent<AudioSource>();
        }
        if (m_rigidbody == null)
            m_rigidbody = GetComponentInParent<Rigidbody2D>();
        if (m_weapon == null)
            m_weapon = GetComponent<ABR_Weapon>();

    }

    private void Update()
    {
        //checks to see if the gun was already fired
        if (!IsOkayToFire)
        {
            //if it was already fired, increment the timer
            m_fireTimeElapsed += Time.deltaTime;
            //checks to see if the timer has expired
            if (m_fireTimeElapsed >= m_fireTimer)
            {
                //reloads the weapon
                Reload();
            }
        }
        else
        {
            if (ContinuousFire)
            {
                FireBullet();
            }
        }
    }

    /// <summary>
    /// Resets the isOkayToFire boolean and the associated timer
    /// </summary>
    private void Reload()
    {
        IsOkayToFire = true;
        m_fireTimeElapsed = 0.0f;
    }

    /// <summary>
    /// Will fire a bullet in the direction of the spawn location
    /// </summary>
    public void FireBullet()
    {
        //fireDirection is the vector from the turret position to the spawnlocation
        Vector3 fireDirection = m_spawnLocation.position - gameObject.transform.position;
        if (IsOkayToFire)
        {
            //checks if the weapon successfully fired
            if (m_weapon.Fire(m_rigidbody.velocity))
            {
                //Plays Shot Audio
                PlaySound();
            }
            IsOkayToFire = false;
        }
        if (DebugMode)
        {
            Debug.Log("Weapon Type: " + m_weapon.GetType());
        }

    }

    /// <summary>
    /// Given the weapon type will play the sound assigned to the attached weapon
    /// </summary>
    public void PlaySound()
    {
        switch (m_weapon.GetBulletType())
        {
            case eBulletType.BASIC:
                m_audioSource.clip = m_audioClips[0];
                break;
            case eBulletType.SHOTGUN:
                m_audioSource.clip = m_audioClips[1];
                break;
            case eBulletType.EXPLOSION:
                m_audioSource.clip = m_audioClips[2];
                break;
            case eBulletType.PIERCE:
                m_audioSource.clip = m_audioClips[3];
                break;
            case eBulletType.LASER:
                m_audioSource.clip = m_audioClips[4];
                break;
        }
        m_audioSource.Play();

    }
    /// <summary>
    /// Gets the current weapon type
    /// </summary>
    /// <returns>The Current weapon type as a string</returns>
    public string GetWeaponType()
    {
        return m_weapon.GetBulletType().ToString();
    }

    /// <summary>
    /// switches the current weapon to the type passed in
    /// </summary>
    /// <param name="bulletType">The type that the weapon will change to</param>
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