using RPG.Combat;
using RPG.Core;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction
    {

        private NavMeshAgent agent;
       
        private Animator animator;

        private ActionScheduler actionScheduler;

        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
            actionScheduler = GetComponent<ActionScheduler>();
        }

        void Update()
        {
            UpdateAnimator();
        }

        public void Cancel()
        {
            agent.isStopped = true;
        }

        public void StartMoveAction(Vector3 destination) 
        {
            actionScheduler.StartAction(this);
            MoveTo(destination);
        }

        public void MoveTo(Vector3 destination)
        {
            agent.isStopped = false;
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