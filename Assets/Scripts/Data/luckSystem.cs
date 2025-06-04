using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class luckSystem : MonoBehaviour
{
    private static string luckPath = Application.persistentDataPath + "/luckdata.json";

    public static void Store(LuckData luck)
    {
        string json = JsonUtility.ToJson(luck, true);
        File.WriteAllText(luckPath, json);
        Debug.Log("Stored Luck Mana at:" + json);
    }

    public static LuckData Retrieve()
    {
        if (File.Exists(luckPath))
        {
            string json = File.ReadAllText(luckPath);
            LuckData luckData = JsonUtility.FromJson<LuckData>(json);
            return luckData;
        }
        else
        {
            return null;
        }
    }

    public static void Remove()
    {
        if (!File.Exists(luckPath))
        {
            File.Delete(luckPath);
        }
    }
}
