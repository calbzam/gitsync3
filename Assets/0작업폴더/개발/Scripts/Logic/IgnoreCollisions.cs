using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreCollisions : MonoBehaviour
{
    private void Awake()
    {
        Physics2D.IgnoreLayerCollision(Layers.DefaultLayer.LayerValue, Layers.NoCollisionLayer.LayerValue);
        Physics2D.IgnoreLayerCollision(Layers.PlayerLayer.LayerValue, Layers.NoCollisionLayer.LayerValue);
    }
}
