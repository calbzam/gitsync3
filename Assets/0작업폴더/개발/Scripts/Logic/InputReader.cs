using System;
using UnityEngine;

public class InputReader : MonoBehaviour
{
    public struct FrameInput
    {
        public static bool JumpStarted = false;
        public static bool JumpHeld = false;
        public static Vector2 Move;
    }

    private InputControls _input;
    private bool JumpHolding, JumpTriggered, JumpTriggeredPrev;

    private void Awake()
    {
        _input = new InputControls();
        JumpHolding = JumpTriggered = JumpTriggeredPrev = false;
    }

    private void OnEnable()
    {
        _input.Enable();
    }

    private void OnDisable()
    {
        _input.Disable();
    }

    private void Update()
    {
        GatherInput();
    }

    private void GatherInput()
    {
        JumpHolding = _input.Player.Jump.IsPressed();
        JumpTriggered = !JumpTriggeredPrev && JumpHolding;
        JumpTriggeredPrev = JumpHolding;

        FrameInput.JumpStarted = JumpTriggered;
        FrameInput.JumpHeld = JumpHolding;
        FrameInput.Move = _input.Player.Movement.ReadValue<Vector2>();
    }
}
