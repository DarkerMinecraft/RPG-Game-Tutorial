using RPG.Combat;
using RPG.Core;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction
    {

        [SerializeField]
        private float maxSpeed;

        private NavMeshAgent agent;
        private Animator animator;
        private ActionScheduler actionScheduler;
        private Health health;

        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
            actionScheduler = GetComponent<ActionScheduler>();
            health = GetComponent<Health>();
        }

        void Update()
        {
            agent.enabled = !health.IsDead();

            UpdateAnimator();
        }

        public void Cancel()
        {
            agent.isStopped = true;
        }

        public void StartMoveAction(Vector3 destination, float speedFraction = 1) 
        {
            actionScheduler.StartAction(this);
            MoveTo(destination, speedFraction);
        }

        public void MoveTo(Vector3 destination, float speedFraction = 1)
        {
            agent.isStopped = false;
            agent.speed = maxSpeed * Mathf.Clamp01(speedFraction);
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