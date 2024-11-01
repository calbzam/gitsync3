using UnityEngine;

public class CheckpointTrigger : MonoBehaviour
{
    [SerializeField] private Checkpoint _checkPoint;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag(Tags.PlayerTag))
        {
            col.GetComponent<PlayerController>().SetRespawnPoint(_checkPoint);
        }
    }
}
