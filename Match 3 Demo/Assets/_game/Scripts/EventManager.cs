using UnityEngine.Events;

namespace _game.Scripts
{
    public static class EventManager
    {
        public static event UnityAction MatchStart;
        public static event UnityAction TimerStart;
        public static event UnityAction TimerStop;
        public static event UnityAction<float> TimerUpdate;
        public static event UnityAction<int> ScoreChange;
        public static event UnityAction MatchEnd;
        public static event UnityAction ConnectionLost;
    
        public static void OnMatchStart() => MatchStart?.Invoke();
        public static void OnTimerStart() => TimerStart?.Invoke();
        public static void OnTimerStop() => TimerStop?.Invoke();
        public static void OnTimerUpdate(float value) => TimerUpdate?.Invoke(value);
        public static void OnScoreChange(int value) => ScoreChange?.Invoke(value);
    
        public static void OnMatchEnd() => MatchEnd?.Invoke();
        public static void OnConnectionLost() => ConnectionLost?.Invoke();
   
 
    }
}
