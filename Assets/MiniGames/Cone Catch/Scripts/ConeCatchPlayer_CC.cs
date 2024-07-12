using System;
using UnityEngine;

namespace MiniGames.Cone_Catch.Scripts
{
    public class ConeCatchPlayer_CC : ConeCatch_CC
    {
        private bool canInput = true;
        
        

        protected override void OnGameStart()
        {
            base.OnGameStart();
            canInput = true;
        }

        protected override void OnGameEnd()
        {
            base.OnGameEnd();
            canInput = false;
        }

        
        private void Update()
        {
            HandleInput();
        }

        private void HandleInput()
        {
            if (!canInput) return;

            _frameInput = new FrameInput
            {
                JumpDown = Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.C),
                JumpHeld = Input.GetButton("Jump") || Input.GetKey(KeyCode.C),
                Move = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"))
            };
        }
        
    }
}