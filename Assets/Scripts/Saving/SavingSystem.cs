using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace RPG.Saving
{
    public class SavingSystem : MonoBehaviour
    {
       
        public void Save(string saveFile) 
        {
            string path = GetPathFromSaveFile(saveFile);
            print("Saving to " + path);

            using (FileStream fs = File.Open(path, FileMode.Create))
            {
                Transform player = GameObject.FindGameObjectWithTag("Player").transform;
                byte[] buffer = SerizalizeVector(player.position);
                fs.Write(buffer, 0, buffer.Length);
            }
        }

        public void Load(string saveFile)
        {
            string path = GetPathFromSaveFile(saveFile);
            print("Loading from " + path);

            using (FileStream fs = File.Open(path, FileMode.Open))
            {
                byte[] buffer = new byte[fs.Length];
                fs.Read(buffer, 0, buffer.Length);
                Vector3 position = DeserizalizeVector(buffer);

                GameObject.FindGameObjectWithTag("Player").transform.position = position;
            }
        }

        private byte[] SerizalizeVector(Vector3 vector) 
        {
            byte[] vectorBytes = new byte[4 * 3];
            BitConverter.GetBytes(vector.x).CopyTo(vectorBytes, 0);
            BitConverter.GetBytes(vector.y).CopyTo(vectorBytes, 4);
            BitConverter.GetBytes(vector.z).CopyTo(vectorBytes, 8);    
            return vectorBytes;
        }

        private Vector3 DeserizalizeVector(byte[] buffer) 
        {
            Vector3 vector = new Vector3();
            vector.x = BitConverter.ToSingle(buffer, 0);
            vector.y = BitConverter.ToSingle(buffer, 4);
            vector.z = BitConverter.ToSingle(buffer, 8);
            return vector;
        }

        private string GetPathFromSaveFile(string saveFile) 
        {
            return Path.Combine(Application.persistentDataPath, saveFile + ".dat");
        }
    }
}
