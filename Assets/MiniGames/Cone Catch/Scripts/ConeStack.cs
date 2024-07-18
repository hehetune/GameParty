using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace MiniGames.Cone_Catch.Scripts
{
    public class ConeStack : MonoBehaviour
    {
        [SerializeField] private List<Cone> cones = new();
        [SerializeField] private float rotationChangeSpeed = 5f;
        [SerializeField] private float rotationChangeSpeedMax = 20f;
        [SerializeField] private float rotationChangeSpeedModifier = 1f;

        [SerializeField] private float maxAngle = 5;

        [SerializeField] private float angleIncreaseSpeed = 0.5f;
        [SerializeField] private float angleIncreaseSpeedMin = 0.1f;
        [SerializeField] private float angleIncreaseModifier = 0.1f;
        [SerializeField] private float angleDecreaseSpeed = 0.6f;
        [SerializeField] private float angleDecreaseSpeedMin = 0.05f;
        [SerializeField] private float angleDecreaseModifier = 0.05f;

        private float currentAngle = 0f;

        private float resetAngleTimer = 0f;
        private float resetAngleTime = 0.25f;

        public Action<Cone> OnTopConeChange;

        public void AttachCone(Cone cone)
        {
            cone.ConeStack = this;
            Cone previousCone = cones[^1];
            cone.targetPosition = previousCone.nextConePosition;
            cone.transform.position = previousCone.nextConePosition.position;
            cone.transform.rotation = Quaternion.Euler(0f, 0f, currentAngle * cones.Count);
            cone.UpdateSortingOrder(cones.Count);
            cone.isCollected = true;
            cones.Add(cone);

            angleIncreaseSpeed = Mathf.Max(angleIncreaseSpeedMin, angleIncreaseSpeed - angleIncreaseModifier);
            angleDecreaseSpeed = Mathf.Max(angleDecreaseSpeedMin, angleDecreaseSpeed - angleDecreaseModifier);
            rotationChangeSpeed = Mathf.Min(rotationChangeSpeedMax, rotationChangeSpeed + rotationChangeSpeedModifier);
            
            OnTopConeChange?.Invoke(cone);
        }

        private void Update()
        {
            if (cones.Count > 1)
            {
                for (int i = 1; i < cones.Count; i++)
                {
                    var Cone = cones[i];
                    Cone.transform.rotation = Quaternion.RotateTowards(Cone.transform.rotation,
                        Quaternion.Euler(0f, 0f, currentAngle * i), rotationChangeSpeed * Time.deltaTime);
                }
            }

            resetAngleTimer = (resetAngleTimer - Time.deltaTime) < 0 ? 0 : (resetAngleTimer - Time.deltaTime);

            if (resetAngleTimer <= 0)
            {
                currentAngle = Mathf.Lerp(currentAngle, 0f, angleDecreaseSpeed * Time.deltaTime);
            }
        }

        public void HandleConesFallDelay()
        {
            Debug.LogWarning("Handle cones fall delay");
            for(int i = 1; i < cones.Count; i++)
            {
                cones[i].DelayFollowTarget();
            }
        }

        // IEnumerator ConeStackJumpEffectCoroutine()
        // {
        //     foreach (Cone cone in cones)
        //     {
        //         yield return ConeJumpEffectCoroutine(cone.transform);
        //         yield return 0.2f.Wait();
        //     }
        // }
        //
        // IEnumerator ConeJumpEffectCoroutine(Transform cone)
        // {
        //     float elapsedTime = 0.25f;
        //     float timer = 0f;
        //     float distance = 0.5f;
        //
        //     while (timer < elapsedTime)
        //     {
        //         cone.localPosition = new Vector3(0f, Mathf.Lerp(0f, distance, timer), 0f);
        //         timer += Time.deltaTime;
        //         yield return null;
        //     }
        // }

        public void ChangeStackAngle(bool increase)
        {
            resetAngleTimer = resetAngleTime;
            currentAngle = Mathf.Clamp(currentAngle + angleIncreaseSpeed * (increase ? 1 : -1), -maxAngle, maxAngle);
        }

        public void DamageByBomb()
        {
            int i = cones.Count - 1;
            int conesDestroyed = 0;
            while (i >= 1 && conesDestroyed <= 3)
            {
                cones[i].GetComponent<PoolObject>().ReturnToPool();
                cones.RemoveAt(i);
                conesDestroyed++;
                i--;
            }

            cones[^1].SetConeCollidersEnable(true);
            cones[^1].isConnected = false;
            
            OnTopConeChange?.Invoke(cones[^1]);
        }

        public Cone LastCone => cones[^1];
    }
}