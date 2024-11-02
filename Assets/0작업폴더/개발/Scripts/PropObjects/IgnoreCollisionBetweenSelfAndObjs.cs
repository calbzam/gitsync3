using UnityEngine;

public class IgnoreCollisionBetweenSelfAndObjs : MonoBehaviour
{
    [SerializeField] private Collider2D[] _collidersToIgnore;
    private Collider2D _selfCollider;

    private void Awake()
    {
        _selfCollider = GetComponent<Collider2D>();
    }

    private void OnEnable()
    {
        foreach (var col in _collidersToIgnore)
        {
            Physics2D.IgnoreCollision(_selfCollider, col);
        }
    }
}
