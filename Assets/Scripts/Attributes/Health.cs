using RPG.Core;
using RPG.Stats;
using RPG.Saving;
using UnityEngine;
namespace RPG.Attributes
{
    [RequireComponent(typeof(BaseStats))]
    public class Health : MonoBehaviour, ISaveable
    {
        public float health = -1f;
        bool isDead = false;

        public bool IsDead { get { return isDead; } }

        private void Start() {
            if(health < 0)
            {
                health = GetComponent<BaseStats>().GetStat(Stat.Health);
            }
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
