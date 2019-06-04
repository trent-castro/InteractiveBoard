using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A script that enables the object to deal damage based on collisions.
/// </summary>
public class ABR_TouchDamage : MonoBehaviour
{
    [Header("Configuration")]
    [Tooltip("Whether or not the object will instantly kill on collision.")]
	[SerializeField]
    private bool isInstantKill = true;
    [Tooltip("The amount of damage dealt upon collision.")]
	[SerializeField]
    private float m_damage = 0;

	private void OnCollisionEnter2D(Collision2D collision)
	{
		ABR_ShipHealth ship = collision.gameObject.GetComponent<ABR_ShipHealth>();
		if (ship)
		{
			ship.TakeDamage(isInstantKill ? ship.GetMaxHealth() : m_damage);
		}
	}
}
