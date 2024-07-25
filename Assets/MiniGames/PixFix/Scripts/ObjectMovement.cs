using System;
using UnityEngine;

namespace MiniGames.PixFix.Scripts
{
    public class ObjectMovement : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private Rigidbody2D _spriteRigidbody;
        [SerializeField] private Transform _model;

        [SerializeField] private float _rotateSpeed = 10f;
        [SerializeField] private float _moveSpeed = 10f;

        private Vector3 _moveDirection;

        private void Start()
        {
            this._moveDirection = transform.right;
            this._rigidbody.velocity = this._moveDirection * this._moveSpeed;
            this._spriteRigidbody.angularVelocity = this._rotateSpeed;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("PixFixWall"))
            {
                CalculateBounce(other.contacts[0].normal);
            }
        }

        private void BounceToDirection(Vector3 direction)
        {
            this._moveDirection = direction;
            this._rigidbody.velocity = this._moveDirection * this._moveSpeed;
        }

        private void CalculateBounce(Vector3 normalVector)
        {
            Vector3 bounceDirection = Vector3.Reflect(this._moveDirection, normalVector);
            this.BounceToDirection(bounceDirection.normalized);
        }
    }
}