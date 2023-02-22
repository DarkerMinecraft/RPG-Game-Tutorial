using RPG.Attributes;
using RPG.Core;
using System.Collections;
using UnityEngine;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {

        [SerializeField]
        private float speed;

        [SerializeField]
        private bool isHoming = true;

        [SerializeField]
        private GameObject hitEffect;

        [SerializeField]
        private float maxLifeTime = 10, lifeAfterImpact = 2;

        [SerializeField]
        private GameObject[] destroyOnHit; 
        

        private Health target;
        private float damage;

        private void Start()
        {
            transform.LookAt(GetAimLocation());
        }

        void Update()
        {
            if (target == null) return;

            if(isHoming && !target.IsDead())
                transform.LookAt(GetAimLocation());

            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }

        public void SetTarget(Health target, float damage)
        {
            this.target = target; 
            this.damage = damage;

            Destroy(gameObject, maxLifeTime);
        }

        Vector3 GetAimLocation() 
        {
            CapsuleCollider targetCapsule = target.GetComponent<CapsuleCollider>();
            return target.transform.position + Vector3.up * targetCapsule.height / 2;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Health>() != target) return;

            if (target.IsDead()) return;

            target.TakeDamage(damage);
            speed = 0;

            if (hitEffect != null)
                Instantiate(hitEffect, GetAimLocation(), transform.rotation);
            

            foreach (GameObject obj in destroyOnHit)
                Destroy(obj);
           
            Destroy(gameObject, lifeAfterImpact);
        }
    }
}