using UnityEngine;

public class LeverHandleReader : MonoBehaviour
{
    public bool PlayerIsInRange { get; private set; }

    private void Start()
    {
        PlayerIsInRange = false;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag(Tags.PlayerTag))
        {
            PlayerIsInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag(Tags.PlayerTag))
        {
            PlayerIsInRange = false;
        }
    }
}
