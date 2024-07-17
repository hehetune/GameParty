using System;
using UnityEngine;

namespace MiniGames.Cone_Catch.Scripts
{
    public class Bomb : MonoBehaviour
    {
        [SerializeField] private float _fallSpeed = 3f;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private Prefab _explotionPrefab;

        public void Setup(Sprite sprite)
        {
            _spriteRenderer.sprite = sprite;
        }
        
        private void Update()
        {
            transform.position += Vector3.down * Time.deltaTime * _fallSpeed;
            
            if(transform.position.y < -8f) GetComponent<IPoolObject>().ReturnToPool();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Cone"))
            {
                Cone cone = other.GetComponent<Cone>();
                if (!cone || cone.isPlayerCone || !cone.ConeStack) return;
                
                cone.ConeStack.DamageByBomb();

                PoolManager.Get<PoolObject>(_explotionPrefab, out var explotionGO);
                explotionGO.transform.position = transform.position;
                explotionGO.transform.rotation = Quaternion.identity;
                explotionGO.ReturnToPoolByLifeTime(1f);
                
                GetComponent<PoolObject>().ReturnToPool();
            }
        }
    }
}
