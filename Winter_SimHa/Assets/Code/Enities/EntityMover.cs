using Code.Core.StatSystem;
using NUnit.Framework.Constraints;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Code.Entities
{
    public class EntityMover : MonoBehaviour, IEntityComponent, IAfterInit
    {
        public UnityEvent<Vector2> OnMove;

        [SerializeField] private StatSO moveSpeedStat, jumpPowerStat;

        [Header("Collision detection")]
        [SerializeField] private Transform groundCheckTrm;
        [SerializeField] private Transform wallCheckTrm;
        [SerializeField] private float groundCheckDistance, groundBoxWidth, wallCheckDistance;
        [SerializeField] private LayerMask whatIsGround;



        #region Member field

        private float _movementX;
        private float _moveSpeed = 6f;
        private float _moveSpeedMultiplier;
        private float _jumpPower;

        private Rigidbody2D _rbCompo;
        private EntityStat _statCompo;

        #endregion


        public bool CanManualMove { get; set; } = true;

        #region Init section

        public void Initialize(Entity entity)
        {
            _rbCompo = entity.GetComponent<Rigidbody2D>();
            _statCompo = entity.GetCompo<EntityStat>();
            _moveSpeedMultiplier = 1f;
        }

        public void AfterInit()
        {
            _statCompo.GetStat(moveSpeedStat).OnValueChange += HandleMoveSpeedChange;
            _statCompo.GetStat(jumpPowerStat).OnValueChange += HandleJumpPowerChange;

            _moveSpeed = _statCompo.GetStat(moveSpeedStat).Value;
            _jumpPower = _statCompo.GetStat(jumpPowerStat).Value;
        }

        private void OnDestroy()
        {
            _statCompo.GetStat(moveSpeedStat).OnValueChange -= HandleMoveSpeedChange;
            _statCompo.GetStat(jumpPowerStat).OnValueChange -= HandleJumpPowerChange;
        }

        #endregion

        public void Jump() => AddForceToEntity(new Vector2(0, _jumpPower));

        public void AddForceToEntity(Vector2 force)
            => _rbCompo.AddForce(force, ForceMode2D.Impulse);

        public void SetMoveSpeedMultiplier(float value)
            => _moveSpeedMultiplier = value;

        private void HandleMoveSpeedChange(StatSO stat, float current, float previous)
            => _moveSpeed = current;

        private void HandleJumpPowerChange(StatSO stat, float current, float previous)
            => _jumpPower = current;

        private void FixedUpdate()
        {
            if (CanManualMove)
                _rbCompo.linearVelocityX = _movementX * _moveSpeed * _moveSpeedMultiplier;

            OnMove?.Invoke(_rbCompo.linearVelocity);
        }

        public void SetMovementX(float xMovement)
        {
            _movementX = xMovement;
        }

        public void StopImmediately(bool isYAxisToo)
        {
            if (isYAxisToo)
                _rbCompo.linearVelocity = Vector2.zero;
            else
                _rbCompo.linearVelocityX = 0;

            _movementX = 0;
        }

        #region Check Collision

        public bool IsGroundDetected()
        {
            float boxHeight = 0.05f;
            Vector2 boxSize = new Vector2(groundBoxWidth, boxHeight);
            return Physics2D.BoxCast(groundCheckTrm.position, boxSize, 0, Vector2.down, groundCheckDistance, whatIsGround);
        }

        public bool IsWallDetected(float facingDirection)
            => Physics2D.Raycast(wallCheckTrm.position, Vector2.right * facingDirection, wallCheckDistance, whatIsGround);

        #endregion

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            if (groundCheckTrm != null)
            {
                Gizmos.DrawWireCube(groundCheckTrm.position - new Vector3(0, groundCheckDistance * 0.5f),
                    new Vector3(groundBoxWidth, groundCheckDistance, 1f));
            }
            if (wallCheckTrm != null)
            {
                Gizmos.DrawLine(wallCheckTrm.position, wallCheckTrm.position + new Vector3(wallCheckDistance, 0));
            }
        }
#endif
    }
}

