using UnityEngine;

public class Letterboxes : MonoBehaviour
{
    [SerializeField] private RectTransform _canvasRectTransform;
    private Vector2 _canvasSize;

    [SerializeField] private RectTransform _topLetterbox;
    [SerializeField] private RectTransform _bottomLetterbox;
    [SerializeField] private float _moveSpeed = 0.4f;
    [SerializeField] private float _presetActivatedDistance = 700;
    private float _activatedDistance;
    private float _origDistance;

    private bool _activated;
    private float _movePercent;

    private bool temptoggleenable = false;
    [ContextMenu("activate Letterboxes")]
    public void activate()
    {
        Activateboxes(temptoggleenable = !temptoggleenable);
    }

    public void Activateboxes(bool enabled)
    {
        _movePercent = 0;
        _activated = enabled;
    }

    private void Awake()
    {
        _activated = false;
        _movePercent = 0;
        _canvasSize = Vector2.zero;
    }

    private void Update()
    {
        if (_canvasSize != _canvasRectTransform.rect.size)
        {
            //_canvasScreenSize = new Vector2(1920, 1920f / Screen.width * Screen.height);
            _canvasSize = _canvasRectTransform.rect.size;
            SetLetterboxesSizeAndPositions();
        }

        if (_activated)
        {
            MoveToTargetDistance(_activatedDistance);
        }
        else
        {
            MoveToTargetDistance(_origDistance);
        }
    }

    private void SetLetterboxesSizeAndPositions()
    {
        _topLetterbox.sizeDelta = new Vector2(_canvasSize.x, _canvasSize.y / 2);
        _bottomLetterbox.sizeDelta = new Vector2(_canvasSize.x, _canvasSize.y / 2);

        _origDistance = (_canvasSize.y / 2) + (_canvasSize.y / 4);
        _activatedDistance = _presetActivatedDistance / 1080 * _canvasSize.y;

        _topLetterbox.anchoredPosition = new Vector2(0, _origDistance);
        _bottomLetterbox.anchoredPosition = new Vector2(0, -_origDistance);
    }

    private void MoveToTargetDistance(float target)
    {
        if (_movePercent == 1f) return;

        float newY = _topLetterbox.anchoredPosition.y;
        _movePercent = Mathf.MoveTowards(_movePercent, 1, _moveSpeed * Time.deltaTime);

        newY = Mathf.Lerp(newY, target, _movePercent);
        _topLetterbox.anchoredPosition = new Vector2(_topLetterbox.anchoredPosition.x, newY);
        _bottomLetterbox.anchoredPosition = new Vector2(_bottomLetterbox.anchoredPosition.x, -newY);
    }
}
