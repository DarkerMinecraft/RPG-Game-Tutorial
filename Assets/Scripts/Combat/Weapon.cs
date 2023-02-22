﻿using RPG.Attributes;
using RPG.Core;
using System.Collections;
using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "RPG/Make New Weapon", order = 0)]
    public class Weapon : ScriptableObject
    {
        [SerializeField]
        private GameObject equippedPrefab = null;
        [SerializeField]
        private AnimatorOverrideController animationOverride = null;

        [SerializeField]
        private float weaponRange, weaponDamage, percentageBonus;

        [SerializeField]
        private bool isRightHanded = true;

        [SerializeField]
        private Projectile projectile;

        const string weaponName = "Weapon";

        public void Spawn(Transform rightHand, Transform leftHand, Animator animator) 
        {

            DestroyOldWeapon(rightHand, leftHand);

            if (equippedPrefab != null)
            {
                GameObject weaponObj = Instantiate(equippedPrefab, isRightHanded ? rightHand : leftHand);
                weaponObj.name = weaponName;
            }

            var overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;
            if (animationOverride != null)
                animator.runtimeAnimatorController = animationOverride;
            else if(overrideController != null)
                animator.runtimeAnimatorController = overrideController.runtimeAnimatorController;
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