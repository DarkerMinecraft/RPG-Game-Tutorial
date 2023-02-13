using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using System;
using UnityEngine;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {

        [SerializeField]
        private float chaseDistance, suspicionTime;

        [SerializeField]
        [Range(0f, 1f)]
        private float patrolSpeedFraction;

        [SerializeField]
        private PatrolPath patrolPath;

        [SerializeField]
        private float waypointWaitTime, waypointTolerance;

        private Fighter fighter;
        private Mover mover;
        private Health health;
        private ActionScheduler actionScheduler;

        private GameObject player;

        private Vector3 guardPosition;

        private float timeSinceLastSawPlayer = Mathf.Infinity;
        private float timeSinceAtWaypoint = Mathf.Infinity;

        private int currentWaypointIndex = 0;

        void Start()
        {
            player = GameObject.FindWithTag("Player");

            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
            mover = GetComponent<Mover>();
            actionScheduler = GetComponent<ActionScheduler>();

            guardPosition = transform.position;
        }

        private void Update()
        {
            if (health.IsDead()) return;

            if (CanFightPlayer())
                AttackBehaviour();
            else if (timeSinceLastSawPlayer < suspicionTime)
                SuspicionBehaviour();
            else
                PatrolBehaviour();

            UpdateTimers();
        }

        bool CanFightPlayer() 
        {
            float distance = Vector3.Distance(player.transform.position, transform.position);
            return distance < chaseDistance && fighter.CanAttack(player);
        }

        void UpdateTimers() 
        {
            timeSinceLastSawPlayer += Time.deltaTime;
            timeSinceAtWaypoint += Time.deltaTime;
        }

        void AttackBehaviour() 
        {
            timeSinceLastSawPlayer = 0;
            fighter.Attack(player);
        }

        void SuspicionBehaviour() 
        {
            actionScheduler.CancelCurrentAction();
        }

        void PatrolBehaviour()
        {
            Vector3 nextPosition = guardPosition;

            if (patrolPath != null)
            {
                if (AtWaypoint())
                {
                    if (timeSinceAtWaypoint > waypointWaitTime)
                    {
                        CycleWaypoint();
                        timeSinceAtWaypoint = 0;
                    }
                }
                nextPosition = GetCurrentWaypoint();
            } else mover.StartMoveAction(nextPosition, patrolSpeedFraction);

            if (timeSinceAtWaypoint > waypointWaitTime) { 
                mover.StartMoveAction(nextPosition, patrolSpeedFraction);
            }
        }

        private Vector3 GetCurrentWaypoint()
        {
            return patrolPath.GetWaypoint(currentWaypointIndex);
        }

        private void CycleWaypoint()
        {
            currentWaypointIndex = patrolPath.GetNextIndex(currentWaypointIndex);
        }

        private bool AtWaypoint()
        {
            float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWaypoint());
            return distanceToWaypoint < waypointTolerance;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }

    }
}