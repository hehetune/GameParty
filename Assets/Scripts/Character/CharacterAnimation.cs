using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace Character
{
    public class CharacterAnimation : MonoBehaviour
    {
        public Animator animator;
        public SpriteRenderer spriteRenderer;

        [SerializeField] private int blinkCount = 3;
        [SerializeField] private float blinkDuration = 0.5f;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void PlayJumpAnimation()
        {
            animator.SetTrigger("Jump");
        }

        public void PlayIdleAnimation()
        {
            animator.SetTrigger("Idle");
        }

        public void BlinkCharacter()
        {
            if (blinkCharacterCoroutine != null) StopCoroutine(blinkCharacterCoroutine);
            blinkCharacterCoroutine = StartCoroutine(BlinkCharacterCoroutine());
        }

        private Coroutine blinkCharacterCoroutine;

        private IEnumerator BlinkCharacterCoroutine()
        {
            for (int i = 0; i < blinkCount; i++)
            {
                // Fade out
                yield return StartCoroutine(FadeTo(0.0f, blinkDuration / 2));
                // Fade in
                yield return StartCoroutine(FadeTo(1.0f, blinkDuration / 2));
            }

            blinkCharacterCoroutine = null;
        }

        private IEnumerator FadeTo(float targetAlpha, float duration)
        {
            Color color = spriteRenderer.color;

            float startAlpha = color.a;
            float time = 0;

            while (time < duration)
            {
                time += Time.deltaTime;
                float alpha = Mathf.Lerp(startAlpha, targetAlpha, time / duration);
                color.a = alpha;
                spriteRenderer.color = color;
                yield return null;
            }

            color.a = targetAlpha;
            spriteRenderer.color = color;
        }
    }
}