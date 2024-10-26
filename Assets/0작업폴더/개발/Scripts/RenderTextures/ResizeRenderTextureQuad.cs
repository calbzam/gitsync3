using UnityEngine;

public class ResizeRenderTextureQuad : MonoBehaviour
{
    [SerializeField] private Camera _outputCamera;

    private Vector2 _canvasSize;

    private void Awake()
    {
        _canvasSize = Vector2.zero;
    }

    private void Update()
    {
        if (_canvasSize != CanvasLogic.CanvasRectTransform.rect.size)
        {
            _canvasSize = CanvasLogic.CanvasRectTransform.rect.size;
            SetQuadSize();
        }
    }

    private void SetQuadSize()
    {
        //https://discussions.unity.com/t/scale-gameobject-to-the-width-of-screen/121090/2
        float width = _outputCamera.orthographicSize * 2.0f * Screen.width / Screen.height;
        float height = _outputCamera.orthographicSize * 2.0f;
        transform.localScale = new Vector3(width, height, transform.localScale.z);
    }
}
