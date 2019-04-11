using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AH_Paddle : MonoBehaviour
{
    public Vector2 m_Velocity { get; set; }
    [SerializeField]

    void Update()
    {
        Vector3 currentPos = transform.position;
        Vector3 goalPos = transform.position;

        //Make it so that the player has to drag the Paddle after touching it. 
        Vector3 touchPos = Vector3.zero;
        if (Input.touches.Length > 0)
        {
            foreach (Touch touch in Input.touches)
            {
                touchPos = touch.position;

                Vector3 screenLocation = Camera.main.ScreenToWorldPoint(touchPos);

                //determine if touchPos is within the circle of the sprite
                Vector3 distanceVec = transform.position - screenLocation;
                distanceVec.z = 0;
                float distance = distanceVec.magnitude;

                //set Position to touch Position
                if (distance <= 1)
                {
                    screenLocation.z = 0.0f;
                    goalPos = screenLocation;
                    break;
                }
            }
        }

        ////this is for bringing the puck to the player touch. 
        //Vector3 touchPos = Vector3.zero;
        //if (Input.touches.Length > 0)
        //    touchPos = Input.touches[0].position;

        //float spriteRadius =  (gameObject.GetComponent<SpriteRenderer>().sprite.border.w)/2;        


        m_Velocity = (goalPos - currentPos) / Time.deltaTime;
        transform.position = goalPos;
        //Debug.DrawLine(transform.position, (Vector2)transform.position + m_Velocity, Color.red);
    }

}
