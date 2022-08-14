using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class WeaponPickup : MonoBehaviour
    {
        [SerializeField] Weapon weapon = null;
        [SerializeField] float destroyDelay = 0f;
        [SerializeField] float enableDelay = 2f;
        bool hasPickedUp = false;

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

        public void DropWeaponPickup(GameObject weaponPickupPrefab)
        {
            //DROP OLD WEAPON 1 METER AWAY FROM PLAYER
            Vector3 dropPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            //instantiate the weapon pickup prefab
            GameObject weaponPickup = Instantiate(weaponPickupPrefab, dropPosition, Quaternion.identity);
        }        
        
        void OnDrawGizmos() {
            Gizmos.DrawWireSphere(transform.position, 1);
        }
    }

}