using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// Countdown Animation for the beginning of the game
/// </summary>
public class AH_CountDownTextAnimation : MonoBehaviour
{
    [Header("Configuration")]
    [Tooltip("Number that you are counting down from")]
    [SerializeField] int m_secondsBeforeGameStart = 3;
    [Tooltip("Scaler that the text bounces to")]
    [SerializeField] float m_bounceScale = 1.1f;
    [Header("External References")]
    [Tooltip("Reference to the parent object")]
    [SerializeField] GameObject m_countDownPanel = null;

    //Sibling components
    TMPro.TextMeshProUGUI text;

    //private member variables
    bool m_isShrinking = true;

    private void Awake()
    {
        //get the textmesh pro sibling component
        text = GetComponent<TMPro.TextMeshProUGUI>();
        //start the coroutines
        StartCoroutine(BouceCoroutine());
        StartCoroutine(FadeCoroutine());
    }

    //Coroutine that slowly shrinks the text
    IEnumerator FadeCoroutine()
    {
        while (m_isShrinking)
        {
            yield return new WaitForSeconds(0.01f);
            gameObject.transform.localScale *= 0.99f;
        }
    }

    //Coroutine that sets the scale of the text to the bounce scale
    IEnumerator BouceCoroutine()
    {
        //loops through the numbers of the countdown
        for (int i = m_secondsBeforeGameStart; i > 0; i--)
        {
            //wait for one second
            yield return new WaitForSeconds(1);
            //set countdown text to the 
            text.text = "" + (i - 1);
            if (i == 1)
            {
                //after one set text to Begin
                text.text = "BEGIN!";
            }
            //create new scale vector that is the size of the bounce scale
            Vector3 scaleVec = Vector3.one * m_bounceScale;
            //set the game object scale to the new scale vector
            gameObject.transform.localScale = scaleVec;
        }
        //wait for 1.5 seconds
        yield return new WaitForSeconds(1.5f);
        //stop the shrinking
        m_isShrinking = false;
        //set the parent object to inactive
        m_countDownPanel.SetActive(false);

    }
}
