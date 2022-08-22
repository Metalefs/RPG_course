using UnityEngine;using RPG.Movement;
using RPG.Interfaces;
using RPG.Attributes;
using RPG.Saving;
using RPG.Core;
using RPG.Stats;
using System.Collections.Generic;

namespace RPG.Combat
{
    public class Fighter : IAction, ISaveable, IModifierProvider
    {
        [SerializeField] float weaponRange = 2f;
        [SerializeField] float timeBetweenAttacks = 1f;
        [SerializeField] float weaponDamage = 5f;
        [SerializeField] Transform rightHandTransform = null;
        [SerializeField] Transform leftHandTransform = null;
        [SerializeField] Weapon DefaultWeapon = null;
        [SerializeField] Weapon CurrentWeapon = null;

        
        float timeSinceLastAttack = Mathf.Infinity;
        Health target = null;
        Health health;
        BaseStats stats;

        private void Start()
        {
            health = GetComponent<Health>();
            stats = GetComponent<BaseStats>();
            if(CurrentWeapon == null)
            {
                EquipWeapon(DefaultWeapon);
            }
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

        public void EquipWeapon(Weapon weapon, WeaponPickup weaponPickup = null)
        {
            if(CurrentWeapon?.weaponPickupPrefab != null)
            {
                weaponPickup?.DropWeaponPickup(CurrentWeapon.weaponPickupPrefab);
            }
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
            float damage = GetComponent<BaseStats>().GetStat(Stat.Damage);
            if (CurrentWeapon.HasProjectile())
            {
                CurrentWeapon.LaunchProjectile(rightHandTransform, leftHandTransform, target, gameObject, damage);
            }
            else
            {
                target.TakeDamage(gameObject, damage + stats.GetStat(Stat.Strength) * 0.65f);
            }
            Debug.Log(damage);
            Debug.Log(stats.GetStat(Stat.Strength) * 0.65f);
            Debug.Log(damage + stats.GetStat(Stat.Strength) * 0.65f);
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

        public Health GetTarget()
        {
            return target;
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

        public IEnumerable<float> GetAdditiveModifiers(Stat stat)
        {
            if(stat == Stat.Damage)
            {
                yield return weaponDamage;
            }
        }

        public IEnumerable<float> GetPercentageModifiers(Stat stat)
        {
            if(stat == Stat.Damage)
            {
                yield return CurrentWeapon.GetPercentageBonus();
            }
        }

        public object CaptureState()
        {
            return CurrentWeapon.name;
        }

        public void RestoreState(object state)
        {
            string weaponName = (string)state;
            Weapon weapon = Resources.Load(weaponName, typeof(Weapon)) as Weapon;
            EquipWeapon(weapon);
        }

    }
}