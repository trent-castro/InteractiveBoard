using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ABR_TouchDamage : MonoBehaviour
{
	[SerializeField] bool isInstantKill = true;
	[SerializeField] float m_damage = 0;
	private void OnCollisionEnter2D(Collision2D collision)
	{
		ABR_ShipHealth ship = collision.gameObject.GetComponent<ABR_ShipHealth>();
		if (ship)
		{
			ship.TakeDamage(isInstantKill ? ship.GetMaxHealth() : m_damage);
		}
	}
}
