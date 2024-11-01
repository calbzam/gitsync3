using UnityEngine;

public class WaterSurfaceCollision : MonoBehaviour
{
    [SerializeField] private bool _diveSwimAllowed = true;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag(Tags.PlayerTag))
        {
            PlayerLogic.Player.DiveSwimAllowed = _diveSwimAllowed;
            PlayerLogic.Player.IsOnWater = true;
        }
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.CompareTag(Tags.PlayerTag))
        {
            if (FrameInputReader.FrameInput.InputDir.y < 0)
                PlayerLogic.Player.IsInWater = true;
            else
                PlayerLogic.Player.IsInWater = false;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag(Tags.PlayerTag))
        {
            PlayerLogic.Player.IsOnWater = false;
        }
    }
}
