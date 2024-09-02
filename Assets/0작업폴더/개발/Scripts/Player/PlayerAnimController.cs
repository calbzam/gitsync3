using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimController : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Vector2 inputDir;

    public int FaceDirX { get; set; }

    private bool _DirInputEnabled = true;
    public void DirInputSetActive(bool enabled) { _DirInputEnabled = enabled; }

    private void Awake()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        FaceDirX = 1;
    }

    private void Update()
    {
        inputDir = CentralInputReader.Input.Player.Movement.ReadValue<Vector2>();

        if (_DirInputEnabled)
        {
            // flip sprite on x direction
            if (inputDir.x > 0)
            {
                spriteRenderer.flipX = false;
                FaceDirX = 1;
            }
            else if (inputDir.x < 0)
            {
                spriteRenderer.flipX = true;
                FaceDirX = -1;
            }
        }
    }
}
