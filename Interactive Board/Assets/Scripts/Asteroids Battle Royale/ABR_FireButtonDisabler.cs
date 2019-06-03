using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ABR_FireButtonDisabler : MonoBehaviour
{
    [Header("External References")]
    [Tooltip("A reference to the associated turret.")]
    [SerializeField]
    private ABR_Turret m_turret = null;
    [Tooltip("A reference to the associated fire button.")]
    [SerializeField]
    private Button m_fireButton = null;
    [Tooltip("A reference to the associated reload visual mask.")]
    [SerializeField]
    private RectTransform m_reloadVisualMask = null;

    /// <summary>
    /// The base size of the reload visual mask for interpolation.
    /// </summary>
    private Vector2 m_baseSize;

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
            FixReloadVisualMask();
        }
        m_fireButton.interactable = m_turret.IsOkayToFire;
    }

    // Modifies parameters for the reload visual mask
    private void FixReloadVisualMask()
    {
        Vector2 size = m_baseSize * Mathf.Lerp(0, 1, m_turret.getReloadPercent());
        m_reloadVisualMask.offsetMin = -size / 2;
        m_reloadVisualMask.offsetMax = size / 2;
    }
}
