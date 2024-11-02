using UnityEngine;

public class LetGoOfPullablePanelsOnTriggerExit : MonoBehaviour
{
    [SerializeField] private GameObject _objToDetectExit;
    [SerializeField] private PullablePanel[] _panelsToDisconnectFromPlayer;

    private int _instanceIDToDetectExit;

    private bool _hasLeftOnce;

    private void Awake()
    {
        _hasLeftOnce = false;
    }

    private void Start()
    {
        _instanceIDToDetectExit = _objToDetectExit.GetInstanceID();
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (!_hasLeftOnce && col.gameObject.GetInstanceID() == _instanceIDToDetectExit)
        {
            foreach (var panel in _panelsToDisconnectFromPlayer) panel.LetgoPanel();

            _hasLeftOnce = true;
        }
    }
}
