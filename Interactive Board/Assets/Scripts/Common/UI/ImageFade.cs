using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// Component that when attached to an Image will fade in or out based on isFadingIn
/// </summary>
[RequireComponent(typeof(Image))]
public class ImageFade : MonoBehaviour
{
    [Header("Config")]
    [Tooltip("The Time it takes for the object to fade")]
    [SerializeField] float m_timeToFade = 1.5f;
    [Tooltip("The Time it takes before the object begins its fading")]
    [SerializeField] float m_delayTimeToStart = 1.5f;
    [Tooltip("Boolean to determing if the image is fading in or out")]
    [SerializeField] bool m_isFadingIn = true;

    //Sibling Components
    private Image m_image;

    //Member variables
    private Color m_startColor;
    private float m_timeElapsed = 0.0f;

    private void Awake()
    {
        GetSiblingComponents();
        //set the starting color to the color of the object
        SetStartingColor();
        //Start the Fade Coroutine
        StartCoroutine(Fade());
    }

    //Function that sets the image color value determined on if you want to fade in or out
    private void SetStartingColor()
    {
        m_startColor = m_image.color;
        //sets the starting colors alpha values
        if (m_isFadingIn)
        {
            m_startColor.a = 0.0f;
        }
        else
        {
            m_startColor.a = 1.0f;
        }
        m_image.color = m_startColor;
    }

    /// <summary>
    /// Gets the Required Sibling components
    /// </summary>
    private void GetSiblingComponents()
    {
        m_image = GetComponent<Image>();
    }

    /// <summary>
    /// Coroutine that fades the Image by lerping the alpha from either 1-0 or 0-1
    /// </summary>
    IEnumerator Fade()
    {
        //Wait for the delay 
        yield return new WaitForSeconds(m_delayTimeToStart);
        //create new color
        Color newColor = m_startColor;
        while (m_timeElapsed < m_timeToFade)
        {
            float time = m_timeElapsed / m_timeToFade;
            if (m_isFadingIn)
            {
                //set new colors alpha to the lerp value of time
                newColor.a = Mathf.Lerp(0, 1, time);
            }
            else
            {
                //set new colors alpha to the lerp value of time
                newColor.a = Mathf.Lerp(1, 0, time);
            }
            //set image color to the new color
            m_image.color = newColor;
            //add deltaTime to time elapsed
            m_timeElapsed += Time.deltaTime;
            yield return null;
        }
    }
}
