using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FromDirection = ColBounds2D.FromDirection;

[ExecuteInEditMode]
public class LightsToggle2D_EditModeOnly : MonoBehaviour
{
    [SerializeField] private BoxCollider2D _triggerColSelf;
    private ColBounds2D _triggerColBounds;
    [SerializeField] private Light[] _lightsToToggle;
    [SerializeField] private FromDirection _enableWhenEntered = FromDirection.FromBelow;
    [SerializeField] private FromDirection _disableWhenEntered = FromDirection.FromAbove;

    private Collider2D _mainCameraCol;

    private void Awake()
    {
        if (Application.isPlaying)
        {
            foreach (Light light in _lightsToToggle) light.enabled = true;
            this.enabled = false;
        }
        else
        {
            _triggerColBounds = new ColBounds2D(_triggerColSelf);
            _mainCameraCol = Camera.main.GetComponentInChildren<Collider2D>();
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
            evalLightsActiveState();
        }
    }

    private void evalLightsActiveState()
    {
        FromDirection evalDir = _triggerColBounds.GetRelativePosition(_mainCameraCol);

        if (evalDir == _disableWhenEntered)
        {
            foreach (Light light in _lightsToToggle) light.enabled = true;
        }
        else if (evalDir == _enableWhenEntered)
        {
            foreach (Light light in _lightsToToggle) light.enabled = false;
        }
    }
#endif
}
