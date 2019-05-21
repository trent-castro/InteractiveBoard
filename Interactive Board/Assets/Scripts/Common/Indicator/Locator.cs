using System.Collections;
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

    private float m_indicationRange = 2500f;

    private Camera m_camera = null;

    private List<Locatable> m_locatables = null;
    private List<GameObject> m_indicators = new List<GameObject>();

    private void Awake()
    {
        LocatorManager.Instance.Register(this);
    }

    void Start()
    {
        m_camera = GetComponent<Camera>();
    }

    public void SetupIndicators(List<Locatable> locatables, List<GameObject> indicatorPrefabs)
    {
        m_locatables = locatables;

        foreach (GameObject indicatorPrefab in indicatorPrefabs)
        {
            AddIndicator(indicatorPrefab);
        }
    }

    public void AddIndicator(GameObject indicatorPrefab)
    {
        GameObject icon = Instantiate(indicatorPrefab, transform);
        m_indicators.Add(icon);
        icon.gameObject.SetActive(false);
        icon.gameObject.layer = m_layer;
        foreach (Transform trans in icon.GetComponentsInChildren<Transform>(true))
        {
            trans.gameObject.layer = m_layer;
        }
    }

    void Update()
    {
        Vector3 cameraZ = new Vector3(0, 0, transform.position.z);

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
            GameObject indicator = m_indicators[i];

            Vector2 locatablePosition = m_camera.WorldToScreenPoint(locatable.transform.position);
            Vector2 direction = locatablePosition - (cameraCenter + centerOffset);

            if (!locatable.isActiveAndEnabled || m_camera.pixelRect.Contains(locatablePosition) || direction.magnitude > m_indicationRange)
            {
                indicator.SetActive(false);
                continue;
            }
            else if (!indicator.activeInHierarchy)
            {
                indicator.SetActive(true);
            }

            Vector2 intersection = FindIntersectionWithBounds(cameraCenter, centerOffset, direction, m_camera.pixelRect) - direction.normalized * locatable.SpaceFromEdge;

            indicator.transform.rotation = Quaternion.Euler(0, 0, direction.ZAngle());

            indicator.transform.position = m_camera.ScreenToWorldPoint(intersection) - cameraZ;
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
