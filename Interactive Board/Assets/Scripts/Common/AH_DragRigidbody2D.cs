using System;
using System.Collections;
using UnityEngine;

namespace UnityStandardAssets.Utility
{
    public class AH_DragRigidbody2D : MonoBehaviour
    {
        private SpringJoint2D m_SpringJoint;

        public float m_dampingRatio = .05f;
        public float m_frequency = 1;

        public float m_drag = 5;
        public float m_angularDrag = 10;

        private void Update()
        {
            // Make sure the user pressed the mouse down
            if (!Input.GetMouseButtonDown(0))
            {
                return;
            }

            var mainCamera = FindCamera();

            // We need to actually hit an object
            Vector2 point = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Collider2D hit = Physics2D.OverlapPoint(point);
            if (hit == null)
            {
                return;
            }

            // We need to hit a rigidbody that is not kinematic
            if (!hit.attachedRigidbody || hit.attachedRigidbody.isKinematic)
            {
                return;
            }

            if (!m_SpringJoint)
            {
                var go = new GameObject("Rigidbody dragger");
                Rigidbody2D body = go.AddComponent<Rigidbody2D>();
                m_SpringJoint = go.AddComponent<SpringJoint2D>();
                body.isKinematic = true;
            }

            m_SpringJoint.transform.position = point;
            m_SpringJoint.anchor = Vector3.zero;
            
            m_SpringJoint.autoConfigureDistance = false;
            m_SpringJoint.dampingRatio = m_dampingRatio;
            m_SpringJoint.frequency = m_frequency;

            m_SpringJoint.connectedBody = hit.attachedRigidbody;

            StartCoroutine("DragObject");
        }


        private IEnumerator DragObject()
        {
            var oldDrag = m_SpringJoint.connectedBody.drag;
            var oldAngularDrag = m_SpringJoint.connectedBody.angularDrag;
            m_SpringJoint.connectedBody.drag = m_drag;
            m_SpringJoint.connectedBody.angularDrag = m_angularDrag;
            var mainCamera = FindCamera();
            while (Input.GetMouseButton(0))
            {
                if (m_SpringJoint != null)
                {
                    m_SpringJoint.transform.position = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                }
                yield return null;
            }
            if (m_SpringJoint.connectedBody)
            {
                m_SpringJoint.connectedBody.drag = oldDrag;
                m_SpringJoint.connectedBody.angularDrag = oldAngularDrag;
                m_SpringJoint.connectedBody = null;
            }
        }


        private Camera FindCamera()
        {
            if (GetComponent<Camera>())
            {
                return GetComponent<Camera>();
            }

            return Camera.main;
        }
    }
}
