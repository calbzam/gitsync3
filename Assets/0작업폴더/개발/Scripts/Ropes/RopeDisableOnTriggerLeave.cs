using Obi;
using UnityEngine;

public class RopeDisableOnTriggerLeave : MonoBehaviour
{
    [Help("Needs BoxCollider2D on this object")]

    [SerializeField] private ObiSolver _solverToDisable;
    [SerializeField] private GameObject[] _additionalObjectsToDisable;

    private ColBounds2D _triggerColBounds;

    private void Start()
    {
        _triggerColBounds = new ColBounds2D(gameObject.GetComponent<BoxCollider2D>());
        evalPlayerIsInSelf();
    }

    private void evalPlayerIsInSelf()
    {
        SetObjectsActive(_triggerColBounds.OtherIsInSelf(PlayerLogic.Player.transform));
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            SetObjectsActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            SetObjectsActive(false);
        }
    }

    private void SetObjectsActive(bool enabled)
    {
        if (enabled)
        {
            _solverToDisable.gameObject.SetActive(true);
            foreach (var obj in _additionalObjectsToDisable) obj.SetActive(true);
        }
        else
        {
            foreach (var obj in _additionalObjectsToDisable) obj.SetActive(false);
            _solverToDisable.gameObject.SetActive(false);
        }
    }
}
