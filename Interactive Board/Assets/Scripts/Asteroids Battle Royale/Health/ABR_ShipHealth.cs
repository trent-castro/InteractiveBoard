using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ABR_ShipHealth : ABR_Health
{
	[SerializeField] GameObject DeathUI = null;
	public ABR_HealthBar m_healthBar = null;


	public override void TakeDamage(float damage)
	{
		base.TakeDamage(damage);
		if (m_healthBar) m_healthBar.UpdateHealth();
	}
	protected override void Die()
	{
		m_isAlive = false;
		//this.gameObject.SetActive(false);
		if (DeathUI) DeathUI.SetActive(true);
	}
}
