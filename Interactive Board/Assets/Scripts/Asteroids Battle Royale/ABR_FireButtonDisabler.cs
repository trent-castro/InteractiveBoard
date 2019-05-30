using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ABR_FireButtonDisabler : MonoBehaviour
{
    [SerializeField]
    private ABR_Turret m_turret = null;

    [SerializeField]
    private Button m_fireButton = null;

    [SerializeField]
    private RectTransform m_reloadVisualMask = null;

    Vector2 m_baseSize;

    private void Start()
    {
        m_baseSize = m_reloadVisualMask.rect.size;
    }

    private void Update()
    {

        if (m_turret.IsOkayToFire && !m_fireButton.interactable)
        {
            m_reloadVisualMask.gameObject.SetActive(false);
        }
        else if (!m_turret.IsOkayToFire)
        {
            if (m_fireButton.interactable)
            {
                m_reloadVisualMask.gameObject.SetActive(true);
            }

            Vector2 size = m_baseSize * Mathf.Lerp(0, 1, m_turret.getReloadPercent());
            m_reloadVisualMask.offsetMin = -size / 2;
            m_reloadVisualMask.offsetMax = size / 2;
        }

        m_fireButton.interactable = m_turret.IsOkayToFire;
    }
}
