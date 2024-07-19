using System;
using UnityEngine;
using UnityEngine.InputSystem;

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

    public static event Action JumpPressed;

    private void Awake()
    {
        _input = new InputControls();
        JumpHolding = JumpTriggered = JumpTriggeredPrev = false;
    }

    private void OnEnable()
    {
        _input.Enable();
        _input.Player.Jump.started += JumpStartedAction;
    }

    private void OnDisable()
    {
        _input.Disable();
        _input.Player.Jump.started -= JumpStartedAction;
    }

    private void Update()
    {
        GatherInput();
    }

    private void JumpStartedAction(InputAction.CallbackContext ctx)
    {
        TriggerJump();
    }

    public static void TriggerJump()
    {
        JumpPressed?.Invoke();
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
