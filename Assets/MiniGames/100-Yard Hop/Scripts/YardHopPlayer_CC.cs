using UnityEngine;

namespace MiniGames._100_Yard_Hop.Scripts
{
    public class YardHopPlayer_CC : YardHop_CC
    {
        private bool canInput = false;

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
            if (Input.GetKeyDown(KeyCode.F))
            {
                Jump(1);
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                Jump(2);
            }
        }
    }
}