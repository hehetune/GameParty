using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MiniGames._100_Yard_Hop.Scripts
{
    public class YardHop_GM : GameManager
    {
        public static YardHop_GM Instance;

        private bool[] m_path = new bool[100];
        public bool[] Path => m_path;
        public float distanceBetweenStep = 1f;


        //Prefabs
        public Prefab waterSplashPrefab;
        public Prefab rockGround;
        // public Prefab playerPrefab;

        public List<YardHop_CC> players = new();

        public Action onGameStart;
        public Action onGameEnd;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;

            GenerateRandomPath();
            GeneratePathUI();
            // SpawnPlayer();
        }

        private void Start()
        {
            CountdownGameStart();
        }

        private void CountdownGameStart()
        {
            StartCoroutine(CountdownCoroutine(3, () => onGameStart()));
        }

        IEnumerator CountdownCoroutine(int seconds, Action onComplete)
        {
            while (seconds >= 0)
            {
                Debug.Log("on count down: " + seconds--);
                yield return 1f.Wait();
            }

            onComplete();
        }

        private void GenerateRandomPath()
        {
            int groundPercent = 50;
            bool isGround;
            for (int i = 0; i < 100; i++)
            {
                isGround = Random.Range(1, 101) > groundPercent;
                if (!isGround) groundPercent = 0;
                else groundPercent = 50;
                m_path[i] = isGround;
            }
        }

        private void GeneratePathUI()
        {
            for (int i = 0; i < 100; i++)
            {
                if (m_path[i])
                {
                    foreach (var player in players)
                    {
                        PoolManager.Get<PoolObject>(rockGround, out var rockGO);
                        Vector3 targetPos = player.startGroundPos.position;
                        targetPos.x += distanceBetweenStep * i;
                        rockGO.transform.position = targetPos;
                        rockGO.transform.rotation = Quaternion.identity;
                    }
                }
            }
        }

        // private void SpawnPlayer()
        // {
        //     PoolManager.Get<PoolObject>(playerPrefab, out var playerGO);
        //     playerGO.transform.position = playerStartPos;
        //     playerGO.transform.rotation = Quaternion.identity;
        // }

        public bool CheckStepValid(int stepIndex)
        {
            if (stepIndex >= 100)
            {
                Debug.Log("Win game");
                onGameEnd();
                return true;
            }
            return m_path[stepIndex];
        }

        public void CharacterDie(YardHop_CC character)
        {
            StartCoroutine(CharacterDieCoroutine(character));
        }

        private IEnumerator CharacterDieCoroutine(YardHop_CC character)
        {
            character.gameObject.SetActive(false);
            SpawnWaterSplashEffect(character.transform.position);
            yield return new WaitForSeconds(1f);
            character.transform.position =
                character.playerStartPos + new Vector3(distanceBetweenStep * character.prevStepIndex, 0, 0);
            character.gameObject.SetActive(true);
            character.PlayRespawnAnimation();
            character.curStepIndex = character.prevStepIndex;
        }

        private void SpawnWaterSplashEffect(Vector3 position)
        {
            PoolManager.Get<PoolObject>(waterSplashPrefab, out var effect);
            effect.transform.position = position;
            effect.transform.rotation = Quaternion.identity;
            effect.ReturnToPoolByLifeTime(0.5f);
        }
    }
}