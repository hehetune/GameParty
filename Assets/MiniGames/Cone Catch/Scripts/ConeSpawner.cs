using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MiniGames.Cone_Catch.Scripts
{
    public class ConeSpawner : MonoBehaviour
    {
        [SerializeField] private List<Sprite> coneSprites;
        public Prefab ConePrefab;

        public float spawnY;
        public float minSpawnX, maxSpawnX;

        public float minSpawnDelay, maxSpawnDelay;

        private bool shouldSpawn = true;

        public Action<Cone> onSpawnConeEvent;

        private void Awake()
        {
            ConeCatch_GM.Instance.onGameStart += SpawnCones;
        }

        // private void Start()
        // {
        //     SpawnCones();
        // }

        private void SpawnCones()
        {
            StartCoroutine(SpawnConesCoroutine());
        }

        IEnumerator SpawnConesCoroutine()
        {
            while (shouldSpawn)
            {
                PoolManager.Get<PoolObject>(ConePrefab, out var coneGO);
                coneGO.transform.position = new Vector3(Random.Range(minSpawnX, maxSpawnX), spawnY, 0);
                coneGO.transform.rotation = Quaternion.identity;
                coneGO.GetComponent<Cone>().Setup(coneSprites[Random.Range(0, coneSprites.Count)]);

                yield return null;

                onSpawnConeEvent?.Invoke(coneGO.GetComponent<Cone>());

                yield return Random.Range(minSpawnDelay, maxSpawnDelay).Wait();
            }
        }
    }
}