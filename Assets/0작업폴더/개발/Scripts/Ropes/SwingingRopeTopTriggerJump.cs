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
            // do not remove .InputDir.y==0 check here, or PlayerOnThisObject?.Invoke(false) will be called,
            // then player will stick to other rope while climbing down from ground above
            if (_rope.PlayerIsAttached && FrameInputReader.FrameInput.InputDir.y == 0)
            {
                _rope.DisconnectPlayerKeepRangeNoJump(); // be careful with this and PlayerOnOtherObject
            }

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
