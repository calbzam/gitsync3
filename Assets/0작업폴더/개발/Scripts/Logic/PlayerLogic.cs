using System;
using UnityEngine;
using Obi;
using TMPro;

public class PlayerLogic : MonoBehaviour
{
    public static PlayerController Player { get; private set; }
    public static PlayerStats PlayerStats { get; private set; }
    public static Rigidbody2D PlayerRb { get; private set; }
    
    public static ObiCollider2D PlayerObiCol { get; private set; }
    public static ObiCollider2D PlayerRopeRiderCol { get; private set; }

    public static TMP_Text PlayerElectrocutedText { get; private set; }

    private static PlayerAnimController _playerAnim;
    private static RigidbodyConstraints2D _origPlayerConstraints;

    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        PlayerStats = Player.Stats;
        PlayerRb = Player.GetComponent<Rigidbody2D>();

        PlayerObiCol = Player.GetComponent<ObiCollider2D>();
        PlayerRopeRiderCol = GameObject.FindGameObjectWithTag("Player ropeRider").GetComponent<ObiCollider2D>();

        PlayerElectrocutedText = GameObject.FindGameObjectWithTag("Player electrocutedText").GetComponent<TMP_Text>();
        PlayerElectrocutedText.gameObject.SetActive(false);

        _playerAnim = Player.GetComponentInChildren<PlayerAnimController>();
        _origPlayerConstraints = PlayerRb.constraints;
    }

    public static void DisconnectedPlayerAddJump()
    {
        if (FrameInputReader.FrameInput.Move.x < 0)
            PlayerRb.AddForce(new Vector2(-1, 1) * PlayerStats.RopeJumpedPlayerAddForce, ForceMode2D.Impulse);
        else if (FrameInputReader.FrameInput.Move.x > 0)
            PlayerRb.AddForce(new Vector2(1, 1) * PlayerStats.RopeJumpedPlayerAddForce, ForceMode2D.Impulse);
    }

    public static void SetPlayerZPosition(float newZPos)
    {
        Player.transform.position = new Vector3(Player.transform.position.x, Player.transform.position.y, newZPos);
    }

    public static void LockPlayerPosition()
    {
        PlayerRb.constraints = _origPlayerConstraints | RigidbodyConstraints2D.FreezePosition;
        Player.DirInputSetActive(false);
        _playerAnim.DirInputSetActive(false);
    }

    public static void FreePlayerPosition()
    {
        PlayerRb.constraints = _origPlayerConstraints;
        Player.DirInputSetActive(true);
        _playerAnim.DirInputSetActive(true);
    }

    public static void IgnorePlayerGroundCollision(bool bypass)
    {
        Physics2D.IgnoreLayerCollision(Layers.PlayerLayer.LayerValue, Layers.GroundLayer.LayerValue, bypass);
    }
}
