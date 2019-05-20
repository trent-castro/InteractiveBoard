using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(ABR_Asteroid))]
public class ABR_AsteroidHealth : ABR_Health
{
	protected override void Die()
	{
		ABR_Asteroid asteroid = GetComponent<ABR_Asteroid>();
		asteroid.ReleaseItem();
		asteroid.Perish();
		//this.gameObject.SetActive(false);
	}

}
