using UnityEngine;

[ExecuteAlways]
public class LockTransform : MonoBehaviour
{
    // https://forum.unity.com/threads/lock-transforms-in-editor-script.779786/

    /// <summary>
    /// 
    /// Lock Transforms in editor script
    /// 
    /// Hi all!
    /// 
    /// I keep running into this issue where I accidentally transform the wrong object in a hierarchy,
    /// so I hacked together this little script that looks for any transforms I'm doing that I shouldn't be and redirects
    /// the selection back to the object that I should be transforming as well as reseting the transform I had wrongly adjusted.
    /// 
    /// Now this is a terrible hacky script, but it's saving me all the time at the moment so I'm using it.
    /// Just wondering if anyone has any suggestions to make it a bit nicer though.
    /// 
    /// Thanks,
    /// Pete
    /// 
    /// </summary>

    public GameObject changeSelectionTo;
    [Space(12)]
    public bool lockScale;
    public Vector3 baseScale = Vector3.one;
    public bool lockRotation;
    public Vector3 baseRot;
    public bool lockTranslation;
    public Vector3 basePos;

    [Space(12)]
    public bool lockNonUniformScale;

#if UNITY_EDITOR
    void OnGUI()
    {
        if (Application.isPlaying) return;

        if (lockNonUniformScale && this.transform.localScale.x != this.transform.localScale.y)
        {
            this.transform.localScale = new Vector3(this.transform.localScale.x, this.transform.localScale.x, this.transform.localScale.x);
            SelectOtherGameObject();
        }

        if (lockScale && this.transform.localScale != baseScale)
        {
            this.transform.localScale = baseScale;
            SelectOtherGameObject();
        }

        if (lockRotation && this.transform.localEulerAngles != baseRot)
        {
            this.transform.localEulerAngles = baseRot;
            SelectOtherGameObject();
        }

        if (lockTranslation && this.transform.localPosition != basePos)
        {
            this.transform.localPosition = basePos;
            SelectOtherGameObject();
        }
    }

    void SelectOtherGameObject()
    {
        //Select the object you should be transforming
        if (changeSelectionTo != null)
        {
            UnityEditor.Selection.activeTransform = changeSelectionTo.transform;
        }
    }
#endif
}
