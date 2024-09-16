using UnityEngine;

public class StageBGMFade : MonoBehaviour
{
    [SerializeField] private AudioSource _stageBgm;
    [SerializeField] private BoxCollider2D _selfCol;
    private float _colTopY, _colBottomY;
    
    [SerializeField] private float _volumeMin = 0.1f;
    private float _volumeMax;
    public float VolumePercent { get; private set; }

    public bool PlayerInZone { get; private set; }
    
    private void Start()
    {
        _colTopY = _selfCol.bounds.max.y;
        _colBottomY = _selfCol.bounds.min.y;

        _volumeMax = _stageBgm.volume;
        EvalVolumePercentInt();

        PlayerInZone = false;
    }

    public void EvalVolumePercentInt()
    {
        if (PlayerLogic.Player.transform.position.y > _colTopY) VolumePercent = _volumeMax;
        else if (PlayerLogic.Player.transform.position.y < _colBottomY) VolumePercent = _volumeMin;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            PlayerInZone = true;
            EvalVolumePercentInt();
        }
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            VolumePercent = Mathf.Max(PlayerLogic.Player.transform.position.y - _colBottomY, 0) / (_colTopY - _colBottomY) * (_volumeMax - _volumeMin) + _volumeMin;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            PlayerInZone = false;
            EvalVolumePercentInt();
        }
    }
}
