using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovePlayerTo : MonoBehaviour
{
    [SerializeField] private Transform _toPoint;
    [SerializeField] private float _moveSpeed = 5;

    private bool _playerIsInRange;
    private bool _movePlayer;

    private void Start()
    {
        _playerIsInRange = false;
        _movePlayer = false;
    }

    private void OnEnable()
    {
        CentralInputReader.Input.Player.EnterHatchDoor.started += EnterButtonPressed;
    }

    private void OnDisable()
    {
        CentralInputReader.Input.Player.EnterHatchDoor.started -= EnterButtonPressed;
    }

    private void Update()
    {
        if (_movePlayer)
        {
            PlayerLogic.SetPlayerXYPos(Vector2.MoveTowards(PlayerLogic.Player.transform.position, _toPoint.position, Time.deltaTime * _moveSpeed));

            if (MyMath.Vector2DiffLessThan(PlayerLogic.Player.transform.position, _toPoint.position, 0.1f)) doneMovingPlayer();
        }
    }

    private void EnterButtonPressed(InputAction.CallbackContext ctx)
    {
        if (_playerIsInRange)
        {
            PlayerLogic.LockPlayer();
            PlayerLogic.IgnorePlayerGroundCollision(true);
            _movePlayer = true;
        }
    }

    private void doneMovingPlayer()
    {
        _movePlayer = false;
        PlayerLogic.IgnorePlayerGroundCollision(false);
        PlayerLogic.FreePlayer();
        if (PlayerLogic.Player.IsOnLadder) PlayerLogic.Player.SetPlayerOnLadder(true);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            _playerIsInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            _playerIsInRange = false;
        }
    }
}
