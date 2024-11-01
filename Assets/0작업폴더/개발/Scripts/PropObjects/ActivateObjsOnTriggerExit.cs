using UnityEngine;

public class ActivateObjsOnTriggerExit : MonoBehaviour
{
    [SerializeField] private GameObject _objToDetectExit;
    [SerializeField] private bool _positionToDetectionobjOnActivate = false;
    [SerializeField] private GameObject[] _objsToActivateOnExit;

    private int _instanceIDToDetectExit;

    private bool _hasLeftOnce;

    private void Awake()
    {
        _hasLeftOnce = false;
    }

    private void Start()
    {
        _instanceIDToDetectExit = _objToDetectExit.GetInstanceID();

        foreach (var obj in _objsToActivateOnExit) obj.SetActive(false);
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (!_hasLeftOnce && col.gameObject.GetInstanceID() == _instanceIDToDetectExit)
        {
            foreach (var obj in _objsToActivateOnExit)
            {
                obj.SetActive(true);
                if (_positionToDetectionobjOnActivate) obj.transform.position = _objToDetectExit.transform.position;
            }
            _hasLeftOnce = true;
        }
    }
}
