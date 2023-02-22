using RPG.Attributes;
using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace RPG.Combat
{
    public class EnemyHealthDisplay : MonoBehaviour
    {
        private Health health;

        private void Awake()
        {
            health = GameObject.FindWithTag("Player").GetComponent<Fighter>().GetTarget();
        }

        private void Update()
        {
            health = GameObject.FindWithTag("Player").GetComponent<Fighter>().GetTarget();

            if (health == null)
                GetComponent<TextMeshProUGUI>().text = "N/A";
            else
                GetComponent<TextMeshProUGUI>().text = String.Format("{0}%", health.GetPercentage());
        }
    }
}