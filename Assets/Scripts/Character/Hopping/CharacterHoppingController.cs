using System.Collections;
using UnityEngine;

namespace Character.Hopping
{
    public abstract class CharacterHoppingController : CharacterController
    {
        public CharacterAnimation characterAnimation;
        
        [SerializeField] protected float jumpDistance = 1.0f;
        [SerializeField] protected float jumpHeight = 1.0f;
        [SerializeField] protected float jumpDuration = 0.5f;
        protected bool isGrounded = true;

        public int prevStepIndex = 0;
        public int curStepIndex = 0;

        public Transform startGroundPos;
        public Vector3 playerStartPos;

        protected Coroutine jumpCoroutine;

        protected void Awake()
        {
            playerStartPos = transform.position;
        }

        protected void Jump(int steps)
        {
            if (!isGrounded) return;
            if (jumpCoroutine != null) return;
            jumpCoroutine = StartCoroutine(JumpCoroutine(steps));
        }

        protected virtual IEnumerator JumpCoroutine(int steps)
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

            jumpCoroutine = null;
        }

        public void PlayRespawnAnimation()
        {
            characterAnimation.BlinkCharacter();
        }
    }
}