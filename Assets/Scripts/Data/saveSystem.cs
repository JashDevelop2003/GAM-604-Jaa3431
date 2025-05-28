using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

/// <summary>
/// This is the save system that identifies if there is a current game in hand
/// This saves the current player's turn, the board the player is on and if it's still in progress
/// The game stores these as a json file in order for the class to retrieve the game to it's current point
/// The system also allows the player to create a new game and remove any current save file from the game.
/// </summary>
public static class saveSystem
{
    private static string filePath = Application.persistentDataPath + "/gamedata.json";

    //This saves when the data mangager has been called from the turn manager to change the player's turn
    public static void Save(GameData data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(filePath, json);
        Debug.Log("Saved: " + filePath);
    }

    //This checks if there is a GameData to be retrieved to finish off a game
    public static GameData Load() 
    {
        if (File.Exists(filePath)) 
        { 
            string json = File.ReadAllText(filePath);
            GameData data = JsonUtility.FromJson<GameData>(json);
            Debug.Log("Game Found");
            return data;
        }

        else
        {
            Debug.LogError("No File was Found");
            return null;
        }
    }

    //This checks if there is an existing game, if there is then destroy the loaded file.
    public static void NewGame()
    {
        if (File.Exists(filePath)) 
        { 
            File.Delete(filePath);
        }
    }
}
