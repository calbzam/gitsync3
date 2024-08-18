using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraBounds : MonoBehaviour
{
    [SerializeField] private CinemachineConfiner _virtualCamDefaultConfiner;
    public static CinemachineConfiner VirtualCamDefaultConfiner { get; set; }

    [Header("Camera Bounds")]
    [SerializeField] private PolygonCollider2D _surfaceBounds;
    [SerializeField] private PolygonCollider2D _bunkerLadderBounds;
    public static PolygonCollider2D SurfaceBounds { get; set; }
    public static PolygonCollider2D BunkerLadderBounds { get; set; }

    private void Awake()
    {
        VirtualCamDefaultConfiner = _virtualCamDefaultConfiner;
        SurfaceBounds = _surfaceBounds;
        BunkerLadderBounds = _bunkerLadderBounds;
    }
}
