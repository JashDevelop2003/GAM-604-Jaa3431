using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class stanceSystem
{
    private static string stancePath = Application.persistentDataPath + "/stancedata.json";

    public static void Store(StanceData stance)
    {
        string json = JsonUtility.ToJson(stance, true);
        File.WriteAllText(stancePath, json);
    }

    public static StanceData Retrieve()
    {
        if (File.Exists(stancePath))
        {
            string json = File.ReadAllText(stancePath);
            StanceData stanceData = JsonUtility.FromJson<StanceData>(json);
            return stanceData;
        }
        else
        {
            return null;
        }
    }

    public static void Remove()
    {
        if (!File.Exists(stancePath))
        {
            File.Delete(stancePath);
        }
    }
}
