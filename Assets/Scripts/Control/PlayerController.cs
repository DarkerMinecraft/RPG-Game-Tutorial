using Cinemachine;
using RPG.Attributes;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {

        private Mover mover;
        private Fighter fighter;
        private Health health;

        private new Camera camera;

        [System.Serializable]
        class CursorMapping 
        {
            public CursorType type;

            public Texture2D texture;
            public Vector2 hotspot;
        }

        [SerializeField]
        private float maxNavMeshProjectionDistance = 1;

        [SerializeField]
        private CursorMapping[] mapping;

        void Start()
        {
            mover = GetComponent<Mover>();
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();

            camera = Camera.main;
        }

        void Update()
        {
            if (InteractWithUI()) 
            {
                SetCursor(CursorType.UI);
                return;
            }
            if (health.IsDead())
            {
                SetCursor(CursorType.None);
                return;
            }

            if (InteractWithComponent()) return; 
            if (InteractWithMovement()) return;

            SetCursor(CursorType.None);
        }

        private bool InteractWithComponent()
        {
            bool interact = false;
            RaycastHit[] hits = RaycastAllSorted();

            foreach (RaycastHit hit in hits)
            {
                IRaycastable[] raycastables = hit.transform.GetComponents<IRaycastable>();
                foreach(IRaycastable raycastable in raycastables) 
                {
                    

                    if (raycastable.HandleRaycast(this)) 
                    {
                        interact = true;
                        SetCursor(raycastable.GetCursorType());
                    }
                }
            }
            return interact;
        }

        RaycastHit[] RaycastAllSorted() 
        {
            RaycastHit[] raycastHits = Physics.RaycastAll(GetMouseRay());
            float[] distances = new float[raycastHits.Length];
            for (int i = 0; i < distances.Length; i++) 
            {
                distances[i] = raycastHits[i].distance;
            }
            Array.Sort(distances, raycastHits);
            return raycastHits;
        }

        bool InteractWithUI() 
        {
            return EventSystem.current.IsPointerOverGameObject();
        }

        bool InteractWithMovement()
        {
            Vector3 target;

            bool hasHit = RaycastNavMesh(out target); 
            if (hasHit)
            {
                if (!mover.CanMoveTo(target)) return false;
                
                if (Input.GetMouseButton(0))
                    mover.StartMoveAction(target);
                SetCursor(CursorType.Movement);
            }
            return hasHit;
        }

        private bool RaycastNavMesh(out Vector3 target)
        {
            RaycastHit hit;
            target = new Vector3();

            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);

            if (!hasHit) return false;

            NavMeshHit navMeshHit;
            bool hasCastToNavMesh = NavMesh.SamplePosition(hit.point, out navMeshHit, 
                maxNavMeshProjectionDistance, NavMesh.AllAreas);

            if(!hasCastToNavMesh) return false;
            target = navMeshHit.position;

            return mover.CanMoveTo(target);
        }

        

        private void SetCursor(CursorType type)
        {
            CursorMapping mapping = GetCursorMapping(type);
            Cursor.SetCursor(mapping.texture, mapping.hotspot, CursorMode.Auto);
        }

        private CursorMapping GetCursorMapping(CursorType type) 
        {
            CursorMapping cursorMap = null;
            foreach (CursorMapping cursorMapping in mapping) 
            {
                if (cursorMapping.type == type) 
                {
                    cursorMap = cursorMapping;
                }
            }
            return cursorMap;
        }

        private Ray GetMouseRay()
        {
            return camera.ScreenPointToRay(Input.mousePosition);
        }
    }
}
