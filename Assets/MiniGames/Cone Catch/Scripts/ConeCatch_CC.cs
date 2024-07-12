using System;
using UnityEngine;
using CharacterController = Character.CharacterController;

namespace MiniGames.Cone_Catch.Scripts
{
    public class ConeCatch_CC : CharacterController
    {
        protected Rigidbody2D _rb;
        private CapsuleCollider2D _col;

        protected bool wasFacingLeft = false;
        
        protected FrameInput _frameInput;
        protected Vector2 _frameVelocity;

        private void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
        }
        
        private bool _jumpToConsume;
        
        private void FixedUpdate()
        {
            CheckCollisions();

            HandleJump();
            HandleDirection();
            HandleGravity();
            
            ApplyMovement();
        }
        
        private void CheckCollisions()
        {
            Physics2D.queriesStartInColliders = false;

            // Ground and Ceiling
            bool groundHit = Physics2D.CapsuleCast(_col.bounds.center, _col.size, _col.direction, 0, Vector2.down, _stats.GrounderDistance, ~_stats.PlayerLayer);
            bool ceilingHit = Physics2D.CapsuleCast(_col.bounds.center, _col.size, _col.direction, 0, Vector2.up, _stats.GrounderDistance, ~_stats.PlayerLayer);

            // Hit a Ceiling
            if (ceilingHit) _frameVelocity.y = Mathf.Min(0, _frameVelocity.y);

            // Landed on the Ground
            if (!_grounded && groundHit)
            {
                _grounded = true;
                _coyoteUsable = true;
                _bufferedJumpUsable = true;
                _endedJumpEarly = false;
                GroundedChanged?.Invoke(true, Mathf.Abs(_frameVelocity.y));
            }
            // Left the Ground
            else if (_grounded && !groundHit)
            {
                _grounded = false;
                _frameLeftGrounded = _time;
                GroundedChanged?.Invoke(false, 0);
            }

            Physics2D.queriesStartInColliders = _cachedQueryStartInColliders;
        }

        private void HandleJump()
        {
            if (_grounded) ExecuteJump();

            _jumpToConsume = false;
        }
        private void ExecuteJump()
        {
            _frameVelocity.y = _stats.JumpPower;
            Jumped?.Invoke();
        }
        
        private void HandleDirection()
        {
            if (_frameInput.Move.x == 0)
            {
                var deceleration = _grounded ? _stats.GroundDeceleration : _stats.AirDeceleration;
                _frameVelocity.x = Mathf.MoveTowards(_frameVelocity.x, 0, deceleration * Time.fixedDeltaTime);
            }
            else
            {
                _frameVelocity.x = Mathf.MoveTowards(_frameVelocity.x, _frameInput.Move.x * _stats.MaxSpeed, _stats.Acceleration * Time.fixedDeltaTime);
            }
        }
        
        private void HandleGravity()
        {
            if (_grounded && _frameVelocity.y <= 0f)
            {
                _frameVelocity.y = _stats.GroundingForce;
            }
            else
            {
                var inAirGravity = _stats.FallAcceleration;
                if (_endedJumpEarly && _frameVelocity.y > 0) inAirGravity *= _stats.JumpEndEarlyGravityModifier;
                _frameVelocity.y = Mathf.MoveTowards(_frameVelocity.y, -_stats.MaxFallSpeed, inAirGravity * Time.fixedDeltaTime);
            }
        }
        
        private void ApplyMovement() => _rb.velocity = _frameVelocity;
        private void FixedUpdate()
        {
            float hehe = Input.GetAxisRaw("Horizontal");
            if (hehe != 0) wasFacingLeft = hehe < 0;
            characterAnimation.animator.SetFloat($"faceLeft", wasFacingLeft ? 1 : 0);
            characterAnimation.animator.SetBool($"isMoving", hehe != 0);
            characterAnimation.animator.SetFloat($"vy", _rb.velocity.y);
        }
        
    }
    
    public struct FrameInput
    {
        public bool JumpDown;
        public bool JumpHeld;
        public Vector2 Move;
    }

    public interface IPlayerController
    {
        public event Action<bool, float> GroundedChanged;

        public event Action Jumped;
        public Vector2 FrameInput { get; }
    }
}
