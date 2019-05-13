using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ABR_WeaponPickup : MonoBehaviour
{
    public eBulletType m_weaponType = default;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            print("Weapon pickup!");
            //TODO IMPLAMENT THIS PROPERLY!!!!

            //Get collision component of Powerup
            //Case/Switch powerup weapon type


            collision.gameObject.GetComponentInChildren<ABR_Turret>().SwitchWeapons(m_weaponType);
            
        }
    }
}
