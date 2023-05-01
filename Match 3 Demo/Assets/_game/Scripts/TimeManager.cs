using System;
using _game.Scripts.Board;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using Zenject;

namespace _game.Scripts
{
    public class TimeManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI m_timeText;
        [ShowInInspector, ReadOnly] private float _currentCounter;
        
        private bool isMatchStarted;
        [Inject] private ProjectSettings _projectSettings;

        private void OnEnable()
        {
            EventManager.TimerStart += OnStartTimer;
            EventManager.TimerStop += OnStopTimer;
            EventManager.TimerUpdate += OnTimerUpdate;
        }

        private void OnDisable()
        {
            EventManager.TimerStart -= OnStartTimer;
            EventManager.TimerStop -= OnStopTimer;
            EventManager.TimerUpdate -= OnTimerUpdate;
        }

        private void OnStartTimer()
        {
            isMatchStarted = true;
            _currentCounter = _projectSettings.TimerCountDown;
            m_timeText.SetText(_currentCounter.ToString());
        }

        private void OnStopTimer() =>  isMatchStarted = false;
        private void OnTimerUpdate(float value) => _currentCounter += value;
        
        
        private void Update()
        {
            if (!isMatchStarted) return;
            if (_currentCounter <= 0)
            {
                EventManager.OnTimerStop();
            };
            
            _currentCounter -= Time.deltaTime;
            m_timeText.SetText(Mathf.RoundToInt(_currentCounter).ToString());
            
        }
    }
}
