using System;
using UnityEngine;

namespace MiniGames.Cone_Catch.Scripts
{
    public class Cone : MonoBehaviour
    {
        public float fallSpeed = 1f;
        public ConeStack ConeStack;

        public bool isAttached = false;

        private void Update()
        {
            if (isAttached) return;
            transform.position =
                new Vector3(transform.position.x, transform.position.y - Time.deltaTime * fallSpeed);
            if (transform.position.y <= -3.5f) GetComponent<PoolObject>().ReturnToPool();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Cone"))
            {
                Cone cone = other.gameObject.GetComponent<Cone>();
                if (cone.isAttached) return;
                ConeStack.AttachCone(this);
            }
        }
    }
}