using RPG.Control;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class WeaponPickup : MonoBehaviour, IRaycastable
    {
        [SerializeField]
        private Weapon weapon;

        [SerializeField]
        private float respawnTime = 5;

        private GameObject[] children;

        private void Start () 
        {

            children = new GameObject[transform.childCount];

            for (int i = 0; i < transform.childCount; i++)
                children[i] = transform.GetChild(i).gameObject;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                Pickup(other.GetComponent<Fighter>());
            }
        }

        private void Pickup(Fighter fighter)
        {
            fighter.EquipWeapon(weapon);
            StartCoroutine(HideForSeconds(respawnTime));
        }

        private IEnumerator HideForSeconds(float seconds) 
        {
            ShowPickup(false);
            yield return new WaitForSeconds(seconds);
            ShowPickup(true);
        }

        void ShowPickup(bool shouldShow) 
        {
            GetComponent<Collider>().enabled = shouldShow;
            foreach (GameObject obj in children)
                obj.SetActive(shouldShow);
        }

        public bool HandleRaycast(PlayerController playerController)
        {
            if (Input.GetMouseButton(0)) 
                Pickup(playerController.GetComponent<Fighter>());
            return true;
        }

        public CursorType GetCursorType()
        {
            return CursorType.Pickup;
        }
    }
}
