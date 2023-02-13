using RPG.Saving;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

namespace RPG.SceneManagement
{
    public class Portal : MonoBehaviour
    {

        enum DestinationIdentifier 
        {
            A, B, C, D, E
        }

        [SerializeField]
        private int sceneToLoad = -1;

        [SerializeField]
        private Transform spawnPoint;

        [SerializeField]
        private DestinationIdentifier destinationIdentifier;

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                StartCoroutine(Transition());
            }
        }

        private IEnumerator Transition() 
        {
            DontDestroyOnLoad(gameObject);

            SavingWrapper savingWrapper = FindObjectOfType<SavingWrapper>();

            savingWrapper.Save();

            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneToLoad);
            yield return asyncOperation;

            savingWrapper.Load();

            Portal otherPortal = GetOtherPortal();
            UpdatePlayer(otherPortal);

            Destroy(gameObject);
        }

        Portal GetOtherPortal() 
        {
            foreach (Portal portal in FindObjectsOfType<Portal>()) 
            {
                if (portal == this) continue;
                if (portal.destinationIdentifier != destinationIdentifier) continue;

                return portal;
            }
            return null;
        }

        void UpdatePlayer(Portal portal) 
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            player.GetComponent<NavMeshAgent>().enabled = false;
            player.transform.position = portal.spawnPoint.position;
            player.transform.rotation = portal.spawnPoint.rotation;
            player.GetComponent<NavMeshAgent>().enabled = true;
        }
    }
}
