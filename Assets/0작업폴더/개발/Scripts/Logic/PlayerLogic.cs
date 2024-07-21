using System;
using UnityEngine;
using Obi;

public class PlayerLogic : MonoBehaviour
{
    public static PlayerController Player { get; private set; }
    public static PlayerStats PlayerStats { get; private set; }
    public static Rigidbody2D PlayerRb { get; private set; }
    
    public static ObiCollider2D PlayerObiCol { get; private set; }
    public static ObiCollider2D PlayerRopeRiderCol { get; private set; }

    private static PlayerAnimController _playerAnim;
    private static RigidbodyConstraints2D _origPlayerConstraints;

    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        PlayerStats = Player.Stats;
        PlayerRb = Player.GetComponent<Rigidbody2D>();

        PlayerObiCol = Player.GetComponent<ObiCollider2D>();
        PlayerRopeRiderCol = GameObject.FindGameObjectWithTag("Player ropeRider").GetComponent<ObiCollider2D>();

        _playerAnim = Player.GetComponentInChildren<PlayerAnimController>();
        _origPlayerConstraints = PlayerRb.constraints;
    }

    public static void DisconnectedPlayerAddJump()
    {
        if (InputReader.FrameInput.Move.x < 0)
            PlayerRb.AddForce(new Vector2(-1, 1) * PlayerStats.RopeJumpedPlayerAddForce, ForceMode2D.Impulse);
        else if (InputReader.FrameInput.Move.x > 0)
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
}
