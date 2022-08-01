using System.IO;
using UnityEngine;
using UnityEngine.Events;

public static class SaveLoad
{
    public static UnityAction OnSaveGame;
    public static UnityAction<SaveData> OnLoadGame;

    public static string directory = "/Saves/";
    public static string fileName = "SaveData.cfg";

    public static string path;

    public static bool Save(SaveData data)
    {
        OnSaveGame?.Invoke();

        GUIUtility.systemCopyBuffer = Application.persistentDataPath;

        path = Application.persistentDataPath + directory;

        if (!Directory.Exists(path)) Directory.CreateDirectory(path);

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(path + fileName, json);

        Debug.Log("Saving game");

        return true;
    }

    public static SaveData Load()
    {
        SaveData data = new SaveData();

        if (File.Exists(path + fileName))
        {
            string json = File.ReadAllText(path + fileName);
            data = JsonUtility.FromJson<SaveData>(json);

            OnLoadGame?.Invoke(data);

            Debug.Log("Loading save");
        }
        else
        {
            Debug.Log("Save file dosen't exsist");
        }

        return data;
    }

    public static void DeleteSaveData()
    {
        if (File.Exists(path + fileName)) File.Delete(path + fileName);

        Debug.Log("Deleted save");
    }
}
