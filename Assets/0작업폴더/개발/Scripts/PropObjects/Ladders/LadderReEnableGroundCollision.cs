using UnityEngine;

public class LadderReEnableGroundCollision : MonoBehaviour
{
    [SerializeField] private LadderTrigger _ladder;
    private bool _bypassGroundPrevState;

    private void Start()
    {
        _bypassGroundPrevState = _ladder.BypassGroundCollision;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag(Tags.PlayerTag))
        {
            if (PlayerLogic.Player.CurrentLadder == _ladder)
            {
                _ladder.BypassGroundCollision = false;
                PlayerLogic.EnablePlayerGroundCollision(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.CompareTag(Tags.PlayerTag))
        {
            if (PlayerLogic.Player.CurrentLadder == _ladder)
            {
                _ladder.BypassGroundCollision = _bypassGroundPrevState;
                if (PlayerLogic.Player.IsOnLadder)
                    PlayerLogic.EnablePlayerGroundCollision(!_bypassGroundPrevState);
            }
        }
    }
}
