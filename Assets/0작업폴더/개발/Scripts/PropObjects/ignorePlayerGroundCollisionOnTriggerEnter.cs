using UnityEngine;

public class ignorePlayerGroundCollisionOnTriggerEnter : MonoBehaviour
{
    [SerializeField] private Collider2D[] _groundCollidersToIgnore;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag(Tags.PlayerTag))
        {
            foreach (var groundCol in _groundCollidersToIgnore)
            {
                Physics2D.IgnoreCollision(groundCol, col, true);
                Physics2D.IgnoreCollision(groundCol, PlayerLogic.PlayerRopeRiderCol.SourceCollider, true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag(Tags.PlayerTag))
        {
            foreach (var groundCol in _groundCollidersToIgnore)
            {
                Physics2D.IgnoreCollision(groundCol, col, false);
                Physics2D.IgnoreCollision(groundCol, PlayerLogic.PlayerRopeRiderCol.SourceCollider, false);
            }
        }
    }
}
