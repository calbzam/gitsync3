using UnityEngine;

public class PlayerAnimTools : MonoBehaviour
{
    [SerializeField] private Animator _anim;

    [Header("Unity Editor 내에서 만지지 말 것 (스크립트로 컨트롤됨)")]
    public AnimState CurrState; // current state
    public AnimState NextState;

    // for animation crossfade evaluation
    private static readonly int Idle = Animator.StringToHash("2D-idle-temp");
    private static readonly int Walk = Animator.StringToHash("2D-walking-temp");

    // for animation logic evaluation
    public enum AnimState
    {
        None = 0,

        Idle,
        Walk,
        Electrocuted,
    }

    private void Start()
    {
        CurrState = AnimState.Idle;
        NextState = AnimState.None;
    }

    public bool IsInTransition()
    {
        return _anim.IsInTransition(0);
    }

    public int GetAnimHash(AnimState animState)
    {
        switch (animState)
        {
            case AnimState.Idle:
                return Idle;

            case AnimState.Walk:
                return Walk;

            case AnimState.Electrocuted:
                return Idle;

            default:
                return Idle;
        }
    }

    public float GetAnimTransitionDur(AnimState animState)
    {
        switch (animState)
        {
            default:
                return 0.3f;
        }
    }

    public void PlayAnimation(AnimState toAnimState, bool interruptPrev = false)
    {
        PlayAnimation(toAnimState, GetAnimTransitionDur(toAnimState), interruptPrev);
    }

    public void PlayAnimation(AnimState toAnimState, float fadeDuration, bool interruptPrev = false)
    {
        if (interruptPrev) fadeDuration = 0.1f; // interrupt previous anim and play immediately

        if (interruptPrev || !IsInTransition())
        {
            _anim.SetInteger("AnimState", (int)toAnimState); // for debugging purposes in the Unity Animator window
            _anim.CrossFade(GetAnimHash(toAnimState), fadeDuration);
            NextState = toAnimState;
        }
    }
}