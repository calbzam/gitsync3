using UnityEngine;

public struct LayerStruct
{
    public LayerStruct(int maskValue)
    {
        MaskValue = maskValue;
        for (LayerValue = 0; (maskValue >>= 1) != 0; ++LayerValue) ;
    }

    public int MaskValue;
    public int LayerValue;
}

public static class Layers
{
    public static LayerStruct DefaultLayer = new LayerStruct(LayerMask.GetMask("Default"));
    public static LayerStruct PlayerLayer = new LayerStruct(LayerMask.GetMask("Player"));
    public static LayerStruct NoCollisionLayer = new LayerStruct(LayerMask.GetMask("No Collision"));

    public static LayerStruct GroundLayer = new LayerStruct(LayerMask.GetMask("Ground"));
    public static LayerStruct SwingingGroundLayer = new LayerStruct(LayerMask.GetMask("SwingingGround"));
    public static LayerStruct PushableBoxLayer = new LayerStruct(LayerMask.GetMask("Pushable Box"));

    public static LayerStruct ClimbingRopeLayer = new LayerStruct(LayerMask.GetMask("ClimbingRope"));
    public static LayerStruct DoNotRenderLayer = new LayerStruct(LayerMask.GetMask("Do Not Render"));

    public static int GetLayerMask(int layerValue)
    {
        return 1 << layerValue;
    }

    public static int GetLayerValue(int layerMask)
    {
        int value = 0;
        while ((layerMask >>= 1) != 0) ++value;
        return value;
    }

    // source: https://forum.unity.com/threads/checking-if-a-layer-is-in-a-layer-mask.1190230/#post-7613611
    public static bool LayerIsInLayerMask(int layer, LayerMask layerMask)
    {
        return (layerMask & (1 << layer)) != 0;
    }
}
