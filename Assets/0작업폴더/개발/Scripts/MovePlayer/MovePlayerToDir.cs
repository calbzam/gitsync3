using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayerToDir : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5;
    [SerializeField] private FromDirection _fromDirection = FromDirection.FromBelow;
    private Vector3 _toDirection;
    [SerializeField] private float _checkMargin = 0.02f;

    private enum FromDirection
    {
        FromAbove,
        FromBelow,
        FromLeft,
        FromRight,
    }

    private bool _isMovingPlayer;
    private Bounds _colBounds;
    private Vector2 _colSize;

    private void Start()
    {
        _isMovingPlayer = false;
        SetColSize();

        CalcToDirection();
    }

    private void Update()
    {
        if (_isMovingPlayer)
        {
            movePlayer();
        }
    }

    private void movePlayer()
    {
        PlayerLogic.Player.transform.position += Time.deltaTime * _moveSpeed * _toDirection;
    }

    private void SetColSize()
    {
        _colBounds = gameObject.GetComponent<BoxCollider2D>().bounds;
        _colSize = new Vector2(_colBounds.extents.x, _colBounds.extents.y);
    }
    
    private void CalcToDirection()
    {
        switch (_fromDirection)
        {
            default: _toDirection = Vector3.zero; break;

            case FromDirection.FromAbove:
                _toDirection = Vector3.down; break;

            case FromDirection.FromBelow:
                _toDirection = Vector3.up; break;

            case FromDirection.FromLeft:
                _toDirection = Vector3.right; break;

            case FromDirection.FromRight:
                _toDirection = Vector3.left; break;
        }
    }

    //private bool checkValidEnter()
    //{
    //    switch (_fromDirection)
    //    {
    //        default: return true;
    //
    //        case FromDirection.FromAbove:
    //            return PlayerLogic.Player.transform.position.y > transform.position.y;
    //
    //        case FromDirection.FromBelow:
    //            return PlayerLogic.Player.transform.position.y < transform.position.y;
    //
    //        case FromDirection.FromLeft:
    //            return PlayerLogic.Player.transform.position.x < transform.position.x;
    //
    //        case FromDirection.FromRight:
    //            return PlayerLogic.Player.transform.position.x > transform.position.x;
    //    }
    //}

    private bool checkValidEnter(Collision2D collision)
    {
        Vector2 playerPos = PlayerLogic.Player.transform.position;

        switch (_fromDirection)
        {
            default: return true;

            case FromDirection.FromAbove:
                foreach (ContactPoint2D contact in collision.contacts)
                    if (contact.point.y < playerPos.y) return true;
                return false;

            case FromDirection.FromBelow:
                foreach (ContactPoint2D contact in collision.contacts)
                    if (contact.point.y > playerPos.y) return true;
                return false;

            case FromDirection.FromLeft:
                foreach (ContactPoint2D contact in collision.contacts)
                    if (contact.point.x > playerPos.x) return true;
                return false;

            case FromDirection.FromRight:
                foreach (ContactPoint2D contact in collision.contacts)
                    if (contact.point.x < playerPos.x) return true;
                return false;
        }
    }

    private void startMovingPlayer()
    {
        PlayerLogic.LockPlayer();
        //PlayerLogic.IgnorePlayerGroundCollision(true);
        _isMovingPlayer = true;
        PlayerLogic.Player.transform.position += 0.1f * _toDirection;
    }

    private void finishMovingPlayer()
    {
        _isMovingPlayer = false;
        //PlayerLogic.IgnorePlayerGroundCollision(false);
        PlayerLogic.FreePlayer();

        if (PlayerLogic.Player.IsOnLadder) PlayerLogic.Player.SetPlayerOnLadder(true);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (!PlayerLogic.PlayerIsLocked && checkValidEnter(collision))
                startMovingPlayer();
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (!_isMovingPlayer && !PlayerLogic.PlayerIsLocked && checkValidEnter(collision))
                startMovingPlayer();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //PlayerLogic.IgnorePlayerGroundCollision(false);
            if (!Physics2D.OverlapBox(transform.position, transform.lossyScale, transform.eulerAngles.z, Layers.PlayerLayer.MaskValue))
                finishMovingPlayer();
            //else
            //    PlayerLogic.IgnorePlayerGroundCollision(true);
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        MyMath.DrawWireBox(transform.position, transform.lossyScale, transform.eulerAngles.z, Color.white, 0);
    }
#endif
}
