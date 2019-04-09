using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    [SerializeField]
    bool IsRightGoal;

    private GameMaster gm;

    private void Start()
    {
        gm = FindObjectOfType<GameMaster>();   
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<Puck>() != null)
        {
            gm.GivePointToPlayer(IsRightGoal);
            Destroy(collision.gameObject, 2.0f);
        }
    }
}
