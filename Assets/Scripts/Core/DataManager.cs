using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.ComponentModel;

namespace Assets.Scripts.Core
{
    public class DataManager : MonoBehaviour
    {
        public void SaveData(string name = "save")
        {
            Debug.Log("Saving...");
            string filePath = @"C:\Users\Public\Documents\" + name + "data";
            //if (!File.Exists(filePath)) return;

            Dictionary<string, object> gameData = new Dictionary<string, object>
            {
                { "Player",     GameManager._instance.Player.GetData() },
                { "Inverntory", GameManager._instance.inventoryManager.GetData() }
            };

            using (var fileStream = File.Create(filePath))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(fileStream, gameData);
            }
            Debug.Log("Saving... "+filePath);

        }

        public void LoadSavedData(string filePath = @"C:\Users\Public\Documents", string name = "savedata")
        {
            Debug.Log("Loading...");
            string file = Path.Combine(filePath, name);
            Debug.Log(file);
            //if (!File.Exists(filePath))
            //{
            //    Debug.Log($"File({file}) was not found !!");
            //    return;
            //} 
            Dictionary<string, object> gameData = new Dictionary<string, object>();
            using (var stream = File.Open(file, FileMode.Open))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                gameData = formatter.Deserialize(stream) as Dictionary<string, object>;
            }
            GameManager._instance.Player.LoadData((Dictionary<string, int>)gameData["Player"]);
            GameManager._instance.inventoryManager.LoadData((Dictionary<string, int>)gameData["Inverntory"]);
        }
    }
}
