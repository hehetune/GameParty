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
            // this._rigidbody.AddForce(transform.right * _moveSpeed, ForceMode2D.Impulse);
            this._rigidbody.velocity = this._moveDirection * this._moveSpeed;
            this._spriteRigidbody.angularVelocity = this._rotateSpeed;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            Debug.LogWarning("OnTriggerEnter");

            if (other.gameObject.CompareTag("PixFixWall"))
            {
                Debug.LogWarning("OnTriggerEnter PixFixWall");
                // ContactPoint2D[] contacts = new ContactPoint2D[1];
                // other.
                // int contactCount = other.GetContacts(contacts);
                //
                // if (contactCount > 0)
                // {
                CalculateBounce(other.contacts[0].normal);
                // }
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
        }

        private void BounceToDirection(Vector3 direction)
        {
            this._moveDirection = direction;
            this._rigidbody.velocity = this._moveDirection * this._moveSpeed;
        }

        private void CalculateBounce(Vector3 normalVector)
        {
            Debug.LogWarning("normalVector: " + normalVector);
            Debug.LogWarning("velocityVector: " + this._rigidbody.velocity);
            Vector3 bounceDirection = Vector3.Reflect(this._moveDirection, normalVector);
            Debug.LogWarning("bounceDirection: " + bounceDirection);
            this.BounceToDirection(bounceDirection.normalized);
        }
    }
}