using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class ABR_StateAI : ABR_Ship
{
    [Header("Debug Mode")]
    [SerializeField]
    [Tooltip("Enables Debug Mode")]
    private bool m_debugMode = false;

    [Header("Base Configuration")]
    [Tooltip("The rotation speed of the state AI.")]
    [SerializeField]
    [Range(0.0f, 2.0f)]
    private float rotationSpeed = 1.0f;
    [Tooltip("The max movement speed of the state AI.")]
    [SerializeField]
    [Range(0.0f, 10.0f)]
    private float maxMovementSpeed = 1.0f;
   
    // Protected Sibling Components
    protected Rigidbody2D m_rigidbody2D = null;

    // Protected Internal Information
    protected GameObject m_currentTarget = null;
    protected GameObject[] players;

    /// <summary>
    /// [DEBUG MODE] Records a message if debug mode is enabled.
    /// </summary>
    /// <param name="debugLog">The message to record</param>
    protected void DebugLog(string debugLog)
    {
        if (m_debugMode)
        {
            Debug.Log(debugLog);
        }
    }

    protected abstract void EnableAI();

    protected abstract void DisableAI();

    protected void Awake()
    {
        GetSiblingComponents();
        players = GameObject.FindGameObjectsWithTag("Player");
    }

    private void GetSiblingComponents()
    {
        m_rigidbody2D = GetComponent<Rigidbody2D>();
    }
}