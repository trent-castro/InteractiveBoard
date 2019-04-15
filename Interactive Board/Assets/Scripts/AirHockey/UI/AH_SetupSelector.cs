using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.AirHockey.UI;


public class AH_SetupSelector : MonoBehaviour
{
	
	[SerializeField] List<GameObject> m_selections;
	[SerializeField] List<Sprite> m_NeumontTheme;
	[SerializeField] List<Sprite> m_SpaceTheme;
	[SerializeField] List<Sprite> m_NatureTheme;
    // Start is called before the first frame update
    void Start()
    {
		SetTheme((int)AH_SetUpMenu.defaultTheme);
    }

	public void SetTheme(int theme)
	{
		List<Sprite> selection = m_NeumontTheme;
		switch ((ETheme)theme)
		{
			case ETheme.NEUMONT:
				selection = m_NeumontTheme;
				break;
			case ETheme.SPACE:
				selection = m_SpaceTheme;
				break;
			case ETheme.NATURE:
				selection = m_NatureTheme;
				break;
		}
		for (int i = 0; i < m_selections.Count; i++)
		{
			m_selections[i].GetComponent<Image>().sprite = selection[i];
		}
	}
}
