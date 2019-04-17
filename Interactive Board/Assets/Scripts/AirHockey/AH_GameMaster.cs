﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AH_GameMaster : MonoBehaviour
{
    [SerializeField] TextMeshPro[] m_scores = null;

    [SerializeField]
    private int m_player1Score, m_player2Score = 0;
    [SerializeField]
    private int m_scoreToWin = 8;
    [SerializeField]
    GameObject m_PlayAgainButton = null;

    public void GivePointToPlayer(bool isRightPlayer)
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

        AH_Puck scoringPuck = FindObjectOfType<AH_Puck>();
		Debug.Log(scoringPuck.name);
        scoringPuck.GetComponentInChildren<TrailRenderer>().enabled = false;
        StartCoroutine(PuckResetCoroutine(scoringPuck));
    }

    IEnumerator PuckResetCoroutine(AH_Puck scoringPuck)
    {
        yield return new WaitForSecondsRealtime(0.5f);
        scoringPuck.ResetPuck();
        if (DetermineWinState())
        {
            //End Game Logic Stop players from playing
            Time.timeScale = 0;
            if (m_PlayAgainButton)
            {
                m_PlayAgainButton.SetActive(true);
            }
        }
        StopCoroutine(PuckResetCoroutine(scoringPuck));
    }

    private bool DetermineWinState()
    {
        bool player1Win = m_player1Score >= m_scoreToWin;
        bool player2Win = m_player2Score >= m_scoreToWin;
        if (player1Win)
        {
            m_scores[0].text = "LOSE";
            m_scores[1].text = "WIN";
            return true;
        }
        else if (player2Win)
        {
            m_scores[0].text = "WIN";
            m_scores[1].text = "LOSE";
            return true;
        }
        else return false;        
    }
}
