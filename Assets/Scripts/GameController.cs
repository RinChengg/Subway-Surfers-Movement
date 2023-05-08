using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class GameController : MonoBehaviour
{
    [Header("Timer Setting")]
    [SerializeField] private float _timer = 180f;           // 3 min.
    [SerializeField] private TextMeshProUGUI _timerText;

    // private
    private int _coin = 0;

    // set and get
    public int Coin { get => _coin; set => _coin = value; }
    public float Timer { get => _timer; set => _timer = value; }

    public UnityEvent OnWin;

    private void Update()
    {
        // check win
        if (_timer < 0)
        {
            _timer = 0;
            OnWin?.Invoke();
            return;
        }

        // counting time
        _timer -= Time.deltaTime;
        DisplayTime(_timer);
    }


    private void DisplayTime(float displayTime)
    {
        if (displayTime < 0) displayTime= 0;

        // counting minutes and seconds
        float minutes = Mathf.FloorToInt(displayTime / 60);
        float seconds = Mathf.FloorToInt(displayTime % 60);
        float milliseconds = displayTime % 1 * 1000;
        string timerText = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);

        // update timer text
        _timerText.text = timerText;
    }
}
