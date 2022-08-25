using RPG.Core;
using RPG.Stats;
using RPG.Saving;
using UnityEngine;
using GameDevTV.Utils;

namespace RPG.Attributes
{
    [RequireComponent(typeof(BaseStats))]
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] float regenerationPercentage = 70;
        public LazyValue<float> health;
        bool isDead = false;

        public bool IsDead { get { return isDead; } }
        public delegate void OnDamageTaken(GameObject instigator);
        public event OnDamageTaken onDamageTaken;

        private void Awake() {
            health = new LazyValue<float>(GetInitialHealth);
        }

        private float GetInitialHealth(){
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        private void Start() {
            health.ForceInit();
        }

        private void OnEnable() {
            GetComponent<BaseStats>().onLevelUp += RegenerateHealth;
        }

        private void OnDisable() {
            GetComponent<BaseStats>().onLevelUp -= RegenerateHealth;
        }

        public void TakeDamage(GameObject instigator, float damage)
        {
            health.value = Mathf.Max(health.value - damage, 0);
            if (health.value == 0)
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

        void RegenerateHealth()
        {
            float regenHealth = GetComponent<BaseStats>().GetStat(Stat.Health) * (regenerationPercentage / 100);
            health.value = Mathf.Max(health.value, regenHealth);
        }

        public object CaptureState()
        {
            return health;
        }

        public void RestoreState(object state)
        {
            health.value = (float)state;
            if (health.value == 0)
            {
                Die();
            }
        }
    }
}
