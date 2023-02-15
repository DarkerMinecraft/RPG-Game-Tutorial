using RPG.Saving;
using System.Collections;
using UnityEngine;

namespace RPG.Core
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField]
        private float healthPoints;

        private Animator animator;
        private ActionScheduler actionScheduler;

        private bool isDead = false;

        private void Start()
        {
            animator = GetComponent<Animator>();
            actionScheduler = GetComponent<ActionScheduler>();
        }

        public void TakeDamage(float damage) 
        {
            healthPoints = Mathf.Max(healthPoints - damage, 0);

            if(healthPoints == 0) 
                Die();
            
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
            return healthPoints;
        }

        public void RestoreState(object state)
        {
            healthPoints = (float)state;

            if (healthPoints <= 0)
                Die();
        }
    }
}