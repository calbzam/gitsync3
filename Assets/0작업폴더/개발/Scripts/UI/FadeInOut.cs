using System;
using System.Collections;
using UnityEngine;

public class FadeInOut : MonoBehaviour
{
    [SerializeField] private float _fadeSpeed = 2.2f;
    [SerializeField] private float _fadeHoldDuration = 0.7f;
    private float _fadeOutHoldTime;

    private CanvasGroup _fadeInOutUI;
    public FadingState CurrentState { get; private set; }

    private int _fromInstanceID;

    public event Action<int> FadeOutFinished;

    public enum FadingState
    {
        Ready,
        IsFadingIn,
        IsFadingOut,
        HoldFade,
    }

    public void StartFadeOutIn(int fromInstanceID)
    {
        _fromInstanceID = fromInstanceID;
        CurrentState = FadingState.IsFadingIn;
    }

    private void Awake()
    {
        CurrentState = FadingState.Ready;
        _fadeInOutUI = gameObject.GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        _fadeInOutUI.alpha = 0;
    }

    private void Update()
    {
        EvalFading();
    }

    private void EvalFading()
    {
        switch (CurrentState)
        {
            case FadingState.IsFadingIn:
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
                    CurrentState = FadingState.IsFadingOut;
                break;

            case FadingState.IsFadingOut:
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
