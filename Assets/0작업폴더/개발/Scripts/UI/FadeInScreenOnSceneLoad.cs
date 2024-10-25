using UnityEngine;

public class FadeInScreenOnSceneLoad : MonoBehaviour
{
    [SerializeField] private FadeInOut _fadeInOut;
    private CanvasGroup _fadeInOutUI;

    private void Awake()
    {
        _fadeInOutUI = _fadeInOut.GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        _fadeInOutUI.alpha = 1;
        _fadeInOut.StartFadeInOnly();
    }
}
