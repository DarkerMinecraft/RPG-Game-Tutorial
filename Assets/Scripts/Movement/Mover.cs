using UnityEngine;
using UnityEngine.AI;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour
    {

        private NavMeshAgent agent;
        private Camera cam;

        private Animator animator;

        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            cam = Camera.main;
            animator = GetComponent<Animator>();
        }

        void Update()
        {
            UpdateAnimator();
        }

        private void MoveToCursor()
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
            bool hasHit = Physics.Raycast(ray, out hit);

            if (hasHit)
            {
                MoveTo(hit.point);
            }
        }

        public void MoveTo(Vector3 destination)
        {
            agent.destination = destination;
        }

        private void UpdateAnimator()
        {
            Vector3 velocity = agent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);

            float speed = localVelocity.z;
            animator.SetFloat("forwardSpeed", speed);
        }
    }

}