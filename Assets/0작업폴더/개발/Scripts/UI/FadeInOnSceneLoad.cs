using System.Collections;
using UnityEngine;

public class FadeInOnSceneLoad : MonoBehaviour
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
        StartCoroutine(WaitBeforeFadeIn());
    }

    private IEnumerator WaitBeforeFadeIn()
    {
        yield return new WaitForSeconds(0.3f);
        _fadeInOut.StartFadeInOnly();
    }
}
