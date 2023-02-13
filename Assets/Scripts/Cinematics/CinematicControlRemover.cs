using RPG.Control;
using RPG.Core;
using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics
{
    public class CinematicControlRemover : MonoBehaviour
    {

        private GameObject player;

        private void Start()
        {
            GetComponent<PlayableDirector>().played += DisableControl;
            GetComponent<PlayableDirector>().stopped += EnableControl;

            player = GameObject.FindGameObjectWithTag("Player");
        }

        public void DisableControl(PlayableDirector playableDirector) 
        {
            player.GetComponent<ActionScheduler>().CancelCurrentAction();
            player.GetComponent<PlayerController>().enabled = false;
        }

        public void EnableControl(PlayableDirector playableDirector) 
        {
            player.GetComponent<PlayerController>().enabled = true;
        }
    }
}