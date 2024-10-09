using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSurfaceCollision : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            PlayerLogic.Player.IsOnWater = true;
        }
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            if (FrameInputReader.FrameInput.InputDir.y < 0)
                PlayerLogic.Player.IsInWater = true;
            else
                PlayerLogic.Player.IsInWater = false;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            PlayerLogic.Player.IsOnWater = false;
        }
    }
}
