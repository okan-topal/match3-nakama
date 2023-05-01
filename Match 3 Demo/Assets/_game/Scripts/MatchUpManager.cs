using System;
using System.Collections;
using System.Collections.Generic;
using _game.Scripts;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;
using BoardManager = _game.Scripts.Board.BoardManager;

public class MatchUpManager : MonoBehaviour
{
    public List<Gem> CurrentMatches => _currentMatches;
    [ShowInInspector, ReadOnly] private List<Gem> _currentMatches = new List<Gem>();
    [Inject] private BoardManager _boardManager;

    public void FindMatches()
    {
        _currentMatches.Clear();
        
        for (var i = 0; i < _boardManager.Row; i++)
        {
            for (var j = 0; j < _boardManager.Column; j++)
            {
                var currentGem = _boardManager.GetOtherGem(i, j);
                if(currentGem == null) continue;

                if (i > 0 && i < _boardManager.Column - 1)
                {
                    var leftGem = _boardManager.GetOtherGem(i - 1, j);
                    var rightGem = _boardManager.GetOtherGem(i + 1, j);

                    if (leftGem != null && rightGem != null)
                    {
                        if (currentGem.Type == leftGem.Type && currentGem.Type == rightGem.Type)
                        {
                            CatchMatch(currentGem, leftGem, rightGem);
                            
                        }
                    }
                } 
                if (j > 0 && j< _boardManager.Row - 1)
                {
                    var aboveGem = _boardManager.GetOtherGem(i, j+1);
                    var belowGem = _boardManager.GetOtherGem(i, j-1);

                    if (aboveGem != null && belowGem != null)
                    {
                        if (currentGem.Type == aboveGem.Type && currentGem.Type == belowGem.Type)
                        {
                            CatchMatch(currentGem, aboveGem, belowGem);
                        }
                    }
                } 
            }
        }
    }

    private void CatchMatch(Gem currentGem,Gem otherGem1,Gem otherGem2)
    {
        currentGem.IsMatched = true;
        otherGem1.IsMatched = true;
        otherGem2.IsMatched = true;
        
        _currentMatches.Add(currentGem);
        _currentMatches.Add(otherGem1);
        _currentMatches.Add(otherGem2);
    }
    
    
}
