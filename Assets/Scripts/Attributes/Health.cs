using GameDevTV.Utils;
using RPG.Core;
using RPG.Saving;
using RPG.Stats;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour, ISaveable
    {

        private LazyValue<float> currentHealthPoints;
        private float maxHealthPoints;

        private Animator animator;
        private ActionScheduler actionScheduler;
        private GameObject player;
        private Experience experience;

        private bool isDead = false;

        private BaseStats baseStats;

        [SerializeField]
        private TakeDamageEvent takeDamage;

        [SerializeField]
        private UnityEvent onDie;

        [System.Serializable]
        class TakeDamageEvent : UnityEvent<float> { }


        private void Start()
        {
            animator = GetComponent<Animator>();
            actionScheduler = GetComponent<ActionScheduler>();
            baseStats = GetComponent<BaseStats>();
            experience = GetComponent<Experience>();

            if (experience != null)
                baseStats.OnLevelUp += OnLevelUp;

            player = GameObject.FindGameObjectWithTag("Player");

            currentHealthPoints = new LazyValue<float>(GetCurrentHealthPoints);
            maxHealthPoints = baseStats.GetStat(Stat.Health);
        }

        public void TakeDamage(float damage) 
        {
            currentHealthPoints.value = Mathf.Max(currentHealthPoints.value - damage, 0);
            if (currentHealthPoints.value == 0)
            {
                onDie.Invoke();
                Die();
                if(player != gameObject) 
                {
                    player.GetComponent<Experience>().GainExperience(baseStats.GetStat(Stat.ExperienceReward));
                }
            } else takeDamage.Invoke(damage);
            
        }

        void OnLevelUp() 
        {
            currentHealthPoints.value = GetCurrentHealthPoints();
            maxHealthPoints = currentHealthPoints.value;
        }

        float GetCurrentHealthPoints() 
        {
            return baseStats.GetStat(Stat.Health);
        }

        public int GetPercentage() 
        {
            return Mathf.CeilToInt((currentHealthPoints.value / maxHealthPoints) * 100);
        }

        public float GetFraction() 
        {
            return currentHealthPoints.value / maxHealthPoints;
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
            return currentHealthPoints.value;
        }

        public void RestoreState(object state)
        {
            currentHealthPoints.value = (float) state;

            if (currentHealthPoints.value <= 0)
                Die();
        }
    }
}