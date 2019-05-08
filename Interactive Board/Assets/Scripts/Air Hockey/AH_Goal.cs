using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles the making of goals
/// </summary>
public class AH_Goal : MonoBehaviour
{
    [Header("Configuration")]
    [Tooltip("Whether or not this is the goal on the right side")]
    [SerializeField]
    bool isRightGoal = true;
    [Tooltip("A reference to the prefab child with a particle system")]
    [SerializeField]
    GameObject goalParticles = null;
    
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<AH_Puck>() != null)
        {
            AH_GameMaster.Instance.GivePointToPlayer(isRightGoal, collision);
            goalParticles.GetComponent<ParticleSystem>().Play();
            audioSource.Play();
        }
    }
}