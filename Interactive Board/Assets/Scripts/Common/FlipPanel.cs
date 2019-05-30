using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipPanel : MonoBehaviour
{
    public void Flip()
    {
        transform.localScale = transform.localScale * (new Vector2(-1, 1));
    }
}
