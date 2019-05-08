using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]

public class UIPanTexture : MonoBehaviour
{
	Image m_spriteRenderer = null;
	[SerializeField] Vector2 Scrollspeed = Vector2.up;
    // Start is called before the first frame update
    void Start()
    {
		m_spriteRenderer = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
    }
}
