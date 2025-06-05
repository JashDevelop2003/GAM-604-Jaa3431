using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class luckOutcomeSystem
{
    private static string outcomePath = Application.persistentDataPath + "/luckoutcomedata.json";

    public static void Store(LuckOutcomeData outcome)
    {
        string json = JsonUtility.ToJson(outcome, true);
        File.WriteAllText(outcomePath, json);
    }

    public static LuckOutcomeData Retrieve()
    {
        if (File.Exists(outcomePath))
        {
            string json = File.ReadAllText(outcomePath);
            LuckOutcomeData outcome = JsonUtility.FromJson<LuckOutcomeData>(json);
            return outcome;
        }
        else
        {
            return null;
        }
    }

    public static void Remove()
    {
        if (!File.Exists(outcomePath))
        {
            File.Delete(outcomePath);
        }
    }
}
