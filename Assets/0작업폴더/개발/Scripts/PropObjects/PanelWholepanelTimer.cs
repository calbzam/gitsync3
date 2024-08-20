using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelWholepanelTimer : MonoBehaviour
{
    public bool TimerEnabled { get; set; }
    public float StartTime { get; private set; }

    private int _objectsOnPanelCnt = 0;

    private void Start()
    {
        TimerEnabled = false;
    }

    private void OnCollisionEnter2D(Collision2D collision) // 나무판넬 자체 collider로도 작용시키기 위해 OnTriggerEnter가 아닌 OnCollisionEnter 사용
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("PushableBox"))
        {
            ++_objectsOnPanelCnt;
            if (_objectsOnPanelCnt == 1)
            {
                TimerEnabled = true;
                StartTime = Time.time;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("PushableBox"))
        {
            --_objectsOnPanelCnt;

            if (_objectsOnPanelCnt == 0) TimerEnabled = false;
        }
    }
}
