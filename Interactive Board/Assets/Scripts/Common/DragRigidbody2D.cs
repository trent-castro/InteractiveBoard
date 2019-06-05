using System;
using System.Collections;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class DragRigidbody2D : MonoBehaviour
{
	/// <summary>
	/// The springjoint used to smoothly move an object around.
	/// </summary>
    private SpringJoint2D m_springJoint = null;
	/// <summary>
	/// The rigidbody the springjoint is dragging.
	/// </summary>
    private Rigidbody2D m_rigidbody = null;
	[Tooltip ("The available area for touch events to be resistered.")]
    [SerializeField]
    private CompositeTouchArea m_touchArea = null;
	[Tooltip("The available area that can be grabbed.")]
	[SerializeField]
    private Collider2D m_grabbableArea = null;

	[Tooltip("The dampening of the spring joint.")]
	public float m_dampingRatio = .05f;
	[Tooltip("The frequency of the SpringJoint")]
	public float m_frequency = 5;

	[Tooltip("The drag on the connected body of the SpringJoint")]
	public float m_drag = 50;
	[Tooltip("The angular drag on the connected body of the SpringJoint")]
	public float m_angularDrag = 5;
	/// <summary>
	/// The original drag of the pulled rigidbody that is stored to be restored after the drag.
	/// </summary>
    private float m_oldDrag;
	/// <summary>
	///  The original angular drag of the pulled rigidbody that is stored to be restored after the drag.
	/// </summary>
	private float m_oldAngularDrag;

	/// <summary>
	/// The touch that owns this SpringJoint.
	/// </summary>
    private TouchInfo m_touchInfo = null;

	[Header("Debug Controls")]
    public TextMeshPro m_debugOutput = null;
    public string m_debugString = "";

	/// <summary>
	/// Defines whether or not this Springjoint can currently be grabbed by a touch.
	/// </summary>
    private bool canGrab = true;

    private void Start()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();

        MultiTouchManager.Instance.ListenForTouchesOnOverlapWithEvents(m_grabbableArea.OverlapPoint, CheckForGrab);
        MultiTouchManager.Instance.ListenForTouchesOnOverlapWithEvents(m_touchArea.OverlapPoint, CheckForLetGo);
    }

	/// <summary>
	/// Checks to see if a touchevent is a valid touch event to grab this object.
	/// </summary>
	/// <param name="touchInfo"></param>
	/// <param name="eventType"></param>
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

	/// <summary>
	/// Checks if the touchevent is exiting and releases this object from being grabbed
	/// </summary>
	/// <param name="touchInfo"></param>
	/// <param name="eventType"></param>
    private void CheckForLetGo(TouchInfo touchInfo, ETouchEventType eventType)
    {
        if (eventType == ETouchEventType.EXIT && m_touchInfo != null && touchInfo.FingerId == m_touchInfo.FingerId)
        {
            OnTouchEnd(touchInfo);
            canGrab = false;

            StartCoroutine(SetCanGrabAfterDelay());
        }
    }

	/// <summary>
	/// Allows this object to be grabed after a small delay.
	/// </summary>
	/// <returns></returns>
    private IEnumerator SetCanGrabAfterDelay()
    {
        yield return null;
        canGrab = true;
    }

	/// <summary>
	/// Grabs this object and creates a springjoint if one does not exist.
	/// </summary>
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

	/// <summary>
	/// Cleans up touch info once the touch event controlign this object ends
	/// </summary>
	/// <param name="touchInfo"></param>
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

	/// <summary>
	/// Moves this object according to the touch event controlling it.
	/// </summary>
	/// <param name="touchInfo"></param>
    private void OnTouchMove(TouchInfo touchInfo)
    {
        Debug.DrawLine(m_springJoint.transform.position, transform.position, Color.red);
        m_springJoint.transform.position = Camera.main.ScreenToWorldPoint(touchInfo.Position);

        CreateDebugString("Dragging");
    }

	/// <summary>
	/// Sets debug lines to inclue all relevant informaton
	/// </summary>
	/// <param name="state"></param>
    private void CreateDebugString(string state)
    {
        if (m_debugOutput != null)
        {
            m_debugString = $"{state}\nVelocity: {m_rigidbody.velocity}\nConnected: {m_springJoint.connectedBody != null}\nHas TouchInfo: {m_touchInfo != null}";
            m_debugOutput.text = m_debugString;
        }
    }
}
