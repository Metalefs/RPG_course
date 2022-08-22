using RPG.Core;
using RPG.Stats;
using RPG.Saving;
using UnityEngine;
namespace RPG.Attributes
{
    [RequireComponent(typeof(BaseStats))]
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] float regenerationPercentage = 70;
        public float health = -1f;
        bool isDead = false;

        public bool IsDead { get { return isDead; } }
        public delegate void OnDamageTaken(GameObject instigator);
        public event OnDamageTaken onDamageTaken;

        private void Start() {
            if(health < 0)
            {
                health = GetComponent<BaseStats>().GetStat(Stat.Health);
            }
            GetComponent<BaseStats>().onLevelUp += RegenerateHealth;
        }

        void RegenerateHealth()
        {
            float regenHealth = GetComponent<BaseStats>().GetStat(Stat.Health) * (regenerationPercentage / 100);
            health = Mathf.Max(health, regenHealth);
        }

        public object CaptureState()
        {
            return health;
        }

        public void RestoreState(object state)
        {
            health = (float)state;
            if (health == 0)
            {
                Die();
            }
        }

        public void TakeDamage(GameObject instigator, float damage)
        {
            health = Mathf.Max(health - damage, 0);
            if (health == 0)
            {
                Die();
                AwardExperience(instigator);
            }
            onDamageTaken?.Invoke(instigator);
        }

        private void Die()
        {
            if (isDead) return;
            isDead = true;
            GetComponent<Animator>()?.SetTrigger("die");
            GetComponent<ActionScheduler>()?.CancelCurrentAction();
        }

        private void AwardExperience(GameObject instigator)
        {
            Experience experience = instigator.GetComponent<Experience>();
            if (experience == null) return;
            experience.AddExperience(GetComponent<BaseStats>().GetStat(Stat.ExperienceReward));
        }
    }
}
