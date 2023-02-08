using RPG.Core;
using RPG.Movement;
using UnityEngine;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour
    {

        [SerializeField]
        private float weaponRange;

        private Transform target;

        private Mover mover;
        private ActionScheduler actionScheduler;

        private void Start()
        {
            mover = GetComponent<Mover>();
            actionScheduler = GetComponent<ActionScheduler>();
        }

        void Update()
        {
            if (target == null) return;

            if (!GetIsInRange())
                mover.MoveTo(target.position);
            else 
                mover.Stop();
        }

        private bool GetIsInRange() 
        {
            return Vector3.Distance(transform.position, target.position) < weaponRange;
        }

        public void Attack(CombatTarget combatTarget)
        {
            actionScheduler.StartAction(this);
            target = combatTarget.transform;
        }

        public void Cancel() 
        {
            target = null;
        }
    }
}
