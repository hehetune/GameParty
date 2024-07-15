using System;
using UnityEngine;

namespace MiniGames.Cone_Catch.Scripts
{
    public class ConeCatchPlayer_CC : ConeCatch_CC
    {
        protected override void Awake()
        {
            base.Awake();
            canInput = true;
        }
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
    }
}