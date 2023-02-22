using RPG.Attributes;
using RPG.Core;
using RPG.Movement;
using RPG.Saving;
using RPG.Stats;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction, ISaveable, IModifierProvider
    {

        [SerializeField]
        private float timeBetweenAttacks;

        [SerializeField]
        private Transform rightHandTransform, leftHandTransform;

        [SerializeField]
        private string defaultWeaponName = "Unarmed";

        private Health target;
        private float timeSinceLastAttack = Mathf.Infinity;

        private Weapon currentWeapon;

        private Mover mover;
        private ActionScheduler actionScheduler;
        private Animator animator;
        private BaseStats baseStats;

        private void Start()
        {
            mover = GetComponent<Mover>();
            actionScheduler = GetComponent<ActionScheduler>();
            animator = GetComponent<Animator>();
            baseStats = GetComponent<BaseStats>();

            if (currentWeapon == null)
            {
                Weapon weapon = Resources.Load<Weapon>(defaultWeaponName);
                EquipWeapon(weapon);
            }
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

        public void EquipWeapon(Weapon weapon) 
        {
            weapon.Spawn(rightHandTransform, leftHandTransform, animator);
            currentWeapon = weapon;
        }

        public Health GetTarget() { return target; }

        private bool GetIsInRange() 
        {
            return Vector3.Distance(transform.position, target.transform.position) < currentWeapon.GetRange();
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

        public bool CanAttack(GameObject combatTarget) 
        {
            if (combatTarget == null) return false;

            Health healthToTest = combatTarget.GetComponent<Health>();
            return !healthToTest.IsDead();
        }

        public void Attack(GameObject combatTarget)
        {
            target = combatTarget.GetComponent<Health>();
            actionScheduler.StartAction(this);
        }

        public void Cancel() 
        {
            target = null;
            StopAttack();
            mover.Cancel();
        }

        void StopAttack() 
        {
            animator.ResetTrigger("attack");
            animator.SetTrigger("cancelAttack");
        }

        public IEnumerable<float> GetAdditiveModifers(Stat stat)
        {
            if (stat == Stat.Damage) 
            {
                yield return currentWeapon.GetDamage();
            }
        }

        public IEnumerable<float> GetPercentageModifers(Stat stat)
        {
            if (stat == Stat.Damage)
            {
                yield return currentWeapon.GetPercentageBonus();
            }
        }

        // Animation Event
        void Hit() 
        {
            if (target == null) return;

            float damage = baseStats.GetStat(Stat.Damage);

            if (currentWeapon.HasProjectile()) currentWeapon.LaunchProjectile(rightHandTransform, leftHandTransform, target, damage);
            else target.TakeDamage(damage);
        }

        void Shoot() 
        {
            Hit();
        }

        public object CaptureState()
        {
            return currentWeapon.name;
        }

        public void RestoreState(object state)
        {
            animator = GetComponent<Animator>();
            Weapon weapon = Resources.Load<Weapon>((string) state);
            EquipWeapon(weapon);
        }

    }
}
