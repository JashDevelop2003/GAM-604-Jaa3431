using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class characterSystem
{
    private static string characterPath = Application.persistentDataPath + "/characterdata.json";

    public static void Store(SelectedData selectedData)
    {
        string json = JsonUtility.ToJson(selectedData, true);
        File.WriteAllText(characterPath, json);
        Debug.Log("Stored Character at:" +  json);
    }

    public static SelectedData Retrieve()
    {
        if (File.Exists(characterPath))
        {
            string json = File.ReadAllText(characterPath);
            SelectedData selectedData = JsonUtility.FromJson<SelectedData>(json);
            return selectedData;
        }
        else
        {
            return null;
        }
    }

    public static void Remove()
    {
        if (!File.Exists(characterPath))
        {
            File.Delete(characterPath);
        }
    }
}
