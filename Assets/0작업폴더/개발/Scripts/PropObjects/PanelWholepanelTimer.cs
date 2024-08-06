using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelWholepanelTimer : MonoBehaviour
{
    public bool TimerEnabled { get; set; }
    public float StartTime { get; private set; }

    private void Start()
    {
        TimerEnabled = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            TimerEnabled = true;
            StartTime = Time.time;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            TimerEnabled = false;
        }
    }
}
