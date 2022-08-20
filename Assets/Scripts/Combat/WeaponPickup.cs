using System.Collections;
using System.Collections.Generic;
using RPG.Saving;
using UnityEngine;

namespace RPG.Combat
{
    public class WeaponPickup : MonoBehaviour, ISaveable
    {
        [SerializeField] Weapon weapon = null;
        [SerializeField] float destroyDelay = 0f;
        [SerializeField] float enableDelay = 2f;
        public bool hasPickedUp = false;
        public bool IsClone = false;

        void Start()
        {
            GetComponent<SphereCollider>().enabled = false;
            StartCoroutine(EnablePickup());
        }

        private IEnumerator EnablePickup()
        {
            yield return new WaitForSeconds(enableDelay);
            GetComponent<SphereCollider>().enabled = true;
        }

        private void OnTriggerEnter(Collider other) {
            if (other.gameObject.tag == "Player")
            {
                Pickup(other.gameObject);
            }
        }

        private void Pickup(GameObject subject)
        {
            if (hasPickedUp) return;
            subject.GetComponent<Fighter>().EquipWeapon(weapon,this);
            hasPickedUp = true;
            Destroy(gameObject, destroyDelay);
        }

        public void DropWeaponPickup(WeaponPickup weaponPickupPrefab)
        {
            //DROP OLD WEAPON
            Vector3 dropPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            //instantiate the weapon pickup prefab
            WeaponPickup weaponPickup = Instantiate(weaponPickupPrefab, dropPosition, Quaternion.identity);
        }        
        
        void OnDrawGizmos() {
            Gizmos.DrawWireSphere(transform.position, 1);
        }

        public object CaptureState()
        {
            return IsClone;
        }

        public void RestoreState(object state)
        {
            IsClone = (bool)state;
            if (IsClone)
            {
                Destroy(gameObject);
            }
        }
    }

}