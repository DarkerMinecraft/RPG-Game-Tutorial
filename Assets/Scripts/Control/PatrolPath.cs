using System.Collections;
using UnityEngine;

namespace RPG.Control
{
    public class PatrolPath : MonoBehaviour
    {
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.white;
            for (int i = 0; i < transform.childCount; i++) 
            {
                int j = GetNextIndex(i);
                Gizmos.DrawSphere(GetWaypoint(i), 0.2f);
                Gizmos.DrawLine(GetWaypoint(i), GetWaypoint(j));
            }
        }

        public int GetNextIndex(int index) 
        {
            return index + 1 >= transform.childCount ? 0 : index + 1;
        }

        public Vector3 GetWaypoint(int index) 
        {
            return transform.GetChild(index).position;
        }
    }
}