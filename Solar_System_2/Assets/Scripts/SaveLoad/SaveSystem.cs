using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem 
{
    public const string SavesFolder = "C:/Users/BSNL/Desktop/SolarSystemSaves";

    public static void CreateFolder(string FolderName){
        Directory.CreateDirectory(SavesFolder + "/" + FolderName);
    }

    public static void SaveAsset<T>(T SaveData){

    }
}
