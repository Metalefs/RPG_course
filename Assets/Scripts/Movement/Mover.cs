using RPG.Core;
using RPG.Attributes;
using RPG.Interfaces;
using RPG.Saving;
using UnityEngine;
using UnityEngine.AI;
namespace RPG.Movement
{
    public class Mover : IAction, ISaveable
    {
        [SerializeField] float maxSpeed = 6f;
        private NavMeshAgent Agent;
        private Animator Animator;
        private Health Health;
        Ray lastRay;

        private void Start()
        {
            Health = GetComponent<Health>();
            Agent = GetComponent<NavMeshAgent>();
            Animator = GetComponent<Animator>();
        }

        void Update()
        {
            Agent.enabled = !Health.IsDead;
            UpdateAnimator();
        }

        public void StartMoveAction(Vector3 destination, float speedFraction = 1f)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            MoveTo(destination, speedFraction);
        }

        public void MoveTo(Vector3 destination, float speedFraction)
        {
            Agent.destination = destination;
            Agent.speed = maxSpeed * Mathf.Clamp01(speedFraction);
            Agent.isStopped = false;
        }

        private float GetSpeed()
        {
            return GetComponent<NavMeshAgent>().speed;
        }

        public override void Cancel()
        {
            Agent.isStopped = true;
        }

        private void UpdateAnimator()
        {
            Vector3 velocity = Agent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            float speed = localVelocity.z;
            Animator.SetFloat("forwardSpeed", speed);
        }

        [System.Serializable]
        struct MoverSaveData
        {
            public SerializableVector3 position;
            public SerializableVector3 rotation;
        }

        public object CaptureState()
        {
            MoverSaveData data = new MoverSaveData();
            data.position = new SerializableVector3(transform.position);
            data.rotation = new SerializableVector3(transform.eulerAngles);
            return data;
        }

        public void RestoreState(object state)
        {
            MoverSaveData data = (MoverSaveData)state;
            if (Agent != null)
                Agent.enabled = false;
            transform.position = data.position.ToVector();
            transform.eulerAngles = data.rotation.ToVector();
            if (Agent != null)
                Agent.enabled = true;
        }
    }
}
