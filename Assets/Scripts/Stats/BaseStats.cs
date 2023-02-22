using GameDevTV.Utils;
using RPG.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [SerializeField]
        [Range(1, 30)]
        private int startingLevel = 1;

        [SerializeField]
        private CharacterClass characterClass;

        [SerializeField]
        private Progression progression;

        private LazyValue<int> currentLevel;

        private Experience experience;

        public Action OnLevelUp;

        private void Awake()
        {
            currentLevel = new LazyValue<int>(CalculateLevel);
        }

        private void Start()
        {
            experience = GetComponent<Experience>();
            currentLevel.ForceInit();
            

            if(experience != null) 
            {
                experience.OnExperienceGained += UpdateLevel;
            }
        }

        private void UpdateLevel()
        {
            int newLevel = CalculateLevel();

            if (newLevel > currentLevel.value)
            {
                currentLevel.value = newLevel;
                OnLevelUp();
            }
        }

        public float GetStat(Stat stat)
        {
            return (GetBaseStat(stat) + GetAdditiveModifier(stat)) * GetPercentageModifer(stat);
        }

        private float GetBaseStat(Stat stat)
        {
            return progression.GetStat(stat, characterClass, GetLevel());
        }

        private float GetAdditiveModifier(Stat stat) 
        {
            float totalModifer = 0;

            foreach (IModifierProvider provider in GetComponents<IModifierProvider>()) 
            {
                foreach (float modifier in provider.GetAdditiveModifers(stat)) 
                    totalModifer += modifier;
            }    
            return totalModifer;
        }

        private float GetPercentageModifer(Stat stat)
        {
            float totalModifer = 0;

            foreach (IModifierProvider provider in GetComponents<IModifierProvider>())
            {
                foreach (float modifier in provider.GetPercentageModifers(stat))
                    totalModifer += modifier;
            }
            return 1 + (totalModifer / 100f);
        }

        public int GetLevel() 
        {
            if(currentLevel.value < 1)
                CalculateLevel();
            return currentLevel.value; 
        }

        private int CalculateLevel()
        {
            if (experience == null) return startingLevel;

            float currentXP = experience.GetExperiencePoints();

            int penultimateLevel = progression.GetLevels(Stat.ExperienceToLevelUp, characterClass);
            for (int i = 1; i <= penultimateLevel; i++) 
            {
                float XPToLevelUp = progression.GetStat(Stat.ExperienceToLevelUp, characterClass, i);
                if (XPToLevelUp > currentXP)
                    return i;
            }

            return penultimateLevel + 1;
        }
    }
}
