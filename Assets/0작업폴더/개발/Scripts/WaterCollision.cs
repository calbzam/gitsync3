using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterCollision : MonoBehaviour
{
    //[SerializeField] private BoxCollider2D _waterCol;
    //[SerializeField] private BuoyancyEffector2D _effector;

    //private void OnTriggerEnter2D(Collider2D col)
    //{
    //    if (col.CompareTag("Player"))
    //    {
    //        PlayerLogic.Player.IsInWater = true; // unneeded, executed in PlayerController.cs
    //    }
    //}

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            PlayerLogic.Player.IsInWater = false;
        }
    }
}
