using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;  //formattatore binario
using UnityEngine.UI;

public static class DataSystem
{
    public static void SaveData()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/pescepalla.data"; //persistentDataPath: directory nel SO che non cambia
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, CurrentData.day);

        stream.Close();
    }

    public static void LoadData()
    {
        string path = Application.persistentDataPath + "/pescepalla.data";

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            CurrentData.day = (int)formatter.Deserialize(stream);
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
        }

        Debug.Log(CurrentData.day);
    }
}
