using System;
using UnityEngine;

namespace MiniGames.Cone_Catch.Scripts
{
    public class Cone : MonoBehaviour
    {
        public float fallSpeed = 1f;
        public ConeStack ConeStack;

        public bool isAttached = false;

        public Vector3 nextConeLocalPosition;
        
        private void Update()
        {
            if (ConeStack != null) return;
            transform.position =
                new Vector3(transform.position.x, transform.position.y - Time.deltaTime * fallSpeed);
            if (transform.position.y <= -3.5f) GetComponent<PoolObject>().ReturnToPool();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!isAttached && ConeStack!= null && other.gameObject.CompareTag("ConeBottom"))
            {
                isAttached = true;
                Cone cone = other.gameObject.GetComponentInParent<Cone>();
                if (cone.isAttached) return;
                ConeStack.AttachCone(cone);
            }
        }
    }
}