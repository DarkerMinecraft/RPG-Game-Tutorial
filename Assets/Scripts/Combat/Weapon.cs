using System.Collections;
using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon", order = 0)]
    public class Weapon : ScriptableObject
    {
        [SerializeField]
        private GameObject equippedPrefab = null;
        [SerializeField]
        private AnimatorOverrideController animationOverride = null;

        [SerializeField]
        private float weaponRange, weaponDamage;

        public void Spawn(Transform handTransform, Animator animator) 
        {
            if (equippedPrefab != null) 
                Instantiate(equippedPrefab, handTransform);
            if(animationOverride != null)
                animator.runtimeAnimatorController = animationOverride;
        }

        public float GetRange() { return weaponRange; }

        public float GetDamage() { return weaponDamage; }
    }
}