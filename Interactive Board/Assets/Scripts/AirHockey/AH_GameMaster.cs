using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AH_GameMaster : MonoBehaviour
{
    [SerializeField] TextMeshPro[] m_scores = null;
    [SerializeField] GameObject m_puck = null;

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

        m_puck.GetComponentInChildren<TrailRenderer>().enabled = false;
        StartCoroutine(PuckResetCoroutine());
    }

    private void ResetPuck()
    {
        m_puck.transform.position = Vector3.zero;
        m_puck.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        m_puck.GetComponentInChildren<TrailRenderer>().enabled = true;
    }

    IEnumerator PuckResetCoroutine()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        ResetPuck();
        if (DetermineWinState())
        {
            //End Game Logic Stop players from playing
            Time.timeScale = 0;
            if (m_PlayAgainButton)
            {
                m_PlayAgainButton.SetActive(true);
            }
        }
        StopCoroutine(PuckResetCoroutine());
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
