
using System.Collections.Generic;
using _game.Scripts.Storage;
using Nakama.TinyJson;
using UnityEngine;

public static class MatchDataJson
{   
    
    public static string Score(int score)
    {
        var values = new Dictionary<string, int>
        {
            { "score",score },
        };

        return values.ToJson();
    }
    
    public static string Name(string name)
    {
        var values = new Dictionary<string, string>
        {
            { "name", name },
        };

        return values.ToJson();
    }
    
    public static string Gender(string gender)
    {
        var values = new Dictionary<string, string>
        {
            { "gender", gender },
        };

        return values.ToJson();
    }
    
    
 
}
