using System.Collections;
using UnityEngine;

namespace RPG.Core
{
    public class ActionScheduler : MonoBehaviour
    {

        private MonoBehaviour currentAction = null;

        public void StartAction(MonoBehaviour action) 
        {
            if (currentAction == null)
            {
                currentAction = action;
                return;
            }

            print("Cancelling Action " + currentAction.name);
            currentAction = action;
        }
        
    }
}