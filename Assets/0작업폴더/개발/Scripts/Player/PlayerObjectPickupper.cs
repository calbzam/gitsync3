using UnityEngine;

public class PlayerObjectPickupper : MonoBehaviour
{
    private int _prevFaceDir;

    private void Awake()
    {
        _prevFaceDir = 1;
    }

    private void Update()
    {
        if (_prevFaceDir != PlayerLogic.AnimController.FaceDirX)
        {
            _prevFaceDir = PlayerLogic.AnimController.FaceDirX;
            transform.localPosition = new Vector3(-transform.localPosition.x, transform.localPosition.y, transform.localPosition.z);
        }
    }
}
