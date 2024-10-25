using UnityEngine;

public class Letterboxes : MonoBehaviour
{
    [SerializeField] private RectTransform _topLetterbox;
    [SerializeField] private RectTransform _bottomLetterbox;
    [SerializeField] private float _moveSpeed = 0.4f;
    [SerializeField] private float _activatedDistance = 700;
    private float _origDistance;

    private bool _activated;
    private float _movePercent;

    public void Activateboxes(bool enabled)
    {
        _movePercent = 0;
        _activated = enabled;
    }

    private void Awake()
    {
        _activated = false;
        _movePercent = 0;
    }

    private void Start()
    {
        _origDistance = _topLetterbox.anchoredPosition.y;
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

    private void Update()
    {
        if (_activated)
        {
            MoveToTargetDistance(_activatedDistance);
        }
        else
        {
            MoveToTargetDistance(_origDistance);
        }
    }
}
