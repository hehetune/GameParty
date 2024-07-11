using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MiniGames._100_Yard_Hop.Scripts
{
    public class YardHopAI_CC : YardHop_CC
    {
        protected override void OnGameStart()
        {
            base.OnGameStart();
            startJumpCoroutine = StartCoroutine(StartJumpCoroutine());
        }

        protected override void OnGameEnd()
        {
            base.OnGameEnd();
            if (startJumpCoroutine != null) StopCoroutine(startJumpCoroutine);
        }

        private Coroutine startJumpCoroutine;

        IEnumerator StartJumpCoroutine()
        {
            bool[] path = YardHop_GM.Instance.Path;
            while (curStepIndex < 100)
            {
                if (curStepIndex == 100) Jump(1);
                else if (curStepIndex == 99) Jump(2);
                else Jump(path[curStepIndex + 1] ? 2 : 1);

                yield return Random.Range(0.9f, 1.1f).Wait();
            }

            startJumpCoroutine = null;
        }
    }
}