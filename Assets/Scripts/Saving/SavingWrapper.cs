using System.Collections;
using UnityEngine;

namespace RPG.Saving
{
    public class SavingWrapper : MonoBehaviour
    {
        const string defaultSaveFile = "save";

        private SavingSystem savingSystem;

        private IEnumerator Start()
        {
            savingSystem = GetComponent<SavingSystem>();

            yield return savingSystem.LoadLastScene(defaultSaveFile);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.S))
                Save();
            if (Input.GetKeyDown(KeyCode.L))
                Load();
        }

        public void Save() 
        {
            savingSystem.Save(defaultSaveFile);
        }

        public void Load()
        {
            savingSystem.Load(defaultSaveFile);
        }
    }
}