using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AsteroidsPlayerSelect : MonoBehaviour
{
	[SerializeField] TextMeshProUGUI m_playerCountText = null;
	[SerializeField] [Range (1, 4)] int m_playerCount = 4;
	[SerializeField] GameObject[] m_ships = null;
    // Start is called before the first frame update
    void Start()
    {
		m_playerCountText.text = m_playerCount.ToString();
		ChangePlayerCount(0);
	}

	public void ChangePlayerCount(int offset)
	{
		m_playerCount = Mathf.Clamp(m_playerCount + offset, 1, 4);
		m_playerCountText.text = m_playerCount.ToString();

		
		for (int i = 0; i < m_ships.Length; i++)
		{
			if (i < m_playerCount) m_ships[i].gameObject.SetActive(true);
			else m_ships[i].gameObject.SetActive(false);
		}
	}

    
}
