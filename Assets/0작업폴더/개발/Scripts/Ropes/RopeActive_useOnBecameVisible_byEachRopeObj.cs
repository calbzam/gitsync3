using Obi;
using UnityEngine;

public class RopeActive_useOnBecameVisible_byEachRopeObj : MonoBehaviour
{
    [Help("Needs ObiRope and MeshRenderer on this object")]
    private ObiRope _rope;
    private MeshRenderer _meshRenderer;

    [SerializeField] private GameObject[] _additionalChildObjectsToDisable;

    private void Awake()
    {
        _rope = gameObject.GetComponent<ObiRope>();
        _meshRenderer = gameObject.GetComponent<MeshRenderer>();
    }

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
