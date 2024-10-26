using UnityEngine;

public class ResizeRenderTexture : MonoBehaviour
{
    private Vector2 _canvasSize;

    [SerializeField] private Camera _defaultCamera;
    [SerializeField] private Camera _backgroundCamera;
    [SerializeField] private Camera _outputCamera;
    [SerializeField] private MeshRenderer _defaultQuad;
    [SerializeField] private MeshRenderer _backgroundQuad;

    private void Awake()
    {
        _canvasSize = Vector2.zero;
    }

    private void Update()
    {
        if (_canvasSize != CanvasLogic.CanvasRectTransform.rect.size)
        {
            _canvasSize = CanvasLogic.CanvasRectTransform.rect.size;
            Resize(_defaultCamera.targetTexture, (int)_canvasSize.x, (int)_canvasSize.y);
            Resize(_backgroundCamera.targetTexture, (int)_canvasSize.x, (int)_canvasSize.y);
        }
    }

    // https://discussions.unity.com/t/resize-rendertexture-at-runtime/113573/3
    private void Resize(RenderTexture renderTexture, int width, int height)
    {
        if (renderTexture)
        {
            renderTexture.DiscardContents();
            renderTexture.Release();
            renderTexture.width = width;
            renderTexture.height = height;
        }
    }
}
