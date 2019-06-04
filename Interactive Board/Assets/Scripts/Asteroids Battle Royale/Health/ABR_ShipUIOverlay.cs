using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ABR_ShipUIOverlay : MonoBehaviour
{
    [Header("External References")]
	[Tooltip("The associated ship of the player's UI.")]
	[SerializeField]
    private ABR_Ship m_ship = null;
    [Tooltip("The associated health bar of the player's UI.")]
	[SerializeField]
    private Slider m_healthbar = null;
    [Tooltip("The associated text to display the current percentage of player health remaining.")]
	[SerializeField]
    private TextMeshProUGUI m_percentage = null;
    [Tooltip("The associated text to display the current equipped weapon of a player.")]
	[SerializeField]
    private TextMeshProUGUI m_weaponType = null;

    // Private Sibling Component
    /// <summary>
    /// A reference to the sibling ship health component.
    /// </summary>
	private ABR_ShipHealth m_health = null;

	private void Awake()
	{
		m_health = m_ship.GetComponent<ABR_ShipHealth>();
		m_health.m_healthBar = this;
		m_healthbar.maxValue = m_health.GetMaxHealth();
		m_ship.m_weaponPickupEvent = UpdateWeapon;
		UpdateHealth();
	}

    /// <summary>
    /// Updates the visuals for health based on the internal values.
    /// </summary>
	public void UpdateHealth()
	{
		m_healthbar.value = m_health.GetHealth();
		m_percentage.text = System.String.Format("{0:0}", ((m_health.GetHealth() / m_health.GetMaxHealth()) * 100)) + "%";
	}

    /// <summary>
    /// Updates the current weapon used to the correct weapon.
    /// </summary>
	public void UpdateWeapon()
	{
		if (m_weaponType) m_weaponType.text = "Turret: " + m_ship.GetWeaponType();
	}
}