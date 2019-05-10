using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shadow2D : MonoBehaviour
{
    public GameObject m_owner = null;


    private Vector2 m_ownerBaseScale;
    public Vector2 m_baseScale = Vector3.one;
    public Vector2 m_scaleMultiplier = Vector3.one;

    public Vector2 m_baseOffset = Vector3.one * 0.05f;
    public Vector2 m_offsetMultiplier = Vector3.one * 0.05f;

    // Start is called before the first frame update
    void Start()
    {
        m_ownerBaseScale = m_owner.transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        //Vector2 scale = (Vector2)m_owner.transform.localScale - m_ownerBaseScale;

        transform.position = (Vector2)m_owner.transform.position + m_baseOffset;
    }
}
