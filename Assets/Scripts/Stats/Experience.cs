using RPG.Saving;
using System;
using System.Collections;
using UnityEngine;

namespace RPG.Stats
{
    public class Experience : MonoBehaviour, ISaveable
    {
        [SerializeField]
        private float experiencePoints;

        public event Action OnExperienceGained;

        public void GainExperience(float experience) 
        {
            experiencePoints += experience;
            OnExperienceGained();
        }

        public float GetExperiencePoints() { return experiencePoints; }

        public object CaptureState()
        {
            return experiencePoints;
        }

        public void RestoreState(object state)
        {
            experiencePoints = (float) state;
        }
    }
}