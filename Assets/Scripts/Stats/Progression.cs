using UnityEngine;

namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression", order = 0)]
    public class Progression : ScriptableObject
    {
        [SerializeField] ProgressionCharacterClass[] characterClasses = null;

        public float GetHealth(CharacterClass characterClass, int level)
        {
            foreach (ProgressionCharacterClass progressionCharacterClass in characterClasses)
            {
                if (progressionCharacterClass.characterClass == characterClass)
                {
                    return progressionCharacterClass.GetHealth(level);
                }
            }
            return 0;
        }

        public float GetLevel(CharacterClass characterClass)
        {
            foreach (ProgressionCharacterClass progressionCharacterClass in characterClasses)
            {
                if (progressionCharacterClass.characterClass == characterClass)
                {
                    return progressionCharacterClass.level;
                }
            }
            return 0;
        }
        public float GetExpToLevelUp(CharacterClass characterClass)
        {
            foreach (ProgressionCharacterClass progressionCharacterClass in characterClasses)
            {
                if (progressionCharacterClass.characterClass == characterClass)
                {
                    return progressionCharacterClass.expToLevelUp;
                }
            }
            return 0;
        }
        [System.Serializable]
        public class ProgressionCharacterClass
        {
            public CharacterClass characterClass;
            public int level;
            public float expToLevelUp;
            public float[] health;

            public float GetHealth(int level)
            {
                return health[level-1];
            }
        }
    }
}