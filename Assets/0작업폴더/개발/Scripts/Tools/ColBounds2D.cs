using UnityEngine;

public class ColBounds2D
{
    public ColBounds2D(Collider2D selfCol)
    {
        SelfCollider = selfCol;
        GetColBounds();
    }

    public Collider2D SelfCollider;
    public float RightEnd, LeftEnd, TopEnd, BottomEnd;
    
    public enum FromDirection
    {
        None,
        FromAbove,
        FromBelow,
        FromLeft,
        FromRight,
    }

    public void GetColBounds()
    {
        RightEnd = SelfCollider.bounds.center.x + SelfCollider.bounds.extents.x;
        LeftEnd = SelfCollider.bounds.center.x - SelfCollider.bounds.extents.x;
        TopEnd = SelfCollider.bounds.center.y + SelfCollider.bounds.extents.y;
        BottomEnd = SelfCollider.bounds.center.y - SelfCollider.bounds.extents.y;
    }

    public FromDirection GetRelativePosition(Collider2D col)
    {
        if (col.transform.position.y > TopEnd) return FromDirection.FromAbove;
        else if (col.transform.position.y < BottomEnd) return FromDirection.FromBelow;
        else if (col.transform.position.x < LeftEnd) return FromDirection.FromLeft;
        else if (col.transform.position.x > RightEnd) return FromDirection.FromRight;
        else return FromDirection.None;
    }

    public bool OtherIsInSelf(Collider2D other)
    {
        if (other.transform.position.x > LeftEnd && other.transform.position.x < RightEnd)
        {
            if (other.transform.position.y > BottomEnd && other.transform.position.y < TopEnd)
                return true;
        }
        return false;
    }

    public bool OtherIsInSelf(Transform other)
    {
        if (LeftEnd < other.position.x && other.position.x < RightEnd)
        {
            if (BottomEnd < other.position.y && other.position.y < TopEnd)
                return true;
        }
        return false;
    }
}
