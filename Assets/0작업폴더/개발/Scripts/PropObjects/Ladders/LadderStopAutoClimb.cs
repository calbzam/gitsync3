using UnityEngine;

public class LadderStopAutoClimb : MonoBehaviour
{
    [SerializeField] private LadderTrigger _ladder;
    private bool _autoClimbPrevState;

    private void stopClimbDir(bool stop)
    {
        if (transform.position.y > _ladder.transform.position.y) _ladder.StopClimbingUpwards = stop;
        else if (transform.position.y < _ladder.transform.position.y) _ladder.StopClimbingDownwards = stop;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag(Tags.PlayerTag))
        {
            _autoClimbPrevState = _ladder.AutoClimbWhenJumpedOn;
            _ladder.AutoClimbWhenJumpedOn = false;
            stopClimbDir(true);
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.CompareTag(Tags.PlayerTag))
        {
            _ladder.AutoClimbWhenJumpedOn = _autoClimbPrevState;
            stopClimbDir(false);
        }
    }
}
