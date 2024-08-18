using UnityEngine;

public class ColBounds3D
{
    public ColBounds3D(Collider selfCol)
    {
        SelfCollider = selfCol;
        GetColBounds();
    }

    public Collider SelfCollider;
    public float RightEnd, LeftEnd, TopEnd, BottomEnd, BackEnd, FrontEnd;

    public void GetColBounds()
    {
        RightEnd = SelfCollider.bounds.center.x + SelfCollider.bounds.extents.x;
        LeftEnd = SelfCollider.bounds.center.x - SelfCollider.bounds.extents.x;
        TopEnd = SelfCollider.bounds.center.y + SelfCollider.bounds.extents.y;
        BottomEnd = SelfCollider.bounds.center.y - SelfCollider.bounds.extents.y;
        BackEnd = SelfCollider.bounds.center.z + SelfCollider.bounds.extents.z;
        FrontEnd = SelfCollider.bounds.center.z - SelfCollider.bounds.extents.z;
    }

    public bool OtherIsInCollider(Collider other)
    {
        if (other.transform.position.x > LeftEnd && other.transform.position.x < RightEnd)
        {
            if (other.transform.position.y > BottomEnd && other.transform.position.y < TopEnd)
                if (other.transform.position.z > FrontEnd && other.transform.position.z < BackEnd)
                    return true;
        }
        return false;
    }
}
