using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LightningCircle : MonoBehaviour
{
    LineRenderer m_lineRenderer = null;
    public float m_radius = 1;
    public float m_variance = .1f;
    public float m_varianceTime = .2f;
    public float m_varianceCounter = .2f;
    public int m_lines = 20;

    // Start is called before the first frame update
    void Start()
    {
        m_lineRenderer = GetComponent<LineRenderer>();
        m_lineRenderer.positionCount = m_lines;
    }

    // Update is called once per frame
    void Update()
    {
        m_varianceCounter += Time.deltaTime;
        if (m_varianceCounter >= m_varianceTime)
        {
            m_varianceCounter = 0;


            Vector2 radius = new Vector2(0, m_radius);
            float section = 360.0f / m_lines;

            Vector2 position = radius;

            for (int i = 0; i < m_lineRenderer.positionCount; i++)
            {
                position = Quaternion.Euler(0, 0, section * i) * radius;

                position *= 1 + Random.Range(-m_variance, m_variance);

                m_lineRenderer.SetPosition(i, position);

            }
        }
    }
}
