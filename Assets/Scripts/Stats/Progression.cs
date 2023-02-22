using RPG.Core;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "Progression", menuName = "RPG/Create Progression", order = 1)]
    public class Progression : ScriptableObject
    {

        [SerializeField]
        private ProgressionCharacterClass[] characterClasses;

        private Dictionary<CharacterClass, Dictionary<Stat, float[]>> lookupTable;

        [System.Serializable]
        class ProgressionCharacterClass 
        {
            public CharacterClass characterClass;
            public ProgressionStat[] stats;
        }

        [System.Serializable]
        class ProgressionStat 
        {
            public Stat stat;
            public float[] levels;
        }

        private void BuildLookup() 
        {
            lookupTable = new Dictionary<CharacterClass, Dictionary<Stat, float[]>>();
            
            for (int i = 0; i < characterClasses.Length; i++) 
            {
                Dictionary<Stat, float[]> statDict = new Dictionary<Stat, float[]>();
                for (int j = 0; j < characterClasses[i].stats.Length; j++) 
                {
                    ProgressionStat progStat = characterClasses[i].stats[j];
                    statDict.Add(progStat.stat, progStat.levels);
                }

                lookupTable.Add(characterClasses[i].characterClass, statDict);
            }
        }

        public float GetStat(Stat stat, CharacterClass characterClass, int level) 
        {
            if (lookupTable == null)
                BuildLookup();

            float[] values = lookupTable[characterClass][stat];

            return values[level - 1]; 
        }

        public int GetLevels(Stat stat, CharacterClass characterClass)
        {
            if (lookupTable == null)
                BuildLookup();

            return lookupTable[characterClass][stat].Length;
        }

    }
}