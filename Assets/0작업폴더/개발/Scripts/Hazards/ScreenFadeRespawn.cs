using UnityEngine;

public abstract class ScreenFadeRespawn : MonoBehaviour
{
    [SerializeField] protected float RespawnDelay = 1f;
    private float _respawnDelayTimer;

    private static bool _isInFadingProcess;
    private static int _callerInstanceID;
    private int _selfInstanceID;

    protected abstract void BeforeScreenFadeOut();
    protected abstract void AfterRespawned();

    protected virtual void Awake()
    {
        _isInFadingProcess = false;
        _callerInstanceID = 0;
        _selfInstanceID = GetInstanceID();
    }

    protected virtual void OnEnable()
    {
        StartCoroutine(CanvasLogic.RespawnFadeInOut_AddReader(() => CanvasLogic.RespawnFadeInOut.FadeOutFinished += CallRespawn));
    }

    protected virtual void OnDisable()
    {
        CanvasLogic.RespawnFadeInOut.FadeOutFinished -= CallRespawn;
    }

    private void Update()
    {
        RespawnDelayTimer();
    }

    private void CallRespawn(int fromInstanceID)
    {
        if (fromInstanceID == _selfInstanceID)
        {
            AfterScreenFadeOut();
            AfterRespawned();
        }
    }

    private void RespawnDelayTimer()
    {
        if (_callerInstanceID != _selfInstanceID) return;

        if (_isInFadingProcess)
        {
            if (Time.time - _respawnDelayTimer > RespawnDelay)
            {
                CanvasLogic.EvalAndStartRespawnFade(_selfInstanceID);
                _isInFadingProcess = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag(Tags.PlayerTag))
        {
            GetReadyForFadeRespawn();
        }
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.CompareTag(Tags.PlayerTag))
        {
            GetReadyForFadeRespawn();
        }
    }

    private void GetReadyForFadeRespawn()
    {
        if (_isInFadingProcess) return;

        BeforeScreenFadeOut();
        CallScreenFadeOut();
    }

    private void CallScreenFadeOut()
    {
        _respawnDelayTimer = Time.time;
        _callerInstanceID = _selfInstanceID;

        _isInFadingProcess = true;
    }

    private void AfterScreenFadeOut() // before screen fade in (black screen state)
    {
        PlayerLogic.Player.RespawnPlayer();
    }
}
