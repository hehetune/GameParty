using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace MiniGames.Cone_Catch.Scripts
{
    public class Cone : MonoBehaviour
    {
        public float fallSpeed = 1f;
        public ConeStack ConeStack;

        [SerializeField] private Collider2D[] _colliders;

        public bool isAttached = false;

        public Transform nextConePosition;

        public Transform targetPosition;

        // private ConstantForce2D _constantForce;
        // private Vector2 _forceDirection;
        //
        public bool isPlayerCone = false;

        // private void Awake()
        // {
        //     _constantForce = GetComponent<ConstantForce2D>();
        //     _forceDirection = Vector2.down;
        // }

        private void OnEnable()
        {
            Reset();
        }

        private void Reset()
        {
            isAttached = false;
            SetConeCollidersEnable(true);
            if (!isPlayerCone)
            {
                ConeStack = null;
                targetPosition = null;
            }
        }

        private void Update()
        {
            if (ConeStack != null)
            {
                // float distance = Vector3.Distance(transform.position, targetPosition.position);
                // if (distance < 0.05f) 
                // else
                // {
                transform.position =
                    Vector3.MoveTowards(transform.position, targetPosition.position, 100 * Time.deltaTime);
                // }
            }
            else
            {
                transform.position =
                    new Vector3(transform.position.x, transform.position.y - Time.deltaTime * fallSpeed);
                if (transform.position.y <= -8f) gameObject.GetComponent<PoolObject>().ReturnToPool();
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!isAttached && ConeStack != null && other.gameObject.CompareTag("ConeBottom"))
            {
                Cone cone = other.gameObject.GetComponentInParent<Cone>();
                if (cone.isAttached) return;
                isAttached = true;
                ConeStack.AttachCone(cone);
                SetConeCollidersEnable();
            }
        }

        public void SetConeCollidersEnable(bool enable = false)
        {
            foreach (var col in _colliders)
            {
                col.enabled = enable;
            }
        }
    }
}