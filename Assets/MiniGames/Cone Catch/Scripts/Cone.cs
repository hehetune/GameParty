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

        public bool isConnected = false;
        public bool isCollected = false;

        public Transform nextConePosition;

        public Transform targetPosition;

        // private ConstantForce2D _constantForce;
        // private Vector2 _forceDirection;
        //
        public bool isPlayerCone = false;

        [SerializeField] private SpriteRenderer _spriteRenderer;

        // [SerializeField] private float followSpeed = 100f;
        // [SerializeField] private float minFollowSpeed = 0.1f;
        // [SerializeField] private float maxFollowSpeed = 100f;
        // [SerializeField] private float followSpeedModifier = 0.1f;

        public void Setup(Sprite sprite)
        {
            _spriteRenderer.sprite = sprite;
        }

        public void UpdateSortingOrder(int index)
        {
            _spriteRenderer.sortingOrder = index;
        }

        private void OnEnable()
        {
            Reset();
        }

        private void Reset()
        {
            isConnected = false;
            SetConeCollidersEnable(true);
            if (!isPlayerCone)
            {
                isCollected = false;
                _spriteRenderer.sortingOrder = 0;
                ConeStack = null;
                targetPosition = null;
            }
        }

        // private bool canStopInAir = true;
        // [SerializeField] private float distanceToIncreaseFollowSpeed = 0.75f;
        [SerializeField] private float gravity = 9.81f; // Gia tốc trọng lực

        [SerializeField] private float fallTime = 0f; // Thời gian rơi
        [SerializeField] private float fallDelayTimer = 0f;
        [SerializeField] private float fallDelay = 0.01f;

        private void Update()
        {
            if (isCollected)
            {
                // float distance = Vector3.Distance(transform.position, targetPosition.position);

                Vector3 wantedPosition = targetPosition.position;

                if (targetPosition.position.y >= transform.position.y)
                {
                    fallTime = 0f; // Reset thời gian rơi khi đạt vị trí mục tiêu
                    fallDelayTimer = fallDelay;
                }
                else if (targetPosition.position.y < transform.position.y)
                {
                    if (fallDelayTimer > 0)
                    {
                        fallDelayTimer = Mathf.Max(0f, fallDelayTimer - Time.deltaTime);
                        wantedPosition.y = transform.position.y;
                    }
                    // if (distance < distanceToIncreaseFollowSpeed && canStopInAir)
                    // {
                    //     // fallDelayTimer = Mathf.Max(0f, fallDelayTimer - Time.deltaTime);
                    //     wantedPosition.y = transform.position.y;
                    // }
                    else
                    {
                        // if (canStopInAir) canStopInAir = false;
                        // Tăng thời gian rơi
                        fallTime += Time.deltaTime;

                        // Tính vị trí Y tiếp theo sử dụng công thức rơi tự do
                        float wantedY = transform.position.y - 0.5f * gravity * Mathf.Pow(fallTime, 2);

                        // Đảm bảo không rơi xuống dưới vị trí mục tiêu
                        if (wantedY < targetPosition.position.y)
                        {
                            wantedY = targetPosition.position.y;
                            // fallTime = 0f; // Reset thời gian rơi khi đạt vị trí mục tiêu
                        }

                        wantedPosition.y = wantedY;
                    }
                }

                transform.position = wantedPosition;

                // if (distance >= distanceToIncreaseFollowSpeed && !needToSetMaxFollowSpeed)
                // {
                //     needToSetMaxFollowSpeed = true;
                // }
                //
                // if (needToSetMaxFollowSpeed)
                // {
                //     followSpeed = Mathf.Min(maxFollowSpeed, followSpeed + Time.deltaTime * followSpeedModifier);
                // }
            }
            else
            {
                transform.position =
                    new Vector3(transform.position.x, transform.position.y - Time.deltaTime * fallSpeed);
                if (transform.position.y <= -8f) gameObject.GetComponent<PoolObject>().ReturnToPool();
            }
        }

        public void DelayFollowTarget()
        {
            // fallTime = 0;
            // fallDelayTimer = fallDelay;
            // canStopInAir = true;
            // followSpeed = minFollowSpeed;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!isConnected && isCollected && other.gameObject.CompareTag("ConeBottom"))
            {
                Cone cone = other.gameObject.GetComponentInParent<Cone>();
                if (cone.isCollected) return;
                isConnected = true;
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