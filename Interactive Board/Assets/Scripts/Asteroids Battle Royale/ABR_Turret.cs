using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ABR_Turret : MonoBehaviour
{
    [Header("Configuration")]
    [Tooltip("The Spawn Location for the bullets")]
    [SerializeField] Transform m_spawnLocation = null;
    [Tooltip("Refference to the respecive players bullet pool")]
    [SerializeField] ObjectPool m_bulletPool = null;
    [Tooltip("Refference to the AudioSource attached to the object")]
    [SerializeField] AudioSource m_audioSource = null;

    [Header("Debug")]
    [SerializeField] bool DebugMode = false;

    private void Start()
    {
        if(m_audioSource == null)
        {
            m_audioSource = GetComponent<AudioSource>();
        }
    }

    private void Update()
    {
        if (DebugMode)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("Firing Bullet");
                FireBullet();
            }
        }
    }
    /// <summary>
    /// Will fire a bullet in the direction that the spawn location
    /// </summary>
    public void FireBullet()
    {
        //Get bullet from the bullet pool
        GameObject bullet = m_bulletPool.GetNextPooledObject();
        //Set bullet position to the desired spawn locaion
        bullet.transform.position = m_spawnLocation.position;
        //Activate the bullet
        bullet.gameObject.SetActive(true);

        //Determine the direction the bullet will travel
        Vector3 fireDirection = m_spawnLocation.position - gameObject.transform.position;

        //fire the bullet in the desired direction
        bullet.GetComponent<ABR_Bullet>().Fire(fireDirection);
        //Plays Shot Audio
        m_audioSource.Play();

    }

}
