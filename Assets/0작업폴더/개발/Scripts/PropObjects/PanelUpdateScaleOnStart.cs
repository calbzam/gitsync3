using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PanelUpdateScaleOnStart : MonoBehaviour
{
    [Header("설명:\nScale이 1:1:1로 수정된 오브젝트의 children은\nskew된 상태가 되어 Rotation 수정이 제대로 동작하지 않음." +
        "\n\n따라서 Start()이 실행되었을 때 children을\n본인의 transform에서 unparent 한 후" +
        "\n다시 parent 시켜 줌으로써\n본인의 Scale을 children의 것으로 적용한 후,\n\n본인의 Scale은 1롤 초기화")]
    [SerializeField] bool _;

    Transform[] _children;

    private void Start()
    {
        _children = gameObject.GetComponentsInChildren<Transform>(true).Skip(1).ToArray();
        CheckLocalScaleChanged();
    }

    private void CheckLocalScaleChanged()
    {
        if (transform.localScale != Vector3.one)
        {
            for (int i = 0; i < _children.Length; ++i)
            {
                _children[i].SetParent(null);
            }

            transform.localScale = Vector3.one;

            for (int i = 0; i < _children.Length; ++i)
            {
                _children[i].SetParent(transform);
            }
        }
    }
}
