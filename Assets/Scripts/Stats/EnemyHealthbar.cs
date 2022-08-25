using System.Collections;
using RPG.Attributes;
using RPG.Combat;
using RPG.Stats;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthbar : MonoBehaviour
{
    [SerializeField] Health enemy;
    [SerializeField] Canvas canvas;
    private Image healthbar;
    private BaseStats stats;
    private Fighter playerFighter;

    private float currentHealth;
    public float fillSmoothness = 0.01f;
    void Start()
    {
        playerFighter = GameObject.FindGameObjectWithTag("Player").GetComponent<Fighter>();
        healthbar = GetComponent<Image>();
        stats = enemy.GetComponent<BaseStats>();
        currentHealth = enemy.health.value;
        canvas.worldCamera = Camera.main;
    }

    void Update()
    {
        if (enemy.IsDead)
        {
            Destroy(gameObject);
        }


        float prevFill = healthbar.fillAmount;
        float currFill = enemy.health.value / stats.GetStat(Stat.Health);
        if (currFill > prevFill) prevFill = Mathf.Min(prevFill + fillSmoothness, currFill);
        else if (currFill < prevFill) prevFill = Mathf.Max(prevFill - fillSmoothness, currFill);
        healthbar.fillAmount = prevFill;

        //billboard the healthbar to the camera
        // ignore the y axis
        transform.LookAt(2 * transform.position - Camera.main.transform.position, Vector3.up);
        
        //if the enemy is targeting the player, show the healthbar
        if (playerFighter.GetTarget()?.gameObject == enemy.gameObject)
        {
            healthbar.enabled = true;
        }
        else
        {
            healthbar.enabled = false;
        }
    }
}
