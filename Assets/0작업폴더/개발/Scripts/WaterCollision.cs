using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterCollision : MonoBehaviour
{
    //[SerializeField] private BoxCollider2D _waterCol;
    //[SerializeField] private BuoyancyEffector2D _effector;

    private BoxCollider2D _col;
    private ColBounds2D _colBounds;

    private void Awake()
    {
        _col = gameObject.GetComponent<BoxCollider2D>();
        _colBounds = new ColBounds2D(_col);
    }

    private void Start()
    {
        evalPlayerInitiallyInWater();
    }

    private void evalPlayerInitiallyInWater()
    {
        if (_colBounds.OtherIsInSelf(PlayerLogic.Player.transform))
        {
            PlayerLogic.Player.IsInWater = true;
            if (!PlayerLogic.Player.DiveSwimAllowed) PlayerLogic.Player.DiveSwimAllowed = true;
        }
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            if (!PlayerLogic.Player.IsInWater)
            {
                if (FrameInputReader.FrameInput.InputDir.y < 0)
                    PlayerLogic.Player.IsInWater = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            PlayerLogic.Player.IsInWater = false;
        }
    }
}
