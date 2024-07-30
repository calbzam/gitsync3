using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderTrigger : MonoBehaviour
{
    [SerializeField] private Transform _topPoint;
    [SerializeField] private Transform _bottomPoint;
    public Transform TopPoint => _topPoint; // for public access
    public Transform BottomPoint => _bottomPoint;

    //private bool _playerIsOnLadder;
    //private void Start()
    //{
    //    _playerIsOnLadder = false;
    //}

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            PlayerLogic.Player.IsInLadderRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            PlayerLogic.Player.IsInLadderRange = false;
        }
    }
}
