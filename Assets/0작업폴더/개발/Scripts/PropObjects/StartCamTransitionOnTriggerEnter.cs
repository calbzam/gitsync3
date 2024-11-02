using UnityEngine;

public class StartCamTransitionOnTriggerEnter : MonoBehaviour
{
    [SerializeField] private TransitionCameraTo _transitionCameraTo;
    [SerializeField] private LeverBatteryReader _leverBatteryReaderForEval;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag(Tags.PlayerTag))
        {
            if (_leverBatteryReaderForEval == null || _leverBatteryReaderForEval.BatteryInserted)
                _transitionCameraTo.StartTransitionToDestCam();
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag(Tags.PlayerTag))
        {
            if (_leverBatteryReaderForEval == null || _leverBatteryReaderForEval.BatteryInserted)
                _transitionCameraTo.TransitionBackToDefaultCam();
        }
    }
}
