using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace UnityStandardAssets.Utility
{

    [RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
    public class DragRigidbody2D : MonoBehaviour
    {
        private SpringJoint2D m_springJoint = null;
        private Collider2D m_collider = null;
        private Rigidbody2D m_rigidbody = null;

        public float m_dampingRatio = .05f;
        public float m_frequency = 5;

        public float m_drag = 50;
        public float m_angularDrag = 5;
        private float m_oldDrag;
        private float m_oldAngularDrag;

        private MultiTouchManager m_touchManager = null;
        private TouchInfo m_touchInfo = null;

        public TextMeshPro m_debugOutput = null;
        public string m_debugString = "";

        private void Start()
        {
            m_collider = GetComponent<Collider2D>();
            m_rigidbody = GetComponent<Rigidbody2D>();

            m_touchManager = MultiTouchManager.Instance;
            m_touchManager.ListenForNewTouchesOnCollider(m_collider, CheckNewTouch);
            m_touchManager.ListenForCurrentTouchesOnCollider(m_collider, CheckNewTouch);
        }

        private void CheckNewTouch(TouchInfo touchInfo)
        {
            if (m_touchInfo == null && touchInfo.owners == 0)
            {
                touchInfo.owners++;
                m_touchInfo = touchInfo;
                Grab();

                m_touchInfo.ListenForMove(OnTouchMove);
                m_touchInfo.ListenForEnd(OnTouchEnd);
                //m_touchInfo.ListenForStationary(OnTouchStationary);
            }
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

            createDebugString("Grabbed");
        }

        private void OnTouchEnd(TouchInfo touchInfo)
        {
            if (m_springJoint.connectedBody)
            {
                m_springJoint.connectedBody.drag = m_oldDrag;
                m_springJoint.connectedBody.angularDrag = m_oldAngularDrag;
                m_springJoint.connectedBody = null;
            }

            m_touchInfo = null;

            createDebugString("Let Go");
        }

        private void OnTouchMove(TouchInfo touchInfo)
        {
            Debug.DrawLine(m_springJoint.transform.position, transform.position, Color.red);
            m_springJoint.transform.position = Camera.main.ScreenToWorldPoint(touchInfo.Position);

            createDebugString("Dragging");
        }

        private void createDebugString(string state)
        {
            if (m_debugOutput != null)
            {
                m_debugString = $"{state}\nVelocity: {m_rigidbody.velocity}\nConnected: {m_springJoint.connectedBody != null}\nHas TouchInfo: {m_touchInfo != null}";
                m_debugOutput.text = m_debugString;
            }
        }
    }
}
