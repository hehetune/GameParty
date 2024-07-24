using System;
using UnityEngine;

namespace MiniGames.PixFix.Scripts
{
    public class HandCursor : MonoBehaviour
    {
        private Vector2 _moveInput;

        [SerializeField] private float _moveSpeed = 10f;

        private Rigidbody2D _rigidbody;

        private void Awake()
        {
            this._rigidbody = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            _rigidbody.velocity = _moveInput * _moveSpeed;
        }

        public void UpdateFrameInput(int x, int y)
        {
            this._moveInput.x = x;
            this._moveInput.y = y;
        }
    }
}