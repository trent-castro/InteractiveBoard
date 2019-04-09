using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AH_PowerUp : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Puck"))
        {
            CauseEffect(collision.gameObject.GetComponent<AH_Puck>());
        }
    }

    /// <summary>
    /// Disables the power up and returns the position back to the parent transform
    /// </summary>
    public void DisablePowerUp()
    {
        gameObject.SetActive(false);
        transform.position = transform.parent.position;
    }

    public virtual void CauseEffect(AH_Puck puck) { }
}
