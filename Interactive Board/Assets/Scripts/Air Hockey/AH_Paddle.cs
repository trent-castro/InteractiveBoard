using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AH_Paddle : MonoBehaviour
{
    private AudioSource m_audioSource = null;

    private void Start()
    {
        if(m_audioSource == null)
        {
            m_audioSource = GetComponent<AudioSource>();
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        m_audioSource.Play();
    }
}
