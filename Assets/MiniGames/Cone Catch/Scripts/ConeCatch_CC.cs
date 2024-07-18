using System;
using Character.Movement;
using UnityEngine;

namespace MiniGames.Cone_Catch.Scripts
{
    public class ConeCatch_CC : CharacterMovementController
    {
        public ConeStack _coneStack;

        // protected override void Awake()
        // {
        //     base.Awake();
        // }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();

            HandleChangeConeStackRotation();

            HandleConesStackFallDelay();
        }

        private bool coneFallDelaySet = false;

        protected virtual void HandleConesStackFallDelay()
        {
            switch (_frameVelocity.y)
            {
                case >= 0 when !coneFallDelaySet:
                    coneFallDelaySet = true;
                    break;
                case < 0 when coneFallDelaySet:
                    _coneStack.HandleConesFallDelay();
                    coneFallDelaySet = false;
                    break;
            }
        }

        protected virtual void HandleChangeConeStackRotation()
        {
            if (_frameInput.Move.x == 0) return;
            _coneStack.ChangeStackAngle(_frameInput.Move.x > 0);
        }

        // protected virtual void OnJump()
        // {
        //     
        // }
    }
}