using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[ExecuteInEditMode]
public class LightsToggle3D_EditModeOnly : MonoBehaviour
{
    [SerializeField] private BoxCollider _triggerColToUse;
    private ColBounds3D _triggerColBounds;
    [SerializeField] private Light[] _lightsToToggle;

    private Collider _mainCameraCol;
    private bool _camIsInTrigger;

    private bool setCamIsInTrigger()
    {
        return _camIsInTrigger = _triggerColBounds.OtherIsInSelf(_mainCameraCol);
    }

    private void Awake()
    {
        if (Application.isPlaying)
        {
            foreach (Light light in _lightsToToggle) light.enabled = true;
            this.enabled = false;
        }
        else
        {
            _triggerColBounds = new ColBounds3D(_triggerColToUse);
            _mainCameraCol = Camera.main.GetComponentInChildren<Collider>();
            setCamIsInTrigger();
        }
    }

    private void Start()
    {
        if (Application.isPlaying)
        {
            foreach (Light light in _lightsToToggle) light.enabled = true;
            this.enabled = false;
        }
    }

#if UNITY_EDITOR
    private void Update()
    {
        if (!Application.isPlaying) // edit mode, not playing
        {
            if (!_camIsInTrigger)
            {
                if (setCamIsInTrigger())
                    foreach (Light light in _lightsToToggle) light.enabled = true;
            }
            else // _camIsInTrigger
            {
                if (!setCamIsInTrigger())
                    foreach (Light light in _lightsToToggle) light.enabled = false;
            }
        }
    }
#endif
}
