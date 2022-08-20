using UnityEngine;
namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [Range(1, 99)]
        [SerializeField] int startingLevel = 1;
        [SerializeField] CharacterClass characterClass = CharacterClass.Grunt;
        [SerializeField] Progression progression = null;   

        [SerializeField] GameObject levelUpParticle = null;
        [SerializeField] AudioClip levelUpSFX = null;

        void Start()
        {
            if (progression == null)
            {
                Debug.LogError("No progression found on " + name);
            }
            else
            {
                //SetLevel(startingLevel);
            }
        }
        public float GetHealth()
        {
            return progression.GetHealth(characterClass, startingLevel);
        }
    }
}