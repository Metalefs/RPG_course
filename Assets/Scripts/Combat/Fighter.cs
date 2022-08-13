using UnityEngine;using RPG.Movement;
using RPG.Interfaces;
using RPG.Core;
namespace RPG.Combat
{
    public class Fighter : IAction
    {
        [SerializeField] float weaponRange = 2f;
        [SerializeField] float timeBetweenAttacks = 1f;
        [SerializeField] float weaponDamage = 5f;
        [SerializeField] Transform rightHandTransform = null;
        [SerializeField] Transform leftHandTransform = null;
        [SerializeField] Weapon DefaultWeapon = null;
        [SerializeField] Weapon CurrentWeapon = null;

        
        float timeSinceLastAttack = Mathf.Infinity;
        Health target;
        Health health;

        private void Start()
        {
            health = GetComponent<Health>();
            EquipWeapon(DefaultWeapon);
        }
        
        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;
            if (target == null) return;
            if (target.IsDead || health.IsDead) return;
            if (target != null && !GetIsInRange())
            {
                GetComponent<Mover>().MoveTo(target.transform.position, 1f);
            }
            else
            {
                GetComponent<Mover>().Cancel();
                AttackBehaviour();
            }
        }

        public void EquipWeapon(Weapon weapon)
        {
            CurrentWeapon = weapon;
            CurrentWeapon.Spawn(rightHandTransform, leftHandTransform, GetComponent<Animator>());
        }


       private void AttackBehaviour()
        {
            transform.LookAt(target.transform);
            if (timeSinceLastAttack > timeBetweenAttacks)
            {
                // This will trigger the Hit() event.
                TriggerAttack();
                timeSinceLastAttack = 0;
            }
        }
        private void TriggerAttack()
        {
            GetComponent<Animator>().ResetTrigger("stopAttack");
            GetComponent<Animator>().SetTrigger("attack");
        }
        // Animation Event
        void Hit()
        {
            if(target == null) return;
            
            if (CurrentWeapon.HasProjectile())
            {
                CurrentWeapon.LaunchProjectile(rightHandTransform, leftHandTransform, target);
            }
            else
            {
                target.TakeDamage(CurrentWeapon.GetDamage());
            }
        }

        // Animation Event
        void Shoot()
        {
            Hit();
        }

        private bool GetIsInRange()
        {
            return Vector3.Distance(transform.position, target.transform.position) < CurrentWeapon.GetRange();
        }
        public bool CanAttack(GameObject combatTarget)
        {
            if (combatTarget == null) return false;
            if (combatTarget.name == gameObject.name) return false;
            Health targetToTest = combatTarget.GetComponent<Health>();
            return targetToTest != null && !targetToTest.IsDead;
        }
        public void Attack(GameObject combatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget?.GetComponent<Health>();
        }
        public override void Cancel()
        {
            StopAttack();
            target = null;
        }
        private void StopAttack()
        {
            GetComponent<Animator>().ResetTrigger("attack");
            GetComponent<Animator>().SetTrigger("stopAttack");
        }
    }
}