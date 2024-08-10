using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class FrameInputReader : MonoBehaviour
{
    public struct FrameInput
    {
        public static bool JumpStarted = false;
        public static bool JumpHeld = false;
        public static Vector2 Move;
    }

    //public InputControls Input;
    public static event Action JumpPressed;

    private bool JumpHolding, JumpTriggered, JumpTriggeredPrev;

    private void Awake()
    {
        //Input = new InputControls();
        JumpHolding = JumpTriggered = JumpTriggeredPrev = false;
    }

    private void OnEnable()
    {
        //Input.Enable();
        CentralInputReader.Input.Player.Jump.started += JumpStartedAction;
    }

    private void OnDisable()
    {
        CentralInputReader.Input.Player.Jump.started -= JumpStartedAction;
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
        JumpHolding = CentralInputReader.Input.Player.Jump.IsPressed();
        JumpTriggered = !JumpTriggeredPrev && JumpHolding;
        JumpTriggeredPrev = JumpHolding;

        FrameInput.JumpStarted = JumpTriggered;
        FrameInput.JumpHeld = JumpHolding;
        FrameInput.Move = CentralInputReader.Input.Player.Movement.ReadValue<Vector2>();
    }
}
