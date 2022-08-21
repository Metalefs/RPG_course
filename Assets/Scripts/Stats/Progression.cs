using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression", order = 0)]
    public class Progression : ScriptableObject
    {
        [SerializeField] ProgressionCharacterClass[] characterClasses = null;
        Dictionary<CharacterClass, Dictionary<Stat, float[]>> lookupTable = null;

        public float GetStat(Stat stat, CharacterClass characterClass, int level)
        {
            BuildLookup();

            float[] levels = lookupTable[characterClass][stat];

            if (levels.Length < level)
            {
                return 0;
            }

            return levels[level - 1];
        }

        private void BuildLookup()
        {
            if (lookupTable != null) return;

            lookupTable = new Dictionary<CharacterClass, Dictionary<Stat, float[]>>();

            foreach (ProgressionCharacterClass progressionClass in characterClasses)
            {
                var statLookupTable = new Dictionary<Stat, float[]>();

                foreach (ProgressionStat progressionStat in progressionClass.stats)
                {
                    statLookupTable[progressionStat.stat] = progressionStat.levels;
                }

                lookupTable[progressionClass.characterClass] = statLookupTable;
            }
        }

        public void SetLevel(int level)
        {
            foreach (ProgressionCharacterClass progressionCharacterClass in characterClasses)
            {
                progressionCharacterClass.LevelUp(level);
            }
        }

        private ProgressionCharacterClass GetProgressionCharacterClass(CharacterClass characterClass)
        {
            foreach (ProgressionCharacterClass progressionCharacterClass in characterClasses)
            {
                if (progressionCharacterClass.characterClass == characterClass)
                {
                    return progressionCharacterClass;
                }
            }
            return null;
        }

        [System.Serializable]
        public class ProgressionCharacterClass
        {
            public CharacterClass characterClass;
            public ProgressionStat[] stats = null;
            public int level = 1;


            public float GetStat(Stat stat)
            {
                foreach (ProgressionStat progressionStat in stats)
                {
                    if (progressionStat.stat == stat)
                    {
                        return progressionStat.GetValueAtLevel(level);
                    }
                }
                return 0;
            }

            public void LevelUp(int level)
            {
                this.level = level;
            }
        }

        [System.Serializable]
        public class ProgressionStat
        {
            public Stat stat;
            public float[] levels;

            public float GetValueAtLevel(int level)
            {
                return levels[level - 1];
            }
        }
    }
}