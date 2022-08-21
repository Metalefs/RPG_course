using System.Collections;
using System.Collections.Generic;
using RPG.Attributes;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Stats
{
    public class Healthbar : MonoBehaviour
    {
        private Image healthbar;
        private Health health;
        private BaseStats stats;
        private float currentHealth;
        public float fillSmoothness = 0.01f;
        void Start()
        {
            healthbar = GetComponent<Image>();
            health = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
            stats = GameObject.FindGameObjectWithTag("Player").GetComponent<BaseStats>();
            currentHealth = health.health;
        }

        void Update()
        {
            float prevFill = healthbar.fillAmount;
            float currFill = health.health / stats.GetHealth();
            if(currFill > prevFill) prevFill = Mathf.Min(prevFill + fillSmoothness, currFill);
            else if (currFill < prevFill) prevFill = Mathf.Max(prevFill - fillSmoothness, currFill);
            healthbar.fillAmount = prevFill;
        }

    }
}
