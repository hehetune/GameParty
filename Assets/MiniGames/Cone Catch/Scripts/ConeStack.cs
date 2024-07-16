using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace MiniGames.Cone_Catch.Scripts
{
    public class ConeStack : MonoBehaviour
    {
        [SerializeField] private List<Cone> cones = new();
        [SerializeField] private float rotationChangeSpeed = 10f;
        [SerializeField] private float rotationChangeSpeedMax = 30f;
        [SerializeField] private float rotationChangeSpeedModifier = 1f;

        [SerializeField] private float maxAngle = 5;

        [SerializeField] private float angleIncreaseSpeed = 1f;
        [SerializeField] private float angleIncreaseSpeedMin = 0.1f;
        [SerializeField] private float angleIncreaseModifier = 0.1f;
        [SerializeField] private float angleDecreaseSpeed = 0.3f;
        [SerializeField] private float angleDecreaseSpeedMin = 0.05f;
        [SerializeField] private float angleDecreaseModifier = 0.05f;

        private float currentAngle = 0f;

        private float resetAngleTimer = 0f;
        private float resetAngleTime = 0.25f;

        public void AttachCone(Cone cone)
        {
            cone.ConeStack = this;
            Cone previousCone = cones[^1];
            if (previousCone)
            {
                cone.transform.SetParent(previousCone.transform);
                cone.transform.localPosition = previousCone.nextConeLocalPosition;
                cone.transform.localRotation = Quaternion.Euler(0f, 0f, currentAngle);
            }
            cones.Add(cone);

            angleIncreaseSpeed = Mathf.Max(angleIncreaseSpeedMin, angleIncreaseSpeed - angleIncreaseModifier);
            angleDecreaseSpeed = Mathf.Max(angleDecreaseSpeedMin, angleDecreaseSpeed - angleDecreaseModifier);
            rotationChangeSpeed = Mathf.Min(rotationChangeSpeedMax, rotationChangeSpeed + rotationChangeSpeedModifier);
        }

        private void Update()
        {
            if (cones.Count > 1)
            {
                for (int i = 1; i < cones.Count; i++)
                {
                    var Cone = cones.ElementAt(i);

                    Cone.transform.localRotation = Quaternion.RotateTowards(Cone.transform.localRotation,
                        Quaternion.Euler(0f, 0f, currentAngle), rotationChangeSpeed * Time.deltaTime);
                }
            }

            resetAngleTimer = (resetAngleTimer - Time.deltaTime) < 0 ? 0 : (resetAngleTimer - Time.deltaTime);

            if (resetAngleTimer <= 0)
            {
                currentAngle = Mathf.Lerp(currentAngle, 0f, angleDecreaseSpeed * Time.deltaTime);
            }
        }

        public void ChangeStackAngle(bool increase)
        {
            resetAngleTimer = resetAngleTime;
            currentAngle = Mathf.Clamp(currentAngle + angleIncreaseSpeed * (increase ? 1 : -1), -maxAngle, maxAngle);
        }
    }
}