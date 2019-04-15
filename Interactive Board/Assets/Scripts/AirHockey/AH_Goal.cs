using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AH_Goal : MonoBehaviour
{
    [SerializeField]
    bool IsRightGoal = true;
    [SerializeField]
    GameObject m_GoalParticle = null;

    private AH_GameMaster gm;

    private void Start()
    {
        gm = FindObjectOfType<AH_GameMaster>();   
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<AH_Puck>() != null)
        {
            gm.GivePointToPlayer(IsRightGoal);
            m_GoalParticle.GetComponent<ParticleSystem>().Play();
        }
    }
}
