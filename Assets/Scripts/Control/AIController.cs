using UnityEngine;
using RPG.Movement;
using RPG.Combat;
using RPG.Core;
using RPG.Attributes;
namespace RPG.Control {
    public class AIController: MonoBehaviour {
        [SerializeField] float chaseDistance = 5f;
        [SerializeField] float suspicionTime = 5f;
        [SerializeField] float waypointTolerance = 1f;
        [SerializeField] float waypointDwellTime = 3f;
        [SerializeField] PatrolPath patrolPath;
        [SerializeField] float shoutDistance = 5f;
        [Range(0, 1)] [SerializeField] float patrolSpeedFraction = .2f;
        GameObject player;
        Fighter fighter;
        Health health;
        Mover mover;
        Vector3 guardPosition;
        float timeSinceLastSawPlayer = Mathf.Infinity;
        float timeSinceArrivedAtWaypoint = Mathf.Infinity;
        int currentWaypointIndex = 0;
        private void Start() {
            player = GameObject.FindWithTag("Player");
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
            mover = GetComponent<Mover>();
            guardPosition = transform.position;
        }
        private void Update() {
            if (health.IsDead) return;

            if (InAttackRangeOfPlayer() && fighter.CanAttack(player)) {
                AttackBehaviour();
            } else if (timeSinceLastSawPlayer < suspicionTime) {
                SuspicionBehaviour();
            } else {
                PatrolBehaviour();
            }
            UpdateTimers();
        }
        private void UpdateTimers() {
            timeSinceLastSawPlayer += Time.deltaTime;
            timeSinceArrivedAtWaypoint += Time.deltaTime;
        }
        private void PatrolBehaviour() {
            Vector3 nextPosition = guardPosition;
            if (patrolPath != null) {
                if (AtWaypoint()) {
                    timeSinceArrivedAtWaypoint = 0;
                    CycleWaypoint();
                }
                nextPosition = GetCurrentWaypoint();
            }
            if (timeSinceArrivedAtWaypoint > waypointDwellTime) {
                mover.StartMoveAction(nextPosition, patrolSpeedFraction);
            }
        }
        private Vector3 GetCurrentWaypoint() {
            return patrolPath.GetWaypoint(currentWaypointIndex);
        }
        private void CycleWaypoint() {
            currentWaypointIndex = patrolPath.GetNextIndex(currentWaypointIndex);
        }
        private bool AtWaypoint() {
            float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWaypoint());
            return distanceToWaypoint < waypointTolerance;
        }
        private void SuspicionBehaviour() {
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }
        private void AttackBehaviour() {
            timeSinceLastSawPlayer = 0;
            fighter.Attack(player);
            AggroNearbyEnemies();
        }
        private void AggroNearbyEnemies() {
            RaycastHit[] hits = Physics.SphereCastAll(transform.position, shoutDistance, Vector3.up, 0);
            foreach (RaycastHit hit in hits) {
                AIController ai = hit.collider.GetComponent<AIController>();
                if (ai == null) continue;
                ai.Aggro();
            }
        }
        public void Aggro() {
            timeSinceLastSawPlayer = 0;
        }
        private bool InAttackRangeOfPlayer() {
            float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
            return distanceToPlayer < chaseDistance;
        }
        // Called by Unity
        private void OnDrawGizmos() {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
    }
}