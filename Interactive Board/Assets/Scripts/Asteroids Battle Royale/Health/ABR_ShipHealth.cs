using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ABR_ShipHealth : ABR_Health
{
	[SerializeField] GameObject DeathUI = null;
	protected override void Die()
	{
		m_isAlive = false;
		//this.gameObject.SetActive(false);
		if (DeathUI) DeathUI.SetActive(true);
	}
}
