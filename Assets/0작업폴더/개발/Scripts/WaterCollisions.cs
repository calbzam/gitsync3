using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterCollisions : MonoBehaviour
{
    private BoxCollider2D waterCol;
    private BuoyancyEffector2D effector;

    private void Start()
    {
        waterCol = gameObject.GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
            col.GetComponent<PlayerController>().SetPlayerIsInWater(true);
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
            col.GetComponent<PlayerController>().SetPlayerIsInWater(false);
    }
}
