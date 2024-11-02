using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class ActivateRbsInChildrenOnTriggerExit : MonoBehaviour
{
    [SerializeField] private GameObject _objToDetectExit;
    [SerializeField] private bool _positionToDetectionobjOnActivate = false;
    [SerializeField] private bool _removeParentconstraintInRbparentsOnActivate = false;
    [SerializeField] private GameObject[] _rbparentsToActivateOnExit;
    private List<Rigidbody2D> _allChildrenRbs;

    private int _instanceIDToDetectExit;

    private bool _hasLeftOnce;

    private void Awake()
    {
        _hasLeftOnce = false;
        _allChildrenRbs = new List<Rigidbody2D>();
    }

    private void Start()
    {
        _instanceIDToDetectExit = _objToDetectExit.GetInstanceID();

        foreach (var obj in _rbparentsToActivateOnExit)
        {
            var rbChildren = obj.GetComponentsInChildren<Rigidbody2D>();
            foreach (var rb in rbChildren) { _allChildrenRbs.Add(rb); rb.simulated = false; }
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (!_hasLeftOnce && col.gameObject.GetInstanceID() == _instanceIDToDetectExit)
        {
            foreach (var obj in _rbparentsToActivateOnExit)
            {
                if (_positionToDetectionobjOnActivate) obj.transform.position = _objToDetectExit.transform.position;
                if (_removeParentconstraintInRbparentsOnActivate) obj.GetComponent<ParentConstraint>().enabled = false;
            }

            foreach (var rb in _allChildrenRbs) rb.simulated = true;

            _hasLeftOnce = true;
        }
    }
}
