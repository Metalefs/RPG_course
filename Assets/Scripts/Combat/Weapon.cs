using UnityEngine;
namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Weapon", order = 0)]
    public class Weapon : ScriptableObject
    {
        [SerializeField] public float timeBetweenAttacks = 1f;
        [SerializeField] public float weaponRange = 2f;
        [SerializeField] public float weaponDamage = 5f;
        [SerializeField] public GameObject weaponPrefab = null;
        [SerializeField] public AnimatorOverrideController animatorOverride;

        public void Spawn(Transform RPosition, Transform LPosition, Animator animator)
        {
            if (weaponPrefab != null)
            {
                Instantiate(weaponPrefab, RPosition);

                weaponPrefab.transform.localPosition = Vector3.zero;
                weaponPrefab.transform.localRotation = Quaternion.identity;
            }
            if (animatorOverride != null)
                animator.runtimeAnimatorController = animatorOverride;
        }

        public float GetTimeBetweenAttacks()
        {
            return timeBetweenAttacks;
        }

        public float GetWeaponRange()
        {
            return weaponRange;
        }

        public float GetWeaponDamage()
        {
            return weaponDamage;
        }
    }
}