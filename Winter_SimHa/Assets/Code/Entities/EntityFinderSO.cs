using System;
using UnityEngine;

namespace Code.Entities
{
    [CreateAssetMenu(fileName = "EntityFinder", menuName = "SO/Entity/Finder", order = 0)]
    public class EntityFinderSO : ScriptableObject
    {
        [SerializeField] private string targetTag;
        public Entity target;

        private void OnEnable()
        {
            if (target != null) return;
            if (string.IsNullOrEmpty(targetTag)) return;
            
            GameObject targetObject = GameObject.FindGameObjectWithTag(targetTag);
            if (targetObject != null)
            {
                target = targetObject.GetComponent<Entity>();
            }
        }

        public void SetEntity(Entity entity)
        {
            target = entity;
        }
    }
}