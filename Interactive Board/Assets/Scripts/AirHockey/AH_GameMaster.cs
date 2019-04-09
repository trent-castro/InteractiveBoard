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

        Instantiate(m_puck);

    }

}
