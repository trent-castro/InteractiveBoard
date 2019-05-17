﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ABR_ShipUIOverlay : MonoBehaviour
{
	[SerializeField] Slider m_healthbar = null;
	[SerializeField] ABR_Ship m_ship = null;

	ABR_ShipHealth m_health = null;

	private void Awake()
	{
		m_health = m_ship.GetComponent<ABR_ShipHealth>();
		m_health.m_healthBar = this;
		m_healthbar.maxValue = m_health.GetMaxHealth();
		UpdateHealth();
	}
    
	public void UpdateHealth()
	{
		m_healthbar.value = m_health.GetHealth();
	}
}