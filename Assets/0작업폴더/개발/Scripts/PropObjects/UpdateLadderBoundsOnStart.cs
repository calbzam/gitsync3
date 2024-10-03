using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateLadderBoundsOnStart : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRendererToRead;
    [SerializeField] private BoxCollider2D _boxcol2DToChange;
    [SerializeField] private Transform _topPoint;
    [SerializeField] private Transform _bottomPoint;
    [SerializeField] private Transform _noAutoClimbArea;
    [SerializeField] private LadderTrigger _ladderTrigger;

    private void Start()
    {
        // _boxcol2DToChange
        Quaternion spRot = _spriteRendererToRead.transform.rotation;
        _spriteRendererToRead.transform.rotation = Quaternion.identity; // bounds is the bounding box including rotation, so remove the rotation then rotate back after getting the size

        _boxcol2DToChange.size = _spriteRendererToRead.bounds.size;
        _spriteRendererToRead.transform.rotation = spRot;

        // _topPoint, _bottomPoint
        _topPoint.localPosition = new Vector2(0, _boxcol2DToChange.bounds.extents.y);
        _bottomPoint.localPosition = new Vector2(0, -_boxcol2DToChange.bounds.extents.y);
        _ladderTrigger.UpdateLadderDirection();

        // _noAutoClimbArea
        _noAutoClimbArea.localPosition = new Vector2(0, _boxcol2DToChange.bounds.extents.y);
        _noAutoClimbArea.localScale = new Vector2(_boxcol2DToChange.bounds.size.x / transform.parent.lossyScale.x + 0.1f, _noAutoClimbArea.localScale.y);
    }
}
