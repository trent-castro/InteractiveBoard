using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// Component that when attached to an Image will fade in or out based on isFadingIn
/// </summary>
[RequireComponent(typeof(Image))]
public class ImageFade : MonoBehaviour
{
    [Header("Configuration")]
    [Tooltip("The Time it takes for the object to fade")]
    [SerializeField] float m_timeToFade = 1.5f;
    [Tooltip("The Time it takes before the object begins its fading")]
    [SerializeField] float m_delayTimeToStart = 1.5f;
    [Tooltip("Boolean to determing if the image is fading in or out")]
    [SerializeField] bool m_isFadingIn = true;
    [Tooltip("Boolean to determing if the image is a button or out")]
    [SerializeField] bool m_doesHaveChildText = false;

    //Sibling Components
    private Image m_image;
    private TextMeshProUGUI m_text;

    //Member variables
    private Color m_startColor;
    private Color m_buttonTextStartColor;
    private float m_timeElapsed = 0.0f;
    private float m_originalImageAlpha = 0.0f;

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
        Color modifiedColor = m_startColor;
        Color modifiedTextColor = default;
        if (m_doesHaveChildText)
        {
            m_buttonTextStartColor = m_text.color;
            modifiedTextColor = m_buttonTextStartColor;
        }
        //sets the starting colors alpha values
        if (m_isFadingIn)
        {
            modifiedColor.a = 0.0f;
            modifiedTextColor.a = 0.0f;
        }
        else
        {
            modifiedColor.a = m_image.color.a;
            modifiedTextColor.a = m_text.color.a;
        }
        m_image.color = modifiedColor;
        if (m_doesHaveChildText)
        {
            m_text.color = modifiedTextColor;
        }
    }

    /// <summary>
    /// Gets the Required Sibling components
    /// </summary>
    private void GetSiblingComponents()
    {
        m_image = GetComponent<Image>();
        if (m_doesHaveChildText)
        {
            m_text = GetComponentInChildren<TextMeshProUGUI>();
        }
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
        Color newTextColor = default;
        if (m_doesHaveChildText)
        {
            newTextColor = m_text.color;
        }
        while (m_timeElapsed < m_timeToFade)
        {
            float time = m_timeElapsed / m_timeToFade;
            if (m_isFadingIn)
            {
                //set new colors alpha to the lerp value of time
                newColor.a = Mathf.Lerp(0, m_startColor.a, time);
                if (m_doesHaveChildText)
                {
                    newTextColor.a = Mathf.Lerp(0, m_buttonTextStartColor.a, time);
                }
            }
            else
            {
                //set new colors alpha to the lerp value of time
                newColor.a = Mathf.Lerp(m_startColor.a, 0, time);
                if (m_doesHaveChildText)
                {
                    newTextColor.a = Mathf.Lerp(m_buttonTextStartColor.a, 0, time);
                }
            }
            //set image color to the new color
            if (m_doesHaveChildText)
            {
                m_text.color = newTextColor;
            }
            m_image.color = newColor;
            //add deltaTime to time elapsed
            m_timeElapsed += Time.deltaTime;
            yield return null;
        }
        m_image.color = m_startColor;
        if (m_doesHaveChildText)
        {
            m_text.color = m_buttonTextStartColor;

        }
    }
}
