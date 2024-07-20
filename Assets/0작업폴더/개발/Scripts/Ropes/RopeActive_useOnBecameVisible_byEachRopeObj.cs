using Obi;
using UnityEngine;

public class RopeActive_useOnBecameVisible_byEachRopeObj : MonoBehaviour
{
    [SerializeField] private MeshRenderer _meshRenderer;
    [SerializeField] private ObiRope _rope;
    [SerializeField] private GameObject[] _additionalChildObjectsToDisable;

    private void Start()
    {
        if (_meshRenderer.isVisible) onVisible();
        else onInvisible();
    }

    private void OnBecameVisible()
    {
        onVisible();
    }

    private void OnBecameInvisible()
    {
        onInvisible();
    }

    private void onVisible()
    {
        _rope.solver.enabled = true;
        foreach (var child in _additionalChildObjectsToDisable) child.SetActive(true);
    }

    private void onInvisible()
    {
        foreach (var child in _additionalChildObjectsToDisable) child.SetActive(false);
        _rope.solver.enabled = false;
    }
}
