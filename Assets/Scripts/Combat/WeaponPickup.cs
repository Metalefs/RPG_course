using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class WeaponPickup : MonoBehaviour
    {
        [SerializeField] Weapon weapon = null;
        [SerializeField] float destroyDelay = 0f;
        
        void OnDrawGizmos() {
            Gizmos.DrawWireSphere(transform.position, 1);
        }

        void OnTriggerEnter(Collider other)
        {
            Debug.Log("Triggered");
            if (other.gameObject.tag == "Player")
            {
                other.gameObject.GetComponent<Fighter>().EquipWeapon(weapon);
                Destroy(gameObject, destroyDelay);
            }
        }
    }

}