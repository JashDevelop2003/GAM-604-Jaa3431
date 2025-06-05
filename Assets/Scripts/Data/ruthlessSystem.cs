using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class ruthlessSystem
{
    private static string ruthlessPath = Application.persistentDataPath + "/ruthlessdata.json";

    public static void Store(RuthlessData retaliating)
    {
        string json = JsonUtility.ToJson(retaliating, true);
        File.WriteAllText(ruthlessPath, json);
    }

    public static RuthlessData Retrieve()
    {
        if (File.Exists(ruthlessPath))
        {
            string json = File.ReadAllText(ruthlessPath);
            RuthlessData ruthlessData = JsonUtility.FromJson<RuthlessData>(json);
            return ruthlessData;
        }
        else
        {
            return null;
        }
    }

    public static void Remove()
    {
        if (!File.Exists(ruthlessPath))
        {
            File.Delete(ruthlessPath);
        }
    }
}
