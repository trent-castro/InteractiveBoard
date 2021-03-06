﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class Locator : MonoBehaviour
{
    [SerializeField]
    private GameObject m_center = null;

    [SerializeField, Layer]
    private int m_layer = 0;

    private Camera m_camera = null;

    private List<Locatable> m_locatables = null;
    private List<Indicator> m_indicators = new List<Indicator>();

    [SerializeField]
    private Transform m_indicatorParent = null;

    private void Start()
    {
        LocatorManager.Instance.Register(this);
        m_camera = GetComponent<Camera>();
    }

    public void SetupIndicators(List<Locatable> locatables)
    {
        m_locatables = locatables;

        foreach (Locatable locatable in locatables)
        {
            AddIndicator(locatable);
        }
    }

    public void AddIndicator(Locatable locatable)
    {
        Indicator indicator = locatable.GetIndicatorInstance(m_indicatorParent ?? transform);
        m_indicators.Add(indicator);
        indicator.gameObject.SetActive(false);
        indicator.gameObject.layer = m_layer;
        foreach (Transform trans in indicator.GetComponentsInChildren<Transform>(true))
        {
            trans.gameObject.layer = m_layer;
        }
    }

    void Update()
    {
        Vector2 cameraCenter = m_camera.WorldToScreenPoint(transform.position);
        Vector2 centerOffset;
        if (m_center == null)
        {
            //Camera center
            centerOffset = Vector2.zero;
        }
        else
        {
            //Target center
            centerOffset = (Vector2)m_camera.WorldToScreenPoint(m_center.transform.position) - cameraCenter;
        }

        for (int i = 0; i < m_locatables.Count; i++)
        {
            Locatable locatable = m_locatables[i];
            Indicator indicator = m_indicators[i];

            Vector2 locatablePosition = m_camera.WorldToScreenPoint(locatable.transform.position);
            Vector2 direction = locatablePosition - (cameraCenter + centerOffset);

            if (!locatable.isActiveAndEnabled || m_camera.pixelRect.Contains(locatablePosition) || direction.magnitude > locatable.IndicationRange)
            {
                indicator.gameObject.SetActive(false);
                continue;
            }
            else if (!indicator.isActiveAndEnabled)
            {
                indicator.gameObject.SetActive(true);
            }

            indicator.transform.rotation = Quaternion.Euler(0, 0, direction.ZAngle());

            Vector2 cameraEdge = FindIntersectionWithBounds(cameraCenter, centerOffset, direction, m_camera.rect);
            float distance = (locatablePosition - cameraEdge).magnitude;

            indicator.transform.position = cameraCenter + centerOffset + direction.normalized * locatable.SpaceFromCenter;

            indicator.transform.localScale = Vector3.one * (1 - (0.5f * distance / locatable.IndicationRange));
        }

    }

    private Vector2 FindIntersectionWithBounds(Vector2 center, Vector2 offset, Vector2 direction, Rect bounds)
    {
        Vector2 closestCorner;
        closestCorner.x = (direction.x > 0 ? bounds.xMax : bounds.xMin);
        closestCorner.y = (direction.y > 0 ? bounds.yMax : bounds.yMin);

        closestCorner -= (center + offset);

        float xMult = Mathf.Abs(closestCorner.x / direction.x);
        float yMult = Mathf.Abs(closestCorner.y / direction.y);

        Vector2 intersection = direction * Mathf.Min(xMult, yMult);

        return intersection + (center + offset);
    }
}
