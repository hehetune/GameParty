using System;
using System.Collections;
using UnityEngine;
using CharacterController = Character.CharacterController;

namespace MiniGames._100_Yard_Hop.Scripts
{
    public class YardHop_CC : CharacterController
    {
        [SerializeField] private float jumpDistance = 1.0f;
        [SerializeField] private float jumpHeight = 1.0f;
        [SerializeField] private float jumpDuration = 0.5f;
        private bool isGrounded = true;

        public int prevStepIndex = 0;
        public int curStepIndex = 0;

        public Transform startGroundPos;
        public Vector3 playerStartPos;

        private Coroutine jumpCoroutine;

        private void Awake()
        {
            playerStartPos = transform.position;
        }

        private void Start()
        {
            YardHop_GM.Instance.onGameStart += OnGameStart;
            YardHop_GM.Instance.onGameEnd += OnGameEnd;
        }

        protected override void OnGameEnd()
        {
            base.OnGameEnd();
            if (jumpCoroutine != null) StopCoroutine(jumpCoroutine);
        }

        protected void Jump(int steps)
        {
            if (!isGrounded) return;
            if (jumpCoroutine != null) return;
            jumpCoroutine = StartCoroutine(JumpCoroutine(steps));
        }

        IEnumerator JumpCoroutine(int steps)
        {
            characterAnimation.PlayJumpAnimation();
            isGrounded = false;
            Vector3 startPosition = transform.position;
            Vector3 targetPosition = startPosition + new Vector3(steps * jumpDistance, 0, 0);

            float elapsedTime = 0f;

            while (elapsedTime < jumpDuration)
            {
                float t = elapsedTime / jumpDuration;
                float height = Mathf.Sin(Mathf.PI * t) * jumpHeight;
                transform.position = Vector3.Lerp(startPosition, targetPosition, t) + Vector3.up * height;

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            transform.position = targetPosition;
            isGrounded = true;
            characterAnimation.PlayIdleAnimation();

            prevStepIndex = curStepIndex;
            curStepIndex += steps;

            if (!YardHop_GM.Instance.CheckStepValid(curStepIndex - 1))
            {
                YardHop_GM.Instance.CharacterDie(this);
            }

            jumpCoroutine = null;
        }

        public void PlayRespawnAnimation()
        {
            characterAnimation.BlinkCharacter();
        }
    }
}