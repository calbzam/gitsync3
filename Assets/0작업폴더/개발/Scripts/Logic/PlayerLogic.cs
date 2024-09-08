using System;
using UnityEngine;
using Obi;
using TMPro;

public class PlayerLogic : MonoBehaviour
{
    [SerializeField] private NearestPlayerRespawn_UseDuringDevelopmentOnly _nearestPlayerRespawn_UseDuringDevelopmentOnly;
    public static NearestPlayerRespawn_UseDuringDevelopmentOnly NearestPlayerRespawn { get; private set; }

    public static event Action PlayerRespawned;

    public static bool PlayerIsLocked { get; private set; }

    public static PlayerController Player { get; private set; }
    public static PlayerStats PlayerStats { get; private set; }
    public static Rigidbody2D PlayerRb { get; private set; }

    public static PlayerAnimController AnimController { get; private set; }
    public static PlayerAnimTools AnimTools { get; private set; }

    public static ObiCollider2D PlayerObiCol { get; private set; }
    public static ObiCollider2D PlayerRopeRiderCol { get; private set; }

    public static TMP_Text PlayerElectrocutedText { get; private set; }

    private static RigidbodyConstraints2D _origPlayerConstraints;
    
    private static FreePlayerDragUI _freePlayerDragUI;
    private static bool _freePlayerDragEnabled;

    private void Start()
    {
        NearestPlayerRespawn = _nearestPlayerRespawn_UseDuringDevelopmentOnly;

        PlayerIsLocked = false;

        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        PlayerStats = Player.Stats;
        PlayerRb = Player.GetComponent<Rigidbody2D>();

        AnimController = Player.GetComponentInChildren<PlayerAnimController>();
        AnimTools = AnimController.AnimTools;

        PlayerObiCol = Player.GetComponent<ObiCollider2D>();
        PlayerRopeRiderCol = GameObject.FindGameObjectWithTag("Player ropeRider").GetComponent<ObiCollider2D>();

        PlayerElectrocutedText = GameObject.FindGameObjectWithTag("Player electrocutedText").GetComponent<TMP_Text>();
        PlayerElectrocutedText.gameObject.SetActive(false);

        _origPlayerConstraints = PlayerRb.constraints;

        _freePlayerDragUI = GameObject.FindGameObjectWithTag("FreePlayerMoveUI").GetComponent<FreePlayerDragUI>();
        _freePlayerDragEnabled = false;
        _freePlayerDragUI.gameObject.SetActive(false);
    }

    public static void DisconnectedPlayerAddJump()
    {
        if (FrameInputReader.FrameInput.Move.x < 0)
            PlayerRb.AddForce(new Vector2(-1, 1) * PlayerStats.RopeJumpedPlayerAddForce, ForceMode2D.Impulse);
        else if (FrameInputReader.FrameInput.Move.x > 0)
            PlayerRb.AddForce(new Vector2(1, 1) * PlayerStats.RopeJumpedPlayerAddForce, ForceMode2D.Impulse);
    }

    public static void SetPlayerXYPos(float x, float y)
    {
        Player.transform.position = new Vector3(x, y, Player.transform.position.z);
    }

    public static void SetPlayerXYPos(Vector2 newPos)
    {
        SetPlayerXYPos(newPos.x, newPos.y);
    }

    public static void SetPlayerZPosition(float newZPos)
    {
        Player.transform.position = new Vector3(Player.transform.position.x, Player.transform.position.y, newZPos);
    }

    public static void InvokePlayerRespawedEvent()
    {
        PlayerRespawned?.Invoke();
    }

    public static void LockPlayer()
    {
        PlayerIsLocked = true;
        PlayerRb.constraints = _origPlayerConstraints | RigidbodyConstraints2D.FreezePosition;
        Player.GroundCheckAllowed = false;
        Player.LadderClimbAllowed = false;
        Player.DirInputActive = false;
    }

    public static void FreePlayer()
    {
        PlayerIsLocked = false;
        PlayerRb.constraints = _origPlayerConstraints;
        Player.GroundCheckAllowed = true;
        Player.LadderClimbAllowed = true;
        Player.DirInputActive = true;
    }

    public static void ToggleFreePlayerDrag()
    {
        _freePlayerDragEnabled = !_freePlayerDragEnabled;
        _freePlayerDragUI.gameObject.SetActive(_freePlayerDragEnabled);
        if (_freePlayerDragEnabled) _freePlayerDragUI.MoveUIToPlayerPosition();
    }

    public static void SetFreePlayerDrag(bool enabled)
    {
        _freePlayerDragEnabled = enabled;
        _freePlayerDragUI.gameObject.SetActive(_freePlayerDragEnabled);
        if (_freePlayerDragEnabled) _freePlayerDragUI.MoveUIToPlayerPosition();
    }

    public static void IgnorePlayerGroundCollision(bool bypass)
    {
        Physics2D.IgnoreLayerCollision(Layers.PlayerLayer.LayerValue, Layers.GroundLayer.LayerValue, bypass);
    }

    //public static void IgnorePlayerLadderCollision(bool bypass)
    //{
    //    Physics2D.IgnoreLayerCollision(Layers.PlayerLayer.LayerValue, Layers.(no ladder layer).LayerValue, bypass);
    //}
}
