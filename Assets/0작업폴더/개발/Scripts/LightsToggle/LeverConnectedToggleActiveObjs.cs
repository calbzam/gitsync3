using UnityEngine;

public class LeverConnectedToggleActiveObjs : LeverConnectedObject
{
    [SerializeField] private GameObject[] _objsToToggleSetActive;
    [SerializeField] private bool _initiallyActivated = false;

    private void Awake() // run before Start() of each var obj is called
    {
        foreach (var obj in _objsToToggleSetActive)
            obj.SetActive(_initiallyActivated);
    }

    private void Start()
    {
        foreach (var obj in _objsToToggleSetActive)
            obj.SetActive(_initiallyActivated);
    }

    public override void ActivatedAction(bool enabledState)
    {
        foreach (var obj in _objsToToggleSetActive)
            obj.SetActive(enabledState);
    }
}
