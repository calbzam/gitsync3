using UnityEngine;

public static class Layers
{
    public static LayerMask DefaultLayer = LayerMask.GetMask("Default");
    public static LayerMask PlayerLayer = LayerMask.GetMask("Player");
    public static LayerMask NoCollisionLayer = LayerMask.GetMask("No Collsion");

    public static LayerMask GroundLayer = LayerMask.GetMask("Ground");
    public static LayerMask SwingingGroundLayer = LayerMask.GetMask("SwingingGround");
    public static LayerMask PushableBoxLayer = LayerMask.GetMask("Pushable Box");

    public static LayerMask ClimbingRopeLayer = LayerMask.GetMask("ClimbingRope");
    public static LayerMask DoNotRenderLayer = LayerMask.GetMask("Do Not Render");
}
