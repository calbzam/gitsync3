using UnityEngine;

public class SwingingRopeTopTriggerJump : MonoBehaviour
{
    [SerializeField] private SwingingRope _rope;
    [SerializeField] private bool _resetPlayerPosToTriggerJump = true;

    [Header("If unchecked, disconnect only if Player is grounded")]
    [SerializeField] private bool _disconnectAlways = true;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag(Tags.PlayerTag))
        {
            if (!_disconnectAlways && !PlayerLogic.Player.OnGround) return;
            if (_rope.PlayerIsAttached)
            {
                if (_resetPlayerPosToTriggerJump) PlayerLogic.SetPlayerXYPos(transform.position);
                _rope.DisconnectPlayer();
            }

            SwingingRope.EnablePlayerRopeCollision(false);
            PlayerLogic.Player.RopeClimbAllowed = false;
        }
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.CompareTag(Tags.PlayerTag))
        {
            if (_rope.PlayerIsAttached) _rope.DisconnectPlayerKeepRangeNoJump();

            if (FrameInputReader.FrameInput.InputDir.y == 0)
            {
                SwingingRope.EnablePlayerRopeCollision(false);
            }
            else
            {
                PlayerLogic.Player.RopeClimbAllowed = true;
                SwingingRope.EnablePlayerRopeCollision(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag(Tags.PlayerTag))
        {
            PlayerLogic.Player.RopeClimbAllowed = true;
            SwingingRope.EnablePlayerRopeCollision(true);
        }
    }
}
