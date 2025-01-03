using System;
using UnityEngine;

public class FadeInOut : MonoBehaviour
{
    [SerializeField] private RectTransform _canvasRectTransform;
    private Vector2 _canvasSize;

    [SerializeField] private float _fadeSpeed = 2.2f;
    [SerializeField] private float _fadeHoldDuration = 0.7f;
    private float _fadeOutHoldTime;

    private CanvasGroup _fadeInOutUI;
    private RectTransform _fadeInOutUIRectTransform;
    public FadingState CurrentState { get; private set; }

    private int _fromInstanceID;

    public event Action<int> FadeOutFinished;

    public enum FadingState
    {
        Ready,
        IsFadingOut, // screen turning to black
        IsFadingIn, // screen turning to scene
        HoldFade,
    }

    public void StartFadeOutIn(int fromInstanceID)
    {
        _fromInstanceID = fromInstanceID;
        CurrentState = FadingState.IsFadingOut;
    }

    public void StartFadeInOnly()
    {
        CurrentState = FadingState.IsFadingIn;
    }

    private void Awake()
    {
        _canvasSize = Vector2.zero;
        CurrentState = FadingState.Ready;
        _fadeInOutUI = gameObject.GetComponent<CanvasGroup>();
        _fadeInOutUIRectTransform = _fadeInOutUI.GetComponent<RectTransform>();
    }

    private void EvalFadeInOutUISize()
    {
        if (_canvasSize != _canvasRectTransform.rect.size)
        {
            //_canvasScreenSize = new Vector2(1920, 1920f / Screen.width * Screen.height);
            _canvasSize = _canvasRectTransform.rect.size;
            _fadeInOutUIRectTransform.sizeDelta = _canvasSize;
        }
    }

    private void Update()
    {
        EvalFadeInOutUISize();
        EvalFading();
    }

    private void EvalFading()
    {
        switch (CurrentState)
        {
            case FadingState.IsFadingOut:
                _fadeInOutUI.alpha = Mathf.MoveTowards(_fadeInOutUI.alpha, 1, _fadeSpeed * Time.deltaTime);
                if (_fadeInOutUI.alpha >= 1)
                {
                    _fadeOutHoldTime = Time.time;
                    FadeOutFinished?.Invoke(_fromInstanceID);
                    CurrentState = FadingState.HoldFade;
                }
                break;

            case FadingState.HoldFade:
                if (Time.time - _fadeOutHoldTime > _fadeHoldDuration)
                    CurrentState = FadingState.IsFadingIn;
                break;

            case FadingState.IsFadingIn:
                _fadeInOutUI.alpha = Mathf.MoveTowards(_fadeInOutUI.alpha, 0, _fadeSpeed * Time.deltaTime);
                if (_fadeInOutUI.alpha <= 0)
                {
                    CurrentState = FadingState.Ready;
                }
                break;

            default:
                break;
        }
    }
}
