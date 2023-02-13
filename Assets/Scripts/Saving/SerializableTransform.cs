using System.Collections;
using UnityEngine;

namespace RPG.Saving
{
    [System.Serializable]
    public class SerializableTransform 
    {
        private SerializableVector3 position;
        private SerializableVector3 rotation;
        private SerializableVector3 scale;

        public SerializableTransform(Transform transform) 
        {
            this.position = new SerializableVector3(transform.position);
            this.rotation = new SerializableVector3(transform.rotation.eulerAngles);
            this.scale = new SerializableVector3(transform.localScale);
        }

        public void SetTransform(Transform transform) 
        {
            transform.position = position.ToVector3();
            transform.rotation = Quaternion.Euler(rotation.ToVector3());
            transform.localScale = scale.ToVector3();
        }
    }
}