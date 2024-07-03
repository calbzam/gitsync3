using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreCollisions : MonoBehaviour
{
    private void Start()
    {
        Physics2D.IgnoreLayerCollision(0, 8); // Default layer  <--> No Collision layer
        Physics2D.IgnoreLayerCollision(3, 8); // Player layer   <--> No Collision layer
    }
}
