using UnityEngine;
using UnityEngine.UI;
using Math = System.Math;

public static class MyMath
{
    public static float Remap(float value, float min1, float max1, float min2, float max2)
    {
        return min2 + (value - min1) * (max2 - min2) / (max1 - min1);
    }

    public static float flatSqrMagnitude(Vector3 diff)
    {
        //return (float)(Math.Pow(diff.x, 2) + Math.Pow(diff.z, 2));
        return diff.x * diff.x + diff.z * diff.z;
    }

    public static bool Vector2DiffLessThan(Vector2 v1, Vector2 v2, float delta)
    {
        return Mathf.Abs(v1.x - v2.x) < delta && Mathf.Abs(v1.y - v2.y) < delta;
    }

    public static bool Vector3DiffLessThan(Vector3 v1, Vector3 v2, float delta)
    {
        return Mathf.Abs(v1.x - v2.x) < delta && Mathf.Abs(v1.y - v2.y) < delta && Mathf.Abs(v1.z - v2.z) < delta;
    }

    public static void limit2dVelocity(Rigidbody rb, float moveSpeed)
    {
        Vector3 flat = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if (flatSqrMagnitude(flat) > moveSpeed * moveSpeed)
        {
            flat = flat.normalized * moveSpeed; // limit maximum velocity to moveSpeed
            rb.velocity = new Vector3(flat.x, rb.velocity.y, flat.z);
        }
    }

    public static float lerp(float src, float trgt, float percent)
    {
        return src + percent * (trgt - src);
    }

    // https://discussions.unity.com/t/any-one-know-maths-behind-this-movetowards-function/65501/4
    public static float moveTowards(float current, float target, float maxDelta)
    {
        if (Math.Abs(target - current) < maxDelta) return target;
        return current + Math.Sign(target - current) * maxDelta;
    }

    public static float updatePercentDT(float current, float moveSpeed)
    {
        return moveTowards(current, 1f, moveSpeed * Time.deltaTime);
    }

    public static bool CheckAnimFinished(Animator anim)
    {
        return (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);
    }

    // https://www.reddit.com/r/Unity2D/comments/d9arky/comment/f1fwaog/?utm_source=share&utm_medium=web3x&utm_name=web3xcss&utm_term=1&utm_content=share_button
    /* I added the particular offset for each case (north,south,west,east)
     * and then the overlapbox was in the correct position.
     * It was a very easy thing to solve once I had the proper tools for visualisation.
     * A user that goes by the name DMGregory on StackExchange gave the solution for the last problem
     * with which I was easily able to see where overlapbox was.
     * You don't have to use gizmos to rotate the overlapbox, just use this custom function made by him. 
     */
    public static void DrawWireBox(Vector2 point, Vector2 size, float angle, Color color, float duration)
    {
        var orientation = Quaternion.Euler(0, 0, angle);

        // Basis vectors, half the size in each direction from the center.
        Vector2 right = orientation * Vector2.right * size.x / 2f;
        Vector2 up = orientation * Vector2.up * size.y / 2f;

        // Four box corners.
        var topLeft = point + up - right;
        var topRight = point + up + right;
        var bottomRight = point - up + right;
        var bottomLeft = point - up - right;

        // Now we've reduced the problem to drawing lines.
        Debug.DrawLine(topLeft, topRight, color, duration);
        Debug.DrawLine(topRight, bottomRight, color, duration);
        Debug.DrawLine(bottomRight, bottomLeft, color, duration);
        Debug.DrawLine(bottomLeft, topLeft, color, duration);
    }
}
