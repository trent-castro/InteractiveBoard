using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Locatable : MonoBehaviour
{
    [SerializeField]
    private GameObject m_indicatorPrefab = null;

    [SerializeField]
    private float m_spaceFromEdge = 20;

    public GameObject IndicatorPrefab { get { return m_indicatorPrefab; } }

    public float SpaceFromEdge { get { return m_spaceFromEdge; } }

    private void Awake()
    {
        LocatorManager.Instance.Register(this);
    }
}
