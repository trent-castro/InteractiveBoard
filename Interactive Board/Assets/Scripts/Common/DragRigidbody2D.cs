using System;
using System.Collections;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class DragRigidbody2D : MonoBehaviour
{
    private SpringJoint2D m_springJoint = null;
    [SerializeField]
    private CompositeTouchArea m_touchArea = null;
    [SerializeField]
    private Collider2D m_grabbableArea = null;
    private Rigidbody2D m_rigidbody = null;

    public float m_dampingRatio = .05f;
    public float m_frequency = 5;

    public float m_drag = 50;
    public float m_angularDrag = 5;
    private float m_oldDrag;
    private float m_oldAngularDrag;

    private TouchInfo m_touchInfo = null;

    public TextMeshPro m_debugOutput = null;
    public string m_debugString = "";

    private bool canGrab = true;

    private void Start()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();

        MultiTouchManager.Instance.ListenForTouchesOnOverlapWithEvents(m_grabbableArea.OverlapPoint, CheckForGrab);
        MultiTouchManager.Instance.ListenForTouchesOnOverlapWithEvents(m_touchArea.OverlapPoint, CheckForLetGo);
    }

    private void CheckForGrab(TouchInfo touchInfo, ETouchEventType eventType)
    {
        if (canGrab && eventType != ETouchEventType.EXIT && m_touchInfo == null && touchInfo.m_owners == 0)
        {
            touchInfo.m_owners++;
            m_touchInfo = touchInfo;
            m_touchInfo.ListenForMove(OnTouchMove);
            m_touchInfo.ListenForEnd(OnTouchEnd);
            Grab();
        }
    }

    private void CheckForLetGo(TouchInfo touchInfo, ETouchEventType eventType)
    {
        if (eventType == ETouchEventType.EXIT && m_touchInfo != null && touchInfo.FingerId == m_touchInfo.FingerId)
        {
            OnTouchEnd(touchInfo);
            canGrab = false;

            StartCoroutine(SetCanGrabAfterDelay());
        }
    }

    private IEnumerator SetCanGrabAfterDelay()
    {
        yield return null;
        canGrab = true;
    }

    private void Grab()
    {
        if (!m_springJoint)
        {
            var go = new GameObject("Rigidbody dragger");
            Rigidbody2D body = go.AddComponent<Rigidbody2D>();
            m_springJoint = go.AddComponent<SpringJoint2D>();
            body.isKinematic = true;

            m_springJoint.transform.position = Camera.main.ScreenToWorldPoint(m_touchInfo.Position);
            m_springJoint.anchor = Vector3.zero;

            m_springJoint.autoConfigureDistance = false;
            m_springJoint.dampingRatio = m_dampingRatio;
            m_springJoint.frequency = m_frequency;
        }
        m_springJoint.connectedBody = m_rigidbody;

        m_oldDrag = m_springJoint.connectedBody.drag;
        m_oldAngularDrag = m_springJoint.connectedBody.angularDrag;

        m_springJoint.connectedBody.drag = m_drag;
        m_springJoint.connectedBody.angularDrag = m_angularDrag;

        OnTouchMove(m_touchInfo);
    }

    private void OnTouchEnd(TouchInfo touchInfo)
    {
        if (m_springJoint.connectedBody)
        {
            m_springJoint.connectedBody.drag = m_oldDrag;
            m_springJoint.connectedBody.angularDrag = m_oldAngularDrag;
            m_springJoint.connectedBody = null;
        }
        m_touchInfo.StopListenForMove(OnTouchMove);
        m_touchInfo.StopListenForEnd(OnTouchEnd);
        m_touchInfo.m_owners--;

        m_touchInfo = null;
    }

    private void OnTouchMove(TouchInfo touchInfo)
    {
        Debug.DrawLine(m_springJoint.transform.position, transform.position, Color.red);
        m_springJoint.transform.position = Camera.main.ScreenToWorldPoint(touchInfo.Position);

        CreateDebugString("Dragging");
    }

    private void CreateDebugString(string state)
    {
        if (m_debugOutput != null)
        {
            m_debugString = $"{state}\nVelocity: {m_rigidbody.velocity}\nConnected: {m_springJoint.connectedBody != null}\nHas TouchInfo: {m_touchInfo != null}";
            m_debugOutput.text = m_debugString;
        }
    }
}
