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
    private GameObject m_reloadVisual = null;

    private void Update()
    {

        if (m_turret.IsOkayToFire && !m_fireButton.interactable)
        {
            m_reloadVisual.gameObject.SetActive(false);
        }
        else if (!m_turret.IsOkayToFire)
        {
            if (m_fireButton.interactable)
            {
                m_reloadVisual.gameObject.SetActive(true);
            }

            m_reloadVisual.transform.localScale = Vector3.one * Mathf.Lerp(0, 1, Interpolation.QuadraticOut(m_turret.getReloadPercent()));
        }

        m_fireButton.interactable = m_turret.IsOkayToFire;
    }
}
