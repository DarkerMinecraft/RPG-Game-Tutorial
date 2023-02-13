using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace RPG.Saving
{
    public class SavingSystem : MonoBehaviour
    {
       
        public void Save(string saveFile) 
        {
            Dictionary<string, object> state = LoadFile(saveFile);
            CaptureState(state);
            SaveFile(saveFile, state);
        }

        public void Load(string saveFile)
        {
            RestoreState(LoadFile(saveFile));
        }

        private void CaptureState(Dictionary<string, object> state) 
        {
            foreach(SaveableEntity saveable in FindObjectsOfType<SaveableEntity>()) 
            {
                state[saveable.GetUniqueUdentifier()] = saveable.CaptureState();
            }
        }

        private void RestoreState(Dictionary<string, object> stateDict) 
        {
            foreach (SaveableEntity saveable in FindObjectsOfType<SaveableEntity>())
            {
                string uuid = saveable.GetUniqueUdentifier();
                if(stateDict.ContainsKey(uuid))
                    saveable.RestoreState(stateDict[uuid]);
            }
        }

        void SaveFile(string saveFile, object obj) 
        {
            string path = GetPathFromSaveFile(saveFile);

            using (FileStream fs = File.Open(path, FileMode.Create))
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(fs, obj);
            }
        }

        Dictionary<string, object> LoadFile(string saveFile) 
        {
            string path = GetPathFromSaveFile(saveFile);

            if (!File.Exists(path)) 
                return new Dictionary<string, object>();

            using (FileStream fs = File.Open(path, FileMode.Open)) 
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                return (Dictionary<string, object>) binaryFormatter.Deserialize(fs);
            }
        }

        private string GetPathFromSaveFile(string saveFile) 
        {
            return Path.Combine(Application.persistentDataPath, saveFile + ".dat");
        }
    }
}
