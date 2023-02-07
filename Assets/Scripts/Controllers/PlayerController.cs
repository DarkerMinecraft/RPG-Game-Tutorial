using RPG.Combat;
using RPG.Movement;
using UnityEngine;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {

        private Mover mover;
        private Fighter fighter;

        private new Camera camera;

        void Start()
        {
            mover = GetComponent<Mover>();
            fighter = GetComponent<Fighter>();

            camera = Camera.main;
        }

        void Update()
        {
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
                    mover.MoveTo(hit.point);
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

                interact = true;
                if (Input.GetMouseButtonDown(0))
                    fighter.Attack(combatTarget);
            }
            return interact;
        }

        bool MoveToCursor()
        {
            RaycastHit hit;

            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);
            if (hasHit)
                mover.MoveTo(hit.point);
            return hasHit;
        }

        private Ray GetMouseRay()
        {
            return camera.ScreenPointToRay(Input.mousePosition);
        }
    }
}
