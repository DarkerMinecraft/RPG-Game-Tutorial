using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics 
{
    public class CinematicTrigger : MonoBehaviour
    {
        private PlayableDirector playableDirector;

        private bool playCutscene = true;

        private GameObject player;

        private void Start()
        {
            playableDirector = GetComponent<PlayableDirector>();
            player = GameObject.FindGameObjectWithTag("Player");
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject == player && playCutscene)
            {
                playableDirector.Play();
                playCutscene = false;
            }
        }
    }
}
