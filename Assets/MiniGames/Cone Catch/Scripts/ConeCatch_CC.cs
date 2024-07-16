using System;
using Character.Movement;
using UnityEngine;

namespace MiniGames.Cone_Catch.Scripts
{
    public class ConeCatch_CC : CharacterMovementController
    {
        protected ConeStack _coneStack;

        protected override void Awake()
        {
            base.Awake();
            _coneStack = GetComponent<ConeStack>();
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();

            HandleChangeConeStackRotation();
        }

        protected virtual void HandleChangeConeStackRotation()
        {
            if (_frameInput.Move.x == 0) return;
            _coneStack.ChangeStackAngle(_frameInput.Move.x > 0);
        }
    }
}