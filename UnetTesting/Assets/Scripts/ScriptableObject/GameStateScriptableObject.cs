using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LocalGameState", menuName = "PlayerData/GameState", order = 1)]
public class GameStateScriptableObject : ScriptableObject
{
    public int TotalLevel = 3;
    
    public List<Level> AllLevel = new List<Level>();

    /// <summary>
    /// Helps give the player the base info they will need
    /// </summary>
    /// <param name="player"></param>
    public void Initiate(Player player)
    {
        AllLevel = new List<Level>();
        for (int i = 0; i < TotalLevel; i++)
        {
            //Add the new level to the gameStat
            AllLevel.Add(new Level(i + 1));

            //Give the player the level 
            player.allLevel.Add("Stage" + (i + 1), AllLevel[i]);
        }
    }
}
