using RPG.Attributes;
using RPG.Core;
using System.Collections;
using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "RPG/Make New Weapon", order = 0)]
    public class WeaponConfig : ScriptableObject
    {
        [SerializeField]
        private Weapon equippedPrefab = null;
        [SerializeField]
        private AnimatorOverrideController animationOverride = null;

        [SerializeField]
        private float weaponRange, weaponDamage, percentageBonus;

        [SerializeField]
        private bool isRightHanded = true;

        [SerializeField]
        private Projectile projectile;

        const string weaponName = "Weapon";

        public Weapon Spawn(Transform rightHand, Transform leftHand, Animator animator) 
        {

            DestroyOldWeapon(rightHand, leftHand);

            Weapon weapon = null;

            if (equippedPrefab != null)
            {
                weapon = Instantiate(equippedPrefab, isRightHanded ? rightHand : leftHand);
                weapon.gameObject.name = weaponName;
            }

            var overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;
            if (animationOverride != null)
                animator.runtimeAnimatorController = animationOverride;
            else if(overrideController != null)
                animator.runtimeAnimatorController = overrideController.runtimeAnimatorController;

            return weapon;
        }

        void DestroyOldWeapon(Transform rightHand, Transform leftHand) 
        {
            Transform oldWeapon = rightHand.Find(weaponName) != null ? 
                rightHand.Find(weaponName) : leftHand.Find(weaponName);

            if (oldWeapon == null) return;
            oldWeapon.name = "DESTROYING";  
            Destroy(oldWeapon.gameObject);
        } 

        public float GetRange() { return weaponRange; }

        public float GetDamage() { return weaponDamage; }

        public bool HasProjectile() { return projectile != null; }

        public float GetPercentageBonus() { return percentageBonus; }

        public void LaunchProjectile(Transform rightHand, Transform leftHand, Health target, float calculatedDamage)
        {
            Projectile projectileInstance = Instantiate(projectile,
                (isRightHanded ? rightHand : leftHand).position, Quaternion.identity);
            projectileInstance.SetTarget(target, calculatedDamage);
        }
    }
}