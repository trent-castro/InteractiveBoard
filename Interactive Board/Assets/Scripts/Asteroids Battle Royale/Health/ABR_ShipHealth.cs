using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ABR_ShipHealth : ABR_Health
{
	[SerializeField] GameObject DeathUI = null;
	[SerializeField] GameObject m_root = null;
	[HideInInspector] public ABR_ShipUIOverlay m_healthBar = null;


	public override void TakeDamage(float damage)
	{
		base.TakeDamage(damage);
		if (m_healthBar) m_healthBar.UpdateHealth();
	}

	public override void Respawn()
	{
		base.Respawn();
		if (m_healthBar) m_healthBar.UpdateHealth();
	}
	protected override void Die()
	{
        base.Die();
		m_isAlive = false;
		if (m_root) m_root.SetActive(false);
		if (DeathUI) DeathUI.SetActive(true);
	}
}
