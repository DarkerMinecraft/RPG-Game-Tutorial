using RPG.Core;
using RPG.Movement;
using UnityEngine;
using UnityEngine.UIElements;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {

        [SerializeField]
        private float weaponRange, weaponDamage, timeBetweenAttacks;

        private Health target;
        private float timeSinceLastAttack; 

        private Mover mover;
        private ActionScheduler actionScheduler;
        private Animator animator;

        private void Start()
        {
            mover = GetComponent<Mover>();
            actionScheduler = GetComponent<ActionScheduler>();
            animator = GetComponent<Animator>();

            timeSinceLastAttack = 0;
        }

        void Update()
        {
            timeSinceLastAttack += Time.deltaTime;

            if (target == null) return;
            if (target.IsDead()) return;

            if (!GetIsInRange())
                mover.MoveTo(target.transform.position);
            else
            {
                mover.Cancel();
                AttackBehaviour();
            }
        }

        private bool GetIsInRange() 
        {
            return Vector3.Distance(transform.position, target.transform.position) < weaponRange;
        }

        private void AttackBehaviour() 
        {
            transform.LookAt(target.transform);
            if (timeSinceLastAttack >= timeBetweenAttacks)
            {
                TriggerAttack();
                timeSinceLastAttack = 0;
            }
        }

        private void TriggerAttack() 
        {
            animator.ResetTrigger("cancelAttack");
            animator.SetTrigger("attack");
        }

        public bool CanAttack(CombatTarget combatTarget) 
        {
            if (combatTarget == null) return false;

            Health healthToTest = combatTarget.GetComponent<Health>();
            return !healthToTest.IsDead();
        }

        public void Attack(CombatTarget combatTarget)
        {
            target = combatTarget.GetComponent<Health>();
            actionScheduler.StartAction(this);
        }

        public void Cancel() 
        {
            target = null;
            StopAttack();
        }

        void StopAttack() 
        {
            animator.ResetTrigger("attack");
            animator.SetTrigger("cancelAttack");
        }

        // Animation Event
        void Hit() 
        {
            if (target == null) return;

            target.TakeDamage(weaponDamage);
        }
    }
}
