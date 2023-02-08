using RPG.Combat;
using UnityEngine;

namespace RPG.Controllers
{
    public class AIController : MonoBehaviour
    {

        [SerializeField]
        private float chaseDistance;

        private Fighter fighter;

        private GameObject player;

        void Start()
        {
            player = GameObject.FindWithTag("Player");

            fighter = GetComponent<Fighter>();
        }

        private void Update()
        {
            if (CanFightPlayer())
                fighter.Attack(player);
            else
                fighter.Cancel();
        }

        bool CanFightPlayer() 
        {
            float distance = Vector3.Distance(player.transform.position, transform.position);
            return distance < chaseDistance && fighter.CanAttack(player);
        }

    }
}