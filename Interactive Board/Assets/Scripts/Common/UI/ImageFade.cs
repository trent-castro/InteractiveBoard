using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageFade : MonoBehaviour
{
    [SerializeField] float TimeToFade = 1.5f;
    [SerializeField] float DelayTimeToStart = 1.5f;
    [SerializeField] bool isFadingIn = true;


    private Image m_image;
    private Color m_startColor;
    private float m_timeElapsed = 0.0f;
    private void Awake()
    {
        m_image = GetComponent<Image>();
        m_startColor = m_image.color;
        StartCoroutine(Fade());
    }

    IEnumerator Fade()
    {
        yield return new WaitForSeconds(DelayTimeToStart);
        Color newColor = m_startColor;
        while (m_timeElapsed < TimeToFade)
        {
            if (isFadingIn)
            {
                newColor.a = Mathf.Lerp(0, 1, m_timeElapsed / TimeToFade);
                m_image.color = newColor;
            }
            else
            {
                newColor.a = Mathf.Lerp(1, 0, m_timeElapsed / TimeToFade);
                m_image.color = newColor;
            }
            m_timeElapsed += Time.deltaTime;
            yield return null;
        }
    }
}
