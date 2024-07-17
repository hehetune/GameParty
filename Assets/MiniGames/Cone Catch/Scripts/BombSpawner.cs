using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiniGames.Cone_Catch.Scripts
{
    public class BombSpawner : MonoBehaviour
    {
        [SerializeField] private List<Sprite> bombSprites;

        [SerializeField] private Prefab bombPrefab;
        [SerializeField] private Prefab bombSignPrefab;

        private bool canSpawn = false;

        [SerializeField] private float minXSpawnValue;
        [SerializeField] private float maxXSpawnValue;
        [SerializeField] private float ySpawnValue;
        [SerializeField] private float ySignSpawnValue = 0f;

        private void Start()
        {
            StartSpawn();
        }

        public void StartSpawn()
        {
            canSpawn = true;
            StartCoroutine(StartSpawnCoroutine());
        }

        IEnumerator StartSpawnCoroutine()
        {
            yield return 4f.Wait();

            while (canSpawn)
            {
                SpawnBomb();
                yield return 3f.Wait();
            }
        }

        private void SpawnBomb()
        {
            PoolManager.Get<PoolObject>(bombPrefab, out var bombGO);
            bombGO.transform.position = new Vector3(Random.Range(minXSpawnValue, maxXSpawnValue), ySpawnValue, 0);
            bombGO.transform.rotation = Quaternion.identity;
            
            bombGO.GetComponent<Bomb>().Setup(bombSprites[Random.Range(0, bombSprites.Count)]);
            
            PoolManager.Get<PoolObject>(bombSignPrefab, out var signGO);
            signGO.transform.position = new Vector3(bombGO.transform.position.x, ySignSpawnValue, 0);
            signGO.transform.rotation = Quaternion.identity;
            signGO.ReturnToPoolByLifeTime(1f);
        }
    }
}