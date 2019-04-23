﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AH_GameMaster : MonoBehaviour
{
    [SerializeField] TextMeshPro[] m_scores = null;
    [SerializeField] TextMeshProUGUI[] m_winLossPanels = null;

    [SerializeField]
    private int m_player1Score, m_player2Score = 0;
    [SerializeField]
    private int m_scoreToWin = 8;
    [SerializeField]
    GameObject m_PlayAgainButton = null;
    [SerializeField]
    GameObject m_VictoryParticles = null;

    private bool m_gameIsWon = false;

    public void GivePointToPlayer(bool isRightPlayer, Collider2D collision)
    {
        if (isRightPlayer)
        {
            m_player2Score++;
            m_scores[0].text = "" + m_player2Score;
        }
        else
        {
            m_player1Score++;
            m_scores[1].text = "" + m_player1Score;
        }

        AH_Puck scoringPuck = collision.gameObject.GetComponent<AH_Puck>();
        scoringPuck.GetComponentInChildren<TrailRenderer>().enabled = false;
        StartCoroutine(PuckResetCoroutine(scoringPuck));
    }

    IEnumerator PuckResetCoroutine(AH_Puck scoringPuck)
    {
        yield return new WaitForSecondsRealtime(0.5f);
        scoringPuck.ResetPuck();
        if (DetermineWinState() && !m_gameIsWon)
        {
            //End Game Logic Stop players from playing
            //Time.timeScale = 0;
            m_VictoryParticles.SetActive(true);
            m_gameIsWon = true;
            StartCoroutine(VictoryCoroutine());
  
        }
        StopCoroutine(PuckResetCoroutine(scoringPuck));
    }

    IEnumerator VictoryCoroutine()
    {
        GetComponent<AudioSource>().Play();
        yield return new WaitForSecondsRealtime(1.5f);
        Time.timeScale = 0.0f;
        if (m_PlayAgainButton)
        {
            m_PlayAgainButton.SetActive(true);
        }
        StopCoroutine(VictoryCoroutine());
    }

    private bool DetermineWinState()
    {
        bool player1Win = m_player1Score >= m_scoreToWin;
        bool player2Win = m_player2Score >= m_scoreToWin;
        if (player1Win)
        {
            m_winLossPanels[0].text = "LOSE!!";
            m_winLossPanels[1].text = "WIN!!";
            return true;
        }
        else if (player2Win)
        {
            m_winLossPanels[0].text = "WIN!!";
            m_winLossPanels[1].text = "LOSE!!";
            return true;
        }
        else return false;        
    }

	public void SetTimeScale(float timescale)
	{
		Time.timeScale = timescale;
	}
}
