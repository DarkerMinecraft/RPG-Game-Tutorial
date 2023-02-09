using System.Collections;
using UnityEngine;

namespace RPG.Core
{
    public class Health : MonoBehaviour
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
            healthPoints -= damage;
            healthPoints = Mathf.Clamp(healthPoints, 0, healthPoints);

            if(healthPoints == 0 && !isDead) 
            {
                Die();
            }
        }

        void Die() 
        {
            if (isDead) return;

            isDead = true;
            animator.SetTrigger("death");
            actionScheduler.CancelCurrentAction();
        }

        public bool IsDead() { return isDead; }
    }
}