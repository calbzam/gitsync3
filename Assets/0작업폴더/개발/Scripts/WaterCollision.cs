using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterCollision : MonoBehaviour
{
    //[SerializeField] private BoxCollider2D _waterCol;
    //[SerializeField] private BuoyancyEffector2D _effector;

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
