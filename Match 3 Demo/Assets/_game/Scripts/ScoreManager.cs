using System;
using _game.Scripts.Storage;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace _game.Scripts
{
    public class ScoreManager : MonoBehaviour
    {
        [ShowInInspector, ReadOnly] private int _currentScore;
        [Inject] private IStorage _storage;
        private ScoreData _scoreData;

        private void OnEnable()
        {
            EventManager.TimerStart += Init;
            EventManager.TimerStop += SetHighScore;
        }
        private void OnDisable()
        {
            EventManager.TimerStart -= Init;
            EventManager.TimerStop -= SetHighScore;
        }

        private void Init()
        {
            _scoreData = _storage.Data.ScoreData;
            _currentScore = 0;
        }
        
        private void SetHighScore()
        {
            if (_currentScore <= _scoreData.HighScore) return;
            
            _scoreData.HighScore = _currentScore;
        }

        public int GetScore()
        {
            return _currentScore;
        }
        public void UpdateScore(int score)
        {
            _currentScore += score;
            EventManager.OnScoreChange(_currentScore);
        }
    }
}
