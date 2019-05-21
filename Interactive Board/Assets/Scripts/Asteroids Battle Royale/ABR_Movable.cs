using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ABR_Movable : MonoBehaviour
{
    [SerializeField] float m_forceModifier = 1.0f;
    Rigidbody2D m_rigidbody = default;
    void Start()
    {
        GetSiblingComponents();
    }

    private void GetSiblingComponents()
    {
        if (m_rigidbody == default)
        {
            m_rigidbody = gameObject.GetComponent<Rigidbody2D>();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(ABR_Tags.ExplosionTag))
        {
            Vector3 pushedDirection = gameObject.transform.position - collision.transform.position;
            m_rigidbody.AddForce(pushedDirection * m_forceModifier, ForceMode2D.Impulse);
        }
    }
}
