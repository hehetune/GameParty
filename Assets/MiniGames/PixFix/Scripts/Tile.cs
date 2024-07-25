using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace MiniGames.PixFix.Scripts
{
    public class Tile : MonoBehaviour
    {
        // [SerializeField] private GameObject _border;
        [SerializeField] private Image[] _borders;
        public RectTransform tileRect;
        [SerializeField] private RawImage _tileRawImage;

        [SerializeField] private Color borderNormalColor;
        [SerializeField] private Color borderSelectColor;
        [SerializeField] private Color borderTransparentColor;

        private Coroutine _rotateCoroutine;

        public bool canBeSelect = true;

        public bool isRotating = false;

        private void Awake()
        {
            this.Initialize();
        }

        private void Initialize()
        {
            //correct render texture
            int.TryParse(gameObject.name.Split('_')[1], out int index);
            this._tileRawImage.texture = ResourceManager.TileRenderTextures()[index];
            //random rotate z
            tileRect.eulerAngles = new Vector3(0f, 0f, Random.Range(1, 4) * 90f);
        }

        public void SelectThis(bool select)
        {
            if (select)
            {
                Debug.LogWarning("Select tile " + gameObject.name);
                transform.SetAsLastSibling();
                SetBorderColor(borderSelectColor);
                return;
            }
            Debug.LogWarning("DeSelect tile " + gameObject.name);
            if (CheckIsInTrueOrder())
            {
                SetBorderColor(borderTransparentColor);
                this.canBeSelect = false;
                return;
            }
            
            SetBorderColor(borderNormalColor);
        }

        public void Rotate(bool left)
        {
            if (this._rotateCoroutine != null) return;
            Debug.LogWarning("Rotate tile " + gameObject.name);
            this._rotateCoroutine = StartCoroutine(RotateCoroutine(left));
        }

        IEnumerator RotateCoroutine(bool left)
        {
            isRotating = true;
            float startAngle = tileRect.eulerAngles.z;
            float targetAngle = left ? (startAngle + 90) : (startAngle - 90);

            var startQuaternion = Quaternion.Euler(0f, 0f, startAngle);
            var targetQuaternion = Quaternion.Euler(0f, 0f, targetAngle);

            float elapsedTime = 0.5f;
            float timer = 0f;

            while (timer < elapsedTime)
            {
                timer += Time.deltaTime;
                tileRect.rotation = Quaternion.Slerp(startQuaternion, targetQuaternion, timer / elapsedTime);
                yield return null;
            }

            tileRect.rotation = targetQuaternion;
            isRotating = false;
            this._rotateCoroutine = null;
        }

        private bool CheckIsInTrueOrder()
        {
            return tileRect.eulerAngles.z % 360 < 1;
        }

        private void SetBorderColor(Color color)
        {
            foreach (var border in _borders)
            {
                border.color = color;
            }
        }
    }
}