using System;
using UnityEngine;

public class RopeBuoyColPushableByPlayer : MonoBehaviour
{
    public float PushSpeed { get; set; }
    
    private Transform _origParent;

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            if (col.transform.position.y > transform.parent.position.y && FrameInputReader.FrameInput.InputDir.y < 0)
                transform.parent.position += PushSpeed * Time.deltaTime * Vector3.down;
            else if (col.transform.position.y < transform.parent.position.y && FrameInputReader.FrameInput.InputDir.y > 0)
                transform.parent.position += PushSpeed * Time.deltaTime * Vector3.up;

            if (col.transform.position.x > transform.parent.position.x && FrameInputReader.FrameInput.InputDir.x < 0)
                transform.parent.position += PushSpeed * Time.deltaTime * Vector3.left;
            else if (col.transform.position.x < transform.parent.position.x && FrameInputReader.FrameInput.InputDir.x > 0)
                transform.parent.position += PushSpeed * Time.deltaTime * Vector3.right;
        }
    }
}
