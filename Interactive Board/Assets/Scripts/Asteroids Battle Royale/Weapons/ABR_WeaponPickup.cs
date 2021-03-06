﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A script that allows a player to change the equipped weapon based on the contained enum.
/// </summary>
public class ABR_WeaponPickup : MonoBehaviour
{
    public eBulletType m_weaponType = default;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(ABR_Tags.PlayerTag))
        {
            collision.gameObject.GetComponentInChildren<ABR_Turret>().SwitchWeapons(m_weaponType);
            Destroy(gameObject);
        }
    }
}