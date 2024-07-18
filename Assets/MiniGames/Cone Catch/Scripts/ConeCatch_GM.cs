using System;
using System.Collections;
using UnityEngine;

namespace MiniGames.Cone_Catch.Scripts
{
    public class ConeCatch_GM: GameManager
    {
        public static ConeCatch_GM Instance;
        
        public Action onGameStart;
        public Action onGameEnd;
        
        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }
        
        private void Start()
        {
            CountdownGameStart();
        }

        private void CountdownGameStart()
        {
            StartCoroutine(CountdownCoroutine(1, () => onGameStart()));
        }

        IEnumerator CountdownCoroutine(int seconds, Action onComplete)
        {
            while (seconds >= 0)
            {
                Debug.Log("on count down: " + seconds--);
                yield return 1f.Wait();
            }

            onComplete();
        }
    }
}