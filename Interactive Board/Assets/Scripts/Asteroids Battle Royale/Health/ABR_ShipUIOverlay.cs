using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ABR_ShipUIOverlay : MonoBehaviour
{
	[Header("Associated Ship")]
	[SerializeField] ABR_Ship m_ship = null;

	[Header("UI Elements")]
	[SerializeField] Slider m_healthbar = null;
	[SerializeField] TextMeshProUGUI m_percentage = null;
	[SerializeField] TextMeshProUGUI m_weaponType = null;

	ABR_ShipHealth m_health = null;

	private void Awake()
	{
		m_health = m_ship.GetComponent<ABR_ShipHealth>();
		m_health.m_healthBar = this;
		m_healthbar.maxValue = m_health.GetMaxHealth();
		m_ship.m_weaponPickupEvent = UpdateWeapon;
		UpdateHealth();
	}

	public void UpdateHealth()
	{
		
		m_healthbar.value = m_health.GetHealth();
		m_percentage.text = System.String.Format("{0:0}", ((m_health.GetHealth() / m_health.GetMaxHealth()) * 100)) + "%";
	}

	public void UpdateWeapon()
	{
		if (m_weaponType) m_weaponType.text = "Turret: " + m_ship.GetWeaponType();
	}
}
