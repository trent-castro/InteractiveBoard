using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A script that allows asteroids to be destroyed.
/// </summary>
[RequireComponent (typeof(ABR_Asteroid))]
public class ABR_AsteroidHealth : ABR_Health
{
	protected override void Die()
	{
		ABR_Asteroid asteroid = GetComponent<ABR_Asteroid>();
		asteroid.ReleaseItem();
		asteroid.Perish();
	}
}
