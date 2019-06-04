using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ABR_ShipHealth : ABR_Health
{
	[Header ("External References")]
    [Tooltip("A reference to the player's death UI.")]
	[SerializeField]
    private GameObject DeathUI = null;
    [Tooltip("A reference to the root game object of the player's UI.")]
	[SerializeField]
    private GameObject m_root = null;

    // The unique identifier for the ship.  
	public int ShipId = 0;

    /// <summary>
    /// A reference to the ship's UI overlay's health bar.
    /// </summary>
	[HideInInspector] public ABR_ShipUIOverlay m_healthBar = null;

    /// <summary>
    /// A method that allows an object to take a certain amount of damage.
    /// </summary>
    /// <param name="damage">The amount of current health the object will be losing.</param>
    public override void TakeDamage(float damage)
	{
		base.TakeDamage(damage);
		if (m_healthBar) m_healthBar.UpdateHealth();
	}

    /// <summary>
    /// Resets the internal values for the object's health.
    /// </summary>
    public override void Respawn()
	{
		base.Respawn();

        if (m_healthBar)
        {
            m_healthBar.UpdateHealth();
        }
	}
    
    /// <summary>
    /// Causes the object to trigger an on death method as well as stops the object from becoming invincible.
    /// </summary>
    protected override void Die()
	{
        base.Die();

        if (m_root)
        {
            m_root.SetActive(false);
        }
        if (DeathUI)
        {
            DeathUI.SetActive(true);
        }

        ABR_Gameloop.instance.CheckGameState();
	}
}
