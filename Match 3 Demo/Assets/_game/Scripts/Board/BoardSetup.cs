using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace _game.Scripts.Board
{
    public partial class BoardManager
    {
        private void SpawnBoard()
        {
            //   Random.InitState((int)DateTime.Now.Ticks);
            _tilesParent = new GameObject("TilesParent").transform;
            _gemsParent = new GameObject("GemsParent").transform;
            for (int i = 0; i < Row; i++)
            {
                for (int j = 0; j < Column; j++)
                {
                    SpawnTile(new Vector2Int(i,j));
                    var gemType = FindUniqueColorGem(new Vector2Int(i, j));
                    SpawnGem(new Vector2Int(i,j),_projectSettings.Gems[gemType]);
                }
            }
            AnimateBoard();
        }
        
        private void SpawnTile(Vector2Int pos)
        {
            var tile = _tileFactory.Create(_projectSettings.Tile);
            tile.transform.SetParent(_tilesParent);
            tile.transform.localPosition = new Vector2(pos.x, pos.y);
            tile.transform.localRotation = Quaternion.identity;
            tile.name = $"Tile{pos.x}.{pos.y}";
        }

        private int FindUniqueColorGem(Vector2Int pos)
        {
            var gemIndex = Random.Range(0, m_gems.Length);
            while (MatchesAt(new Vector2Int(pos.x,pos.y),m_gems[gemIndex]))
            {
                gemIndex = Random.Range(0, m_gems.Length);
            }

            return gemIndex;
        }

        private void SpawnGem(Vector2Int pos, Gem gemToSpawn)
        {
            var gem = _gemFactory.Create(gemToSpawn);
            gem.transform.SetParent(_gemsParent);
            gem.transform.localPosition = new Vector2(pos.x, pos.y+m_height);
            gem.transform.localRotation = Quaternion.identity;
            
            gem.name = $"Gem{pos.x}.{pos.y}";
            _gemsCache[pos.x, pos.y] = gem;
            gem.SetPosIndex(pos);
            gem.Move();
        }
        
        private IEnumerator FallGemCoroutine()
        {
            yield return _waitForSeconds;
          
            for (var i = 0; i < Row; i++)
            {
                var nullCounter = 0;
               
                for (var j = 0; j < Column; j++)
                {
                    if (_gemsCache[i, j] == null)
                    {
                        nullCounter++;
                    }
                    else if (nullCounter > 0)
                    {
                        var gem = _gemsCache[i, j];
                        var currentPos = gem.GetPosIndex();
                        gem.SetPosIndex(new Vector2Int(currentPos.x,currentPos.y-nullCounter));

                        _gemsCache[i, j - nullCounter] = _gemsCache[i, j];
                        _gemsCache[i, j] = null;
                        gem.Move();
                    }
                }
            }
            yield return _waitForSeconds;
            ReFillBoard();
            yield return _waitForSeconds;
            _matchUpManager.FindMatches();
            yield return _waitForSeconds;
            if (_matchUpManager.CurrentMatches.Count > 0)
            {
                DestroyMatchedGems();
            }
            else
            {
                SetCurrentState(BoardState.Move);
            }
           
        }
        
        private void ReFillBoard()
        {
            for (int i = 0; i < Row; i++)
            {
                for (int j = 0; j < Column; j++)
                {
                    if(_gemsCache[i,j] != null) continue;
                    var gemIndex = Random.Range(0, m_gems.Length);
                    SpawnGem(new Vector2Int(i,j),m_gems[gemIndex]);
                }
            }
        }

        private bool MatchesAt(Vector2Int postToCheck, Gem gemToCheck)
        {
            if (postToCheck.x > 1)
            {
                if (_gemsCache[postToCheck.x - 1, postToCheck.y].Type == gemToCheck.Type &&
                    _gemsCache[postToCheck.x - 2, postToCheck.y].Type == gemToCheck.Type)
                {
                    return true;
                }
            }
            if (postToCheck.y > 1)
            {
                if (_gemsCache[postToCheck.x , postToCheck.y-1].Type == gemToCheck.Type &&
                    _gemsCache[postToCheck.x , postToCheck.y-2].Type == gemToCheck.Type)
                {
                    return true;
                }
            }
            return false;
        }
        
        private void AnimateBoard()
        {
            m_levelTransform.DOMove(Vector3.zero, 1f).SetDelay(1).OnComplete(() => 
                {
                    SetCurrentState(BoardState.Move);
                }
               
            );
        }

        private void DestroyBoard()
        {
            Destroy(_tilesParent.gameObject);
            Destroy(_gemsParent.gameObject);
        }
        
        
    }
}
