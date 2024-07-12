using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodenPanel : MonoBehaviour
{
    //[SerializeField] private GameObject wholePanel;
    [SerializeField] private GameObject leftHalf;
    [SerializeField] private GameObject rightHalf;

    private bool timerEnabled;
    //public void EnableTimer() { timerEnabled = true; }
    //public void DisableTimer() { timerEnabled = false; }

    private float startTime;

    [Header("나무판넬이 부서지기까지 걸리는 시간 (초)")]
    public float durationToBreak = 4f;

    private void Start()
    {
        timerEnabled = false;
        setPanelBroken(false);
    }

    private void Update()
    {
        measureTime();
    }

    private void setPanelBroken(bool isBroken)
    {
        leftHalf.SetActive(isBroken);
        rightHalf.SetActive(isBroken);
        gameObject.SetActive(!isBroken);
    }

    private void measureTime()
    {
        if (timerEnabled)
        {
            if (Time.time - startTime > durationToBreak)
            {
                timerEnabled = false;
                setPanelBroken(true);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            timerEnabled = true;
            startTime = Time.time;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            timerEnabled = false;
        }
    }
}
