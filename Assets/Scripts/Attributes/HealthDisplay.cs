using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace RPG.Attributes
{
    public class HealthDisplay : MonoBehaviour
    {

        private Health health;

        private void Awake()
        {
            health = GameObject.FindWithTag("Player").GetComponent<Health>();
        }

        private void Update()
        {
            if (health == null) return;
            
            GetComponent<TextMeshProUGUI>().text = String.Format("{0}%", health.GetPercentage());
        }

    }
}