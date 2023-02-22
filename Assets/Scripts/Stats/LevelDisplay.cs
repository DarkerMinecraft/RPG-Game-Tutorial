using RPG.Stats;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Stats
{
    public class LevelDisplay : MonoBehaviour
    {
        private BaseStats baseStat;

        private void Awake()
        {
            baseStat = GameObject.FindWithTag("Player").GetComponent<BaseStats>();
        }

        private void Update()
        {
            if (baseStat == null) return;

            GetComponent<TextMeshProUGUI>().text = "" + baseStat.GetLevel();
        }
    }
}