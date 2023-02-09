using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using UnityEngine;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {

        private Mover mover;
        private Fighter fighter;
        private Health health;

        private new Camera camera;

        void Start()
        {
            mover = GetComponent<Mover>();
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();

            camera = Camera.main;
        }

        void Update()
        {
            if (health.IsDead()) return;

            if (InteractWithCombat()) return;
            if (InteractWithMovement()) return;
        }

        bool InteractWithMovement()
        {
            RaycastHit hit;

            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);
            if (hasHit)
            {
                if (Input.GetMouseButton(0))
                    mover.StartMoveAction(hit.point);
            }
            return hasHit;
        }

        bool InteractWithCombat()
        {
            bool interact = false;
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());

            foreach (RaycastHit hit in hits)
            {
                CombatTarget combatTarget = hit.collider.gameObject.GetComponent<CombatTarget>();
                if (combatTarget == null) continue;
                if (!fighter.CanAttack(combatTarget.gameObject)) continue;

                interact = true;
                if (Input.GetMouseButton(0))
                    fighter.Attack(combatTarget.gameObject);
            }
            return interact;
        }

        private Ray GetMouseRay()
        {
            return camera.ScreenPointToRay(Input.mousePosition);
        }
    }
}
