using RPG.Saving;
using UnityEngine;
namespace RPG.Core
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] float health = 100f;
        bool isDead = false;

        public bool IsDead { get { return isDead; } }

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

        public void TakeDamage(float damage)
        {
            health = Mathf.Max(health - damage, 0);
            Debug.Log(gameObject.name + " " + health);
            if (health == 0)
            {
                Die();
            }
        }

        private void Die()
        {
            if (isDead) return;
            isDead = true;
            GetComponent<Animator>()?.SetTrigger("die");
            GetComponent<ActionScheduler>()?.CancelCurrentAction();
        }
    }
}
