using UnityEngine;

public class SetPlayerToStartPosOnSceneLoad : MonoBehaviour
{
    [SerializeField] private PlayerController _player; // do not replace with PlayerLogic.Player, for loading consistency in testing purposes
    [SerializeField] private Rigidbody2D _playerRb;
    [SerializeField] private Cloth _playerCloth;
    [SerializeField] private Checkpoint _initialSpawnPoint;

    private void Start()
    {
        if (SceneLoadManager.Level1LoadedExternally)
        {
            _playerCloth.enabled = false;
            _playerCloth.gameObject.SetActive(false);
            SetPlayerToInitialSpawnPoint();
        }
    }

    public void ResetPlayerCloth()
    {
        //_playerCloth.enabled = false;
        //_playerCloth.enabled = true;

        _playerCloth.gameObject.SetActive(true);
        _playerCloth.enabled = true;
    }

    private void SetPlayerToInitialSpawnPoint()
    {
        _player.transform.position = _initialSpawnPoint.Position;
        _playerRb.velocity = Vector3.zero;

        _player.SetRespawnPoint(_initialSpawnPoint);

        ResetPlayerCloth();
    }
}
