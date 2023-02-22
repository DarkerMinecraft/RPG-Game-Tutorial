using RPG.Attributes;
using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace RPG.Stats
{
    public class ExperienceDisplay : MonoBehaviour
    {
        private Experience experience;

        private void Awake()
        {
            experience = GameObject.FindWithTag("Player").GetComponent<Experience>();
        }

        private void Update()
        {
            if (experience == null) return;

            GetComponent<TextMeshProUGUI>().text = "" + experience.GetExperiencePoints();
        }
    }
}