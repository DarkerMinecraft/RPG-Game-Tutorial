using RPG.Attributes;
using RPG.Core;
using RPG.Saving;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction, ISaveable
    {

        [SerializeField]
        private float maxSpeed, maxPathLength = 40;

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

        public bool CanMoveTo(Vector3 destination) 
        {
            NavMeshPath path = new NavMeshPath();
            bool hasPath = NavMesh.CalculatePath(transform.position, destination, NavMesh.AllAreas, path);
            if (!hasPath || path.status != NavMeshPathStatus.PathComplete) return false;

            if (GetPathLength(path) > maxPathLength) return false;

            return true;
        }

        private float GetPathLength(NavMeshPath path)
        {
            float distance = 0;
            if (path.corners.Length < 2) return 0;

            for (int i = 0; i < path.corners.Length - 1; i++)
            {
                Vector3 firstVector = path.corners[i];
                Vector3 secondVector = path.corners[i + 1];

                distance += Vector3.Distance(firstVector, secondVector);
            }
            return distance;
        }

        private void UpdateAnimator()
        {
            Vector3 velocity = agent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);

            float speed = localVelocity.z;
            animator.SetFloat("forwardSpeed", speed);
        }

        public object CaptureState()
        {
            return new SerializableTransform(transform);
        }

        public void RestoreState(object state)
        {
            SerializableTransform serializableTransform = (SerializableTransform) state;
            GetComponent<NavMeshAgent>().enabled = false;
            serializableTransform.SetTransform(transform);
            GetComponent<NavMeshAgent>().enabled = true;
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }
    }

}