using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AnimState = PlayerAnimTools.AnimState;

public class PlayerAnimController : MonoBehaviour
{
    [SerializeField] private PlayerAnimTools _animTools;
    public PlayerAnimTools AnimTools => _animTools;

    public bool AnimLocked { get; set; }

    [SerializeField] private SpriteRenderer _spriteRenderer;
    private Vector2 _inputDir;

    public int FaceDirX { get; set; }

    private void Awake()
    {
        AnimLocked = false;
        FaceDirX = 1;
    }

    private void Update()
    {
        _inputDir = CentralInputReader.Input.Player.Movement.ReadValue<Vector2>();

        EvalSpriteFlipped();
        EvalAnim();
    }

    private void EvalSpriteFlipped()
    {
        if (PlayerLogic.Player.DirInputActive)
        {
            // flip sprite on x direction
            if (_inputDir.x > 0)
            {
                _spriteRenderer.flipX = false;
                FaceDirX = 1;
            }
            else if (_inputDir.x < 0)
            {
                _spriteRenderer.flipX = true;
                FaceDirX = -1;
            }
        }
    }

    private void EvalAnim()
    {
        if (AnimLocked) return;

        if (_inputDir.x != 0)
        {
            if (_animTools.CurrState != AnimState.Walk)
                _animTools.PlayAnimation(AnimState.Walk);
        }
        else
        {
            if (_animTools.CurrState != AnimState.Idle)
                _animTools.PlayAnimation(AnimState.Idle);
        }

        _animTools.CurrState = _animTools.NextState;
    }
}
