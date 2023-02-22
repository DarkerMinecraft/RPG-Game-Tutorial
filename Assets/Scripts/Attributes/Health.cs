using RPG.Core;
using RPG.Saving;
using RPG.Stats;
using System.Collections;
using UnityEngine;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour, ISaveable
    {
        
        private float currentHealthPoints = -1;
        private float maxHealthPoints;

        private Animator animator;
        private ActionScheduler actionScheduler;
        private GameObject player;
        private Experience experience;

        private bool isDead = false;

        private BaseStats baseStats;

        private void Start()
        {
            animator = GetComponent<Animator>();
            actionScheduler = GetComponent<ActionScheduler>();
            baseStats = GetComponent<BaseStats>();
            experience = GetComponent<Experience>();

            OnLevelUp();

            if (experience != null)
                baseStats.OnLevelUp += OnLevelUp;

            player = GameObject.FindGameObjectWithTag("Player");
        }

        public void TakeDamage(float damage) 
        {
            currentHealthPoints = Mathf.Max(currentHealthPoints - damage, 0);

            if (currentHealthPoints == 0)
            {
                Die();
                if(player != gameObject) 
                {
                    player.GetComponent<Experience>().GainExperience(baseStats.GetStat(Stat.ExperienceReward));
                }
            }
            
        }

        void OnLevelUp() 
        {
            currentHealthPoints = baseStats.GetStat(Stat.Health);
            maxHealthPoints = currentHealthPoints;
        }

        public int GetPercentage() 
        {
            return Mathf.CeilToInt((currentHealthPoints / maxHealthPoints) * 100);
        }

        public float GetFraction() 
        {
            return currentHealthPoints / maxHealthPoints;
        } 

        void Die() 
        {
            if (isDead) return;

            isDead = true;
            animator.SetTrigger("death");
            actionScheduler.CancelCurrentAction();
        }

        public bool IsDead() { return isDead; }

        public object CaptureState()
        {
            return currentHealthPoints;
        }

        public void RestoreState(object state)
        {
            currentHealthPoints = (float)state;

            if (currentHealthPoints <= 0)
                Die();
        }
    }
}