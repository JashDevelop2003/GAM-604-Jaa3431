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
    private static string onePath = Application.persistentDataPath + "/playeronedata.json";
    private static string twoPath = Application.persistentDataPath + "/playertwodata.json";

    //This saves when the data mangager has been called from the turn manager to change the player's turn
    public static void Save(GameData data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(filePath, json);
        Debug.Log("Saved: " + filePath);
    }

    public static void SaveOne(PlayerData data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(onePath, json);
        Debug.Log("Saved: " + onePath);
    }

    public static void SaveTwo(PlayerData data) 
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(twoPath, json);
        Debug.Log("Saved: " + twoPath);
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

    public static PlayerData LoadOne()
    {
        if (File.Exists(onePath))
        {
            string json = File.ReadAllText(onePath);
            PlayerData data = JsonUtility.FromJson<PlayerData>(json);
            Debug.Log("Player Found");
            return data;
        }

        else
        {
            Debug.LogError("No File was Found");
            return null;
        }
    }

    public static PlayerData LoadTwo() 
    {
        if (File.Exists(twoPath))
        {
            string json = File.ReadAllText(twoPath);
            PlayerData data = JsonUtility.FromJson<PlayerData>(json);
            Debug.Log("Player Found");
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
            File.Delete(onePath);
            File.Delete(twoPath);
        }
    }
}
