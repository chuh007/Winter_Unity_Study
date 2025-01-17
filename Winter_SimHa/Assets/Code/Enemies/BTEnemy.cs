using Code.Core.EventSystems;
using Code.Entities;
using Unity.Behavior;
using UnityEngine;

namespace Code.Enemies
{
    public abstract class BTEnemy : Entity
    {
        [field: SerializeField] public EntityFinderSO PlayerFinder { get; protected set; }
        public LayerMask whatIsPlayer;
        public float FacingDirection => _renderer.FacingDirection;

        //임시코드
        [SerializeField] protected GameEventChannelSO playerChannel;
        [SerializeField] protected int dropExp = 10;
        
        protected BehaviorGraphAgent _btAgent;
        protected EntityRenderer _renderer;

        protected override void AddComponentToDictionary()
        {
            base.AddComponentToDictionary();
            _btAgent = GetComponent<BehaviorGraphAgent>();
            Debug.Assert(_btAgent != null, $"{gameObject.name} does not have an BehaviorGraphAgent");
            
            _renderer = GetCompo<EntityRenderer>();
            Debug.Assert(_renderer != null, $"{gameObject.name} does not have an EntityRenderer");
        }

        protected virtual void Start()
        {
            //do nothing
        }

        public BlackboardVariable<T> GetBlackboardVariable<T>(string key)
        {
            if (_btAgent.GetVariable(key, out BlackboardVariable<T> result))
            {
                return result;
            }
            return default;
        }
    }
}