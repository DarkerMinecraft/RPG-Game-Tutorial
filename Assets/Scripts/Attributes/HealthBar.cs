using System.Collections;
using UnityEngine;

namespace RPG.Attributes
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField]
        private Health health;
        [SerializeField]
        private RectTransform foreground;

        private Canvas rootCanvas; 

        void Start()
        {
            rootCanvas = GetComponentInChildren<Canvas>();    
        }

        void Update()
        {
            float fraction = health.GetFraction();

            if (Mathf.Approximately(fraction, 0))
            {
                rootCanvas.enabled = false;
                return;
            }

            rootCanvas.enabled = true;
            foreground.localScale = new Vector3(fraction, 1, 1);
        }
    }
}