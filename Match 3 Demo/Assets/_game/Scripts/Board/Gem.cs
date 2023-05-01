using System;
using System.Collections;
using _game.Scripts.Board;
using DG.Tweening;
using Sirenix.OdinInspector;
using Unity.Mathematics;
using UnityEngine;
using Zenject;

namespace _game.Scripts
{
    public class Gem : MonoBehaviour
    {
        public enum GemType { Blue, Green, Red, Yellow, Purple }
        public GemType Type => _type;

        public bool IsMatched
        {
            get => _isMatched;
            set => _isMatched = value;
        }
        
        [SerializeField] private GemType _type;
        [SerializeField] private ParticleSystem _destroyFX;
      
        [ShowInInspector, ReadOnly] private Vector2Int _posIndex;
        [ShowInInspector, ReadOnly] private bool isTouched;
        [ShowInInspector,ReadOnly] private bool _isMatched;

        private Camera _camera;
        private Vector2 _firstTouchPosition;
        private Vector2 _finalTouchPosition;
        private float _swipeAngle;
        private Gem _otherGem;
        private Tween _moveTween;
        private Vector2Int _cachePosIndex;
        private WaitForSeconds _waitForSeconds = new WaitForSeconds(0.3f);

        [Inject] private BoardManager _boardManager;
        [Inject] private MatchUpManager _matchUpManager;

        private void Awake()
        {
            _camera = Camera.main;
        }

        public void SetPosIndex(Vector2Int pos)
        {
            _posIndex = pos;
        }

        public Vector2Int GetPosIndex()
        {
            return _posIndex;
        }

        private void OnMouseDown()
        {
            if (_boardManager.CurrentState == BoardManager.BoardState.Wait) return;
            _firstTouchPosition = _camera.ScreenToWorldPoint(Input.mousePosition);
            isTouched = true;
        }

        private void OnMouseUp()
        {
            if (_boardManager.CurrentState == BoardManager.BoardState.Wait) return;
            isTouched = false;
            _finalTouchPosition =_camera.ScreenToWorldPoint(Input.mousePosition);
            CalculateAngle();
        }

        private void CalculateAngle()
        {
            _swipeAngle = Mathf.Atan2(_finalTouchPosition.y - _firstTouchPosition.y,
                _finalTouchPosition.x - _firstTouchPosition.x);
            _swipeAngle = _swipeAngle * 180/ Mathf.PI;

            if ((Vector3.Distance(_firstTouchPosition, _finalTouchPosition) > .5f))
            {
                MovePieces();
            }
        }

        private void MovePieces()
        {
            _cachePosIndex = _posIndex;
            if (_swipeAngle < 45 && _swipeAngle > -45 && _posIndex.x < _boardManager.Column - 1)
            {
                _otherGem = _boardManager.GetOtherGem(_posIndex.x + 1, _posIndex.y);
                _otherGem._posIndex.x--;
                _posIndex.x++;
            }
            else if (_swipeAngle > 135 || _swipeAngle < -135 && _posIndex.x > 0)
            {
                _otherGem = _boardManager.GetOtherGem(_posIndex.x -1, _posIndex.y);
                _otherGem._posIndex.x++;
                _posIndex.x--;
            }
            else if (_swipeAngle > 45 && _swipeAngle <= 135 && _posIndex.y < _boardManager.Row-1)
            {
                _otherGem = _boardManager.GetOtherGem(_posIndex.x, _posIndex.y+1);
                _otherGem._posIndex.y--;
                _posIndex.y++;
            }
            else if (_swipeAngle < -45 && _swipeAngle >= -135 && _posIndex.y >0)
            {
                _otherGem = _boardManager.GetOtherGem(_posIndex.x, _posIndex.y-1);
                _otherGem._posIndex.y++;
                _posIndex.y--;
            }
            else
            {
                return;
            }

            _boardManager.SetCurrentState(BoardManager.BoardState.Wait);
            
            Move();
            _otherGem.Move();
            _moveTween.OnComplete((() =>
            {
                _moveTween?.Kill();
                _boardManager.ReOrderIndex(this,_posIndex);
                _boardManager.ReOrderIndex(_otherGem,_otherGem._posIndex);
                _matchUpManager.FindMatches();
                StartCoroutine(CheckMatchCoroutine());
            }));
            
          
        }

        public void Move()
        {
            _moveTween = transform.DOLocalMove(new Vector3(_posIndex.x, _posIndex.y, 0), 0.3f);
        }

        IEnumerator CheckMatchCoroutine()
        {
            yield return _waitForSeconds;
            
            if (IsMatched || _otherGem.IsMatched)
            {
                _boardManager.DestroyMatchedGems();
            }
            else
            {
                MoveBack();
            }
           
        }

        public void PlayDestroyFX()
        {
            Instantiate(_destroyFX, transform.position, Quaternion.identity);
        }

        private void MoveBack()
        {
            _otherGem._posIndex = _posIndex;
            _posIndex = _cachePosIndex;
            
            _boardManager.ReOrderIndex(this,_cachePosIndex);
            _boardManager.ReOrderIndex(_otherGem,_otherGem._posIndex);
            
            Move();
            _otherGem.Move();
            _moveTween.OnComplete((() =>
                _boardManager.SetCurrentState(BoardManager.BoardState.Move)));
        }
        
    }
}
