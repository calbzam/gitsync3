using UnityEngine;

public class SetPlayerToStartPosOnSceneLoad : MonoBehaviour
{
    [SerializeField] private PlayerController _player; // do not replace with PlayerLogic.Player, for loading consistency in testing purposes
    [SerializeField] private Rigidbody2D _playerRb;
    [SerializeField] private Checkpoint _initialSpawnPoint;

    private void Start()
    {
        if (SceneLoadManager.Level1LoadedExternally)
        {
            _player.transform.position = _initialSpawnPoint.Position;
            _playerRb.velocity = Vector3.zero;
            _player.SetRespawnPoint(_initialSpawnPoint);
        }
    }
}
