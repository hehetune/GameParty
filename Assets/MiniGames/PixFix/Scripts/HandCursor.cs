using System;
using UnityEngine;

namespace MiniGames.PixFix.Scripts
{
    public class HandCursor : MonoBehaviour
    {
        private FrameInput _frameInput;

        [SerializeField] private float _moveSpeed = 10f;

        private Rigidbody2D _rigidbody;

        private Tile _currentTile;

        [SerializeField] private LayerMask tileLayerMask;

        private void Awake()
        {
            this._rigidbody = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            HandleMoveCursor();
        }

        private void HandleMoveCursor()
        {
            if (this._currentTile) _rigidbody.velocity = Vector2.zero;
            else _rigidbody.velocity = _frameInput.move * _moveSpeed;
        }

        public void UpdateFrameInput(float x, float y, bool left, bool right, bool select)
        {
            this._frameInput.move.x = x;
            this._frameInput.move.y = y;
            this._frameInput.left = left;
            this._frameInput.right = right;
            this._frameInput.select = select;

            this.HandleCursorClick();
            this.HandleRotateTile();
        }

        private void HandleRotateTile()
        {
            if (!this._currentTile) return;
            if (this._frameInput.left) this._currentTile.Rotate(true);
            else if (this._frameInput.right) this._currentTile.Rotate(false);
        }

        private void HandleCursorClick()
        {
            if (!this._frameInput.select) return;

            if (this._currentTile)
            {
                if (this._currentTile.isRotating) return;
                this._currentTile.SelectThis(false);
                this._currentTile = null;
                return;
            }

            Collider2D[] hitColliders = Physics2D.OverlapPointAll(transform.position, tileLayerMask);

            foreach (Collider2D hitCollider in hitColliders)
            {
                Debug.Log("Hit " + hitCollider.gameObject.name);
                Tile tile = hitCollider.transform.GetComponent<Tile>();
                if (tile.canBeSelect) SelectTile(tile);
            }

            // Ray ray = new Ray(transform.position, Vector3.back);
            // RaycastHit2D hit;
            //
            // if (Physics2D.RaycastAll(ray, out hit, 10f, tileLayerMask))
            // {
            //     Debug.Log("Hit " + hit.collider.gameObject.name);
            //     Tile tile = hit.transform.GetComponent<Tile>();
            //     if (tile.canBeSelect) SelectTile(tile);
            // }
        }

        private void SelectTile(Tile tile)
        {
            this._currentTile = tile;
            tile.SelectThis(true);
        }
    }

    [Serializable]
    public struct FrameInput
    {
        public bool left;
        public bool right;
        public bool select;
        public Vector2 move;
    }
}