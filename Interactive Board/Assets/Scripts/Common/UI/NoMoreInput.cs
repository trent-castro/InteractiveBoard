using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoMoreInput : MonoBehaviour
{
    [SerializeField]
    private HoldButton button = null;

    [SerializeField]
    private DragPanel panel = null;

    [SerializeField]
    private Joystick joystick = null;

    private void OnEnable()
    {
        button?.KillInput();
        panel?.KillInput();
        joystick?.KillInput();
    }
}
