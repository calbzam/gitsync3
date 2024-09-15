using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TentBreak : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _selfRenderer;
    [SerializeField] private Collider2D _selfCol;

    [SerializeField] private Transform _debrisParent;
    private Transform[] _debrisChildren;

    private Vector3[] _debrisChildrenPos;
    private Quaternion[] _debrisChildrenRot;

    private bool _timerEnabled;

    private void OnEnable()
    {
        PlayerLogic.PlayerRespawned += PlayerRespawnedAction;
    }

    private void OnDisable()
    {
        PlayerLogic.PlayerRespawned -= PlayerRespawnedAction;
    }

    private void PlayerRespawnedAction()
    {
        ResetPanelTransforms();
    }

    private void Start()
    {
        _timerEnabled = false;

        if (_debrisParent != null)
        {
            _debrisChildren = _debrisParent.GetComponentsInChildren<Transform>();
            SetTransformVariables();
        }
        SetPanelBroken(false);
    }

    private void SetTransformVariables()
    {
        _debrisChildrenPos = new Vector3[_debrisChildren.Length];
        _debrisChildrenRot = new Quaternion[_debrisChildren.Length];

        for (int i = 0; i < _debrisChildren.Length; ++i)
        {
            _debrisChildrenPos[i] = _debrisChildren[i].position;
            _debrisChildrenRot[i] = _debrisChildren[i].rotation;
        }
    }

    private void ResetPanelTransforms()
    {
        SetPanelBroken(false);

        if (_debrisParent != null)
            for (int i = 0; i < _debrisChildren.Length; ++i)
            {
                _debrisChildren[i].position = _debrisChildrenPos[i];
                _debrisChildren[i].rotation = _debrisChildrenRot[i];
            }
    }

    private void SetPanelBroken(bool isBroken)
    {
        if (_debrisParent != null)
            foreach (Transform debris in _debrisChildren) debris.gameObject.SetActive(isBroken);

        _selfRenderer.enabled = !isBroken;
        _selfCol.enabled = !isBroken;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _timerEnabled = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (_timerEnabled) SetPanelBroken(true);
        }
    }
}
