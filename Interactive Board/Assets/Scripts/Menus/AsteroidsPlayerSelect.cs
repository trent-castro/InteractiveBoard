using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AsteroidsPlayerSelect : MonoBehaviour
{
	[SerializeField] TextMeshProUGUI m_playerCountText = null;
    private const int MIN_PLAYER_COUNT = 1;
    private const int MAX_PLAYER_COUNT = 4;
    [SerializeField] [Range (1, MAX_PLAYER_COUNT)] int m_playerCount = MAX_PLAYER_COUNT;
	[SerializeField] GameObject[] m_ships = null;
    [SerializeField] GameObject[] m_visibleArrows = null;

    // Start is called before the first frame update
    void Start()
    {
		m_playerCountText.text = m_playerCount.ToString();
		ChangePlayerCount(0);
	}

	public void ChangePlayerCount(int offset)
	{
		m_playerCount = Mathf.Clamp(m_playerCount + offset, MIN_PLAYER_COUNT, MAX_PLAYER_COUNT);
		m_playerCountText.text = m_playerCount.ToString();

		
		for (int i = 0; i < m_ships.Length; i++)
		{
			if (i < m_playerCount) m_ships[i].gameObject.SetActive(true);
			else m_ships[i].gameObject.SetActive(false);
		}
        
        switch(m_playerCount)
        {
            case MIN_PLAYER_COUNT:
                m_visibleArrows[0].SetActive(false);
                break;
            case MIN_PLAYER_COUNT + 1:
                m_visibleArrows[0].SetActive(true);
                break;
            case MAX_PLAYER_COUNT - 1:
                m_visibleArrows[1].SetActive(true);
                break;
            case MAX_PLAYER_COUNT:
                m_visibleArrows[1].SetActive(false);
                break;
            default:
                Debug.Log("Unreachable code detected");
                break;
        }
	}

	public void StartGame()
	{
		PlayerPrefs.SetInt("ABR_PlayerCount", m_playerCount);
	}
}