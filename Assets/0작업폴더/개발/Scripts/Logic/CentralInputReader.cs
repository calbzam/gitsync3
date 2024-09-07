using System;
using UnityEngine;

public class CentralInputReader : MonoBehaviour
{
    public static InputControls Input { get; private set; }

    private void Awake()
    {
        Input = new InputControls();
    }

    private void OnEnable()
    {
        Input.Enable();
    }

    private void OnDisable()
    {
        Input.Disable();
    }
}
