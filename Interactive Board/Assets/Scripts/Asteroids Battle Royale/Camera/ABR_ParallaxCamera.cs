﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct ParallaxLayer
{
    public Renderer renderer;
    public float ratio;
}

/// <summary>
/// A parallax scrolling camera for star scrolling.
/// </summary>
[RequireComponent(typeof(Camera))]
public class ABR_ParallaxCamera : MonoBehaviour
{
    [Tooltip("An list of external references to the various star fields for parallax scrolling.")]
    [SerializeField]
    private List<ParallaxLayer> m_layers = null;

    private void Start()
    {
        foreach (ParallaxLayer layer in m_layers)
        {
            layer.renderer.material = Instantiate(layer.renderer.material);
        }
    }

    private void Update()
    {
        Vector2 layerScale;
        foreach (ParallaxLayer layer in m_layers)
        {
            if (layer.renderer)
            {
                layerScale = layer.renderer.material.mainTextureScale;
                layer.renderer.material.mainTextureOffset = Quaternion.Inverse(layer.renderer.transform.rotation) * new Vector2((transform.position.x / layer.ratio), (transform.position.y / layer.ratio));
            }
        }
    }
}
