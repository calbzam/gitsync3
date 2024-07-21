using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricWaterTrigger : MonoBehaviour
{
    private float _respawnDelayTimer;
    private bool _timerIsActive = false;
    [SerializeField] private float _respawnDelay = 1f;

    private void Start()
    {
        _timerIsActive = false;
    }

    private void Update()
    {
        RespawnDelayTimer();
    }

    private void RespawnDelayTimer()
    {
        if (_timerIsActive)
        {
            if (Time.time - _respawnDelayTimer > _respawnDelay)
            {
                PlayerLogic.FreePlayerPosition();
                PlayerLogic.Player.RespawnPlayer();
                PlayerLogic.PlayerElectrocutedText.gameObject.SetActive(false);
                _timerIsActive = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerLogic.LockPlayerPosition();
            PlayerLogic.PlayerElectrocutedText.gameObject.SetActive(true);
            _respawnDelayTimer = Time.time;
            _timerIsActive = true;
        }
    }
}
