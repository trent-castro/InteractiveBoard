using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AH_GameMaster : MonoBehaviour
{
    [SerializeField] TextMeshPro[] m_scores;
    [SerializeField] GameObject m_puck;

    public void GivePointToPlayer(bool isRightPlayer)
    {
        if (isRightPlayer)
        {
            int result = 0;
            int.TryParse(m_scores[0].text, out result);
            result++;
            m_scores[0].text = "" + result;
        }
        else
        {
            int result = 0;
            int.TryParse(m_scores[1].text, out result);
            result++;
            m_scores[1].text = "" + result;
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
        StopCoroutine(PuckResetCoroutine());
    }

}
