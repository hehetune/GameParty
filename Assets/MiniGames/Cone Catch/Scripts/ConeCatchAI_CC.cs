using System.Collections;
using Character.Movement;
using UnityEngine;

namespace MiniGames.Cone_Catch.Scripts
{
    public class ConeCatchAI_CC : ConeCatch_CC
    {
        private Cone playerTopCone;

        private Cone targetCone;

        [SerializeField] private ConeSpawner _coneSpawner;

        private bool isCatching = false;

        protected override void Start()
        {
            base.Start();
            playerTopCone = _coneStack.LastCone;
            _coneSpawner.onSpawnConeEvent += OnConeSpawn;
            _coneStack.OnTopConeChange += (cone) => playerTopCone = cone;
        }

        // protected override void OnGameStart()
        // {
        //     base.OnGameStart();
        //     
        // }

        // protected override void OnGameEnd()
        // {
        //     base.OnGameEnd();
        //     _coneSpawner.onSpawnConeEvent -= OnConeSpawn;
        // }

        private void OnConeSpawn(Cone cone)
        {
            if (isCatching) return;
            Debug.LogWarning("OnSpawnCone" + cone.gameObject.name);
            targetCone = cone;

            StartCoroutine(CatchConeCoroutine());
        }

        private IEnumerator CatchConeCoroutine()
        {
            isCatching = true;
            float offsetX = 0.25f;
            while (targetCone.transform.position.y > playerTopCone.transform.position.y)
            {
                if (targetCone.isCollected) break;
                if (Mathf.Abs(targetCone.transform.position.x - playerTopCone.transform.position.x) > offsetX)
                {
                    _frameInput.Move.x = targetCone.transform.position.x - playerTopCone.transform.position.x > 0
                        ? 1
                        : -1;
                }
                else
                {
                    _frameInput.Move.x = 0;
                }

                yield return null;
            }

            isCatching = false;
        }
    }
}