using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A MonoBehaviour that adds a shadow to a sprite
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
public class Shadow2D : MonoBehaviour
{
	[Tooltip ("Determines if the shadow is already existing or not")]
    public bool m_ownedByParent = true;
	[Tooltip("Determines if the shadow needs to be created as a copy of the parent sprite or not")]
	public bool m_copyFromParent = true;

	[Tooltip("The owner of the shadow")]
	public GameObject m_owner = null;

	[Tooltip("The sprite renderer of the parent")]
	public SpriteRenderer m_toCopy = null;

	[Tooltip("The sprite renderer of the copied shadow")]
	private SpriteRenderer m_spriteRenderer = null;

	[Tooltip("The base scale of the parent object")]
	private Vector2 m_ownerBaseScale;
	[Tooltip("The base scale of the shadow")]
    private Vector2 m_baseScale;
	[Tooltip("The multiplier of the shadow")]
	public Vector2 m_scaleMultiplier = Vector3.one;

	[Tooltip("The base offset of the shadow")]
	public Vector2 m_baseOffset;
	[Tooltip("The multiplier of the shadow")]
	public Vector2 m_offsetMultiplier = Vector3.one;

    void Start()
    {
    }

    private void Awake()
    {
        if (m_ownedByParent)
        {
            m_owner = transform.parent.gameObject ?? m_owner;
        }

        if (m_copyFromParent)
        {
            m_toCopy = transform.parent.gameObject.GetComponent<SpriteRenderer>() ?? m_toCopy;
        }

        m_ownerBaseScale = m_owner.transform.localScale;

        m_baseScale = transform.localScale;
        m_baseOffset = transform.position - m_owner.transform.position;

        m_spriteRenderer = GetComponent<SpriteRenderer>();
        if (m_toCopy != null)
        {
            m_spriteRenderer.sortingOrder = m_toCopy.sortingOrder - 1;
            m_spriteRenderer.sprite = m_toCopy.sprite;
            m_spriteRenderer.color = Color.black;
        }
    }

    void Update()
    {
        //Vector2 scale = (Vector2)m_owner.transform.localScale - m_ownerBaseScale;

        transform.position = (Vector2)m_owner.transform.position + m_baseOffset;

        if (m_toCopy != null)
        {
            m_spriteRenderer.enabled = m_toCopy.enabled && m_toCopy.gameObject.activeInHierarchy && m_owner.gameObject.activeInHierarchy;
        }
        else
        {
            m_spriteRenderer.enabled = m_owner.gameObject.activeInHierarchy;
        }
    }
}
