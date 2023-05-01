using System;
using System.Collections;
using _game.Scripts.Factories;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace _game.Scripts.Board
{
    public partial class BoardManager : MonoBehaviour
    {
        public enum BoardState
        {
            Wait,
            Move,
        }
        public int Row => _projectSettings.RowCount;
        public int Column => _projectSettings.ColumnCount;

        public BoardState CurrentState => _currentState;

        [ShowInInspector,ReadOnly] private BoardState _currentState;
        [SerializeField] private Gem[] m_gems;
        [SerializeField] private float m_height;

        [SerializeField] private Transform m_levelTransform;

        private Gem[,] _gemsCache;
        private WaitForSeconds _waitForSeconds = new WaitForSeconds(0.5f);

        private Transform _tilesParent;
        private Transform _gemsParent;
        [Inject] private TileFactory _tileFactory;
        [Inject] private GemFactory _gemFactory;
        [Inject] private MatchUpManager _matchUpManager;
        [Inject] private ScoreManager _scoreManager;
        [Inject] private ProjectSettings _projectSettings;

        private void OnEnable()
        {
            EventManager.MatchStart += SpawnBoard;
            EventManager.TimerStop += OnTimerStop;
            EventManager.ConnectionLost += DestroyBoard;
        }

        private void OnDisable()
        {
            EventManager.MatchStart -= SpawnBoard;
            EventManager.TimerStop -= OnTimerStop;
            EventManager.ConnectionLost -= DestroyBoard;
        }

        private void Awake()
        {
            _gemsCache = new Gem[Row, Column];
            SetCurrentState(BoardState.Wait);
        }

        private void OnTimerStop()
        {
            SetCurrentState(BoardState.Wait);
            StopAllCoroutines();
        }

        public void SetCurrentState(BoardState state)
        {
            _currentState = state;
        }

        public Gem GetOtherGem(int posX,int posY)
        {
            return _gemsCache[posX, posY];
        }

        public void ReOrderIndex(Gem targetGem,Vector2Int pos)
        {
            _gemsCache[pos.x, pos.y] = targetGem;
        }
        
        public void DestroyMatchedGems()
        {
            var matches = _matchUpManager.CurrentMatches.Count;
            foreach (var matchedGem in _matchUpManager.CurrentMatches)
            {
                var posIndex = matchedGem.GetPosIndex();
                _gemsCache[posIndex.x, posIndex.y] = null;
                matchedGem.PlayDestroyFX();
                Destroy(matchedGem.gameObject);
            }
            _scoreManager.UpdateScore(matches);
            _matchUpManager.CurrentMatches.Clear();
            
            StartCoroutine(FallGemCoroutine());
        }
        
        

    }
}
