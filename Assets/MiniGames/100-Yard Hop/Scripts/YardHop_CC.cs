using System;
using System.Collections;
using Character.Hopping;
using UnityEngine;

namespace MiniGames._100_Yard_Hop.Scripts
{
    public class YardHop_CC : CharacterHoppingController
    {
        protected override void OnGameStart()
        {
        }

        protected override void OnGameEnd()
        {
            if (jumpCoroutine != null) StopCoroutine(jumpCoroutine);
        }

        protected override IEnumerator JumpCoroutine(int steps)
        {
            yield return base.JumpCoroutine(steps);

            if (!YardHop_GM.Instance.CheckStepValid(curStepIndex - 1))
            {
                YardHop_GM.Instance.CharacterDie(this);
            }
        }
    }
}