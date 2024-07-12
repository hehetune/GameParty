using System.Collections;
using UnityEngine;

namespace MiniGames.Cone_Catch.Scripts
{
    public class ConeSpawner : MonoBehaviour
    {
        public Prefab ConePrefab;

        public float spawnY;
        public float minSpawnX, maxSpawnX;

        public float minSpawnDelay, maxSpawnDelay;

        private bool shouldSpawn = true;

        private void Start()
        {
            SpawnCones();
        }

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

                yield return Random.Range(minSpawnDelay, maxSpawnDelay).Wait();
            }
        }
    }
}