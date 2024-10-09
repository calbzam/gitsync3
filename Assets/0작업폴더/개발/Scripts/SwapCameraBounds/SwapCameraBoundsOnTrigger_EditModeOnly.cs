using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[ExecuteInEditMode]
public class SwapCameraBoundsOnTrigger_EditModeOnly : MonoBehaviour
{
    [SerializeField] private CinemachineConfiner _virtualCamDefaultConfiner;
    [SerializeField] private PolygonCollider2D _onTriggerEnterBounds;
    [SerializeField] private BoxCollider2D _triggerColSelf;
    private ColBounds2D _triggerColBounds;
    private Transform _playerTransform;

    private void Awake()
    {
        if (Application.isPlaying)
        {
            this.enabled = false;
        }
        else
        {
            _triggerColBounds = new ColBounds2D(_triggerColSelf);
            _playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        }
    }

    private void Start()
    {
        if (Application.isPlaying)
        {
            this.enabled = false;
        }
    }

#if UNITY_EDITOR
    private void Update()
    {
        if (_triggerColBounds.OtherIsInSelf(_playerTransform))
        {
            _virtualCamDefaultConfiner.m_BoundingShape2D = _onTriggerEnterBounds;
        }
    }
#endif
}
