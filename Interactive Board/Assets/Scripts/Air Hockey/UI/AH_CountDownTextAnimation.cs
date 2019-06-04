using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AH_CountDownTextAnimation : MonoBehaviour
{
    [SerializeField] int m_secondsBeforeGameStart = 3;
    [SerializeField] GameObject m_countDownPanel = null;

    TMPro.TextMeshProUGUI text;
    bool CoroutineLooping = true;
    Image panel = null;

    private void Start()
    {
        text = GetComponent<TMPro.TextMeshProUGUI>();
        panel = m_countDownPanel.GetComponent<Image>();
        StartCoroutine(BouceCoroutine());
        StartCoroutine(FadeCoroutine());
    }

    IEnumerator FadeCoroutine()
    {
        while (CoroutineLooping)
        {
            yield return new WaitForSeconds(0.01f);
            gameObject.transform.localScale *= 0.99f;
        }
    }

    IEnumerator BouceCoroutine()
    {
        for (int i = m_secondsBeforeGameStart; i > 0; i--)
        {
            yield return new WaitForSeconds(1);
            text.text = "" + (i - 1);
            if (i == 1)
            {
                text.text = "BEGIN!";
            }
            Vector3 scaleVec = Vector3.one * 1.1f;
            gameObject.transform.localScale = scaleVec;
        }
        yield return new WaitForSeconds(1.5f);
        CoroutineLooping = false;
        m_countDownPanel.SetActive(false);

    }
}
