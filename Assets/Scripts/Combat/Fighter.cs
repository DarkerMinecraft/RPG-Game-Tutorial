using GameDevTV.Utils;
using RPG.Attributes;
using RPG.Core;
using RPG.Movement;
using RPG.Saving;
using RPG.Stats;
using System;
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

        private WeaponConfig currentWeaponConfig;
        private LazyValue<Weapon> currentWeapon;

        private Mover mover;
        private ActionScheduler actionScheduler;
        private Animator animator;
        private BaseStats baseStats;

        void Awake() 
        {
            mover = GetComponent<Mover>();
            actionScheduler = GetComponent<ActionScheduler>();
            animator = GetComponent<Animator>();
            baseStats = GetComponent<BaseStats>();

            currentWeapon = new LazyValue<Weapon>(SetupDefaultWeapon);
        }

        private void Start()
        {
            currentWeapon.ForceInit();
        }

        void Update()
        {
            timeSinceLastAttack += Time.deltaTime;

            if (target == null) return;
            if (target.IsDead()) return;

            if (!GetIsInRange(target.transform))
                mover.MoveTo(target.transform.position);
            else
            {
                mover.Cancel();
                AttackBehaviour();
            }
        }

        private Weapon SetupDefaultWeapon()
        {
            return EquipWeapon(GetWeapon(defaultWeaponName));
        }

        public Weapon EquipWeapon(WeaponConfig weapon) 
        {
            Weapon weap = weapon.Spawn(rightHandTransform, leftHandTransform, animator);
            currentWeaponConfig = weapon;
            currentWeapon.value = weap;
            return weap;
        }

        public WeaponConfig GetWeapon(string weaponName) 
        {
            return Resources.Load<WeaponConfig>(weaponName);
        }

        public Health GetTarget() { return target; }

        private bool GetIsInRange(Transform targetTransform) 
        {
            return Vector3.Distance(transform.position, targetTransform.position) < currentWeaponConfig.GetRange();
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
            if (!mover.CanMoveTo(combatTarget.transform.position)
                && !GetIsInRange(combatTarget.transform)) return false;

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
                yield return currentWeaponConfig.GetDamage();
            }
        }

        public IEnumerable<float> GetPercentageModifers(Stat stat)
        {
            if (stat == Stat.Damage)
            {
                yield return currentWeaponConfig.GetPercentageBonus();
            }
        }

        // Animation Event
        void Hit() 
        {
            if (target == null) return;

            float damage = baseStats.GetStat(Stat.Damage);

            if (currentWeapon.value != null) 
                currentWeapon.value.OnHit();
            

            if (currentWeaponConfig.HasProjectile()) currentWeaponConfig.LaunchProjectile(rightHandTransform, leftHandTransform, target, damage);
            else target.TakeDamage(damage);
        }

        void Shoot() 
        {
            Hit();
        }

        public object CaptureState()
        {
            return currentWeaponConfig.name;
        }

        public void RestoreState(object state)
        {
            animator = GetComponent<Animator>();
            WeaponConfig weapon = Resources.Load<WeaponConfig>((string) state);
            EquipWeapon(weapon);
        }

    }
}
