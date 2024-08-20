using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[ExecuteInEditMode]
public class ControlVolumes_EditModeOnly : MonoBehaviour
{
    [SerializeField] private Volume[] _volumesToToggle;
    [SerializeField] private BoxCollider _triggerColToUse;

    private ColBounds3D _triggerColBounds;
    private Collider _mainCameraCol;

    private void Awake()
    {
        if (Application.isPlaying)
        {
            foreach (Volume volume in _volumesToToggle) volume.enabled = true;
            this.enabled = false;
        }
        else
        {
            _triggerColBounds = new ColBounds3D(_triggerColToUse);
            _mainCameraCol = Camera.main.GetComponentInChildren<Collider>();
        }
    }

    private void Start()
    {
        if (Application.isPlaying)
        {
            foreach (Volume volume in _volumesToToggle) volume.enabled = true;
            this.enabled = false;
        }
    }

#if UNITY_EDITOR
    private void Update()
    {
        if (!Application.isPlaying) // edit mode, not playing
        {
            if (_triggerColBounds.OtherIsInSelf(_mainCameraCol))
            {
                foreach (Volume volume in _volumesToToggle) volume.enabled = true;
            }
            else
            {
                foreach (Volume volume in _volumesToToggle) volume.enabled = false;
            }
        }
    }

    //private bool mainCamIsInTriggerCol()
    //{
    //    Collider[] colliders = Physics.OverlapBox(_triggerColToUse.bounds.center, _triggerColToUse.bounds.extents, Quaternion.identity, Layers.DefaultLayer.LayerValue);
    //    foreach (Collider col in colliders)
    //    {
    //        if (col.CompareTag("MainCamera Collider")) return true;
    //    }
    //    return false;
    //}
#endif
}
