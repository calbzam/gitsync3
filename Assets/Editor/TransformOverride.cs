#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

// https://forum.unity.com/threads/can-i-see-global-coordinates-in-the-inspector.247453/#post-9135373
[CustomEditor(typeof(Transform))]
class TransformOverride : Editor
{
    public override void OnInspectorGUI()
    {
        Transform transform = (Transform)target;
        base.OnInspectorGUI();

        EditorGUILayout.LabelField("");
        EditorGUILayout.LabelField("World Transform [Read Only]", EditorStyles.boldLabel);
        EditorGUILayout.Vector3Field("World Position", transform.position);
        EditorGUILayout.Vector3Field("World Rotation", transform.eulerAngles);
        EditorGUILayout.Vector3Field("World Scale", transform.localScale);
    }
}
#endif
