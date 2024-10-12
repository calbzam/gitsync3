using Obi;
using UnityEngine;

public class RopeDisableOnTriggerLeave : MonoBehaviour
{
    [Help("Needs BoxCollider2D on this object")]

    [SerializeField] private ObiSolver _solverToDisable;
    [SerializeField] private GameObject[] _additionalObjectsToDisable;

    private ColBounds3D _triggerColBounds;
    private Collider _mainCameraCol;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            _solverToDisable.gameObject.SetActive(true);
            foreach (var obj in _additionalObjectsToDisable) obj.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            foreach (var obj in _additionalObjectsToDisable) obj.SetActive(false);
            _solverToDisable.gameObject.SetActive(false);
        }
    }
}
