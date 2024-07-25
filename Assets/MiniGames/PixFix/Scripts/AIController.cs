using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MiniGames.PixFix.Scripts
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] private HandCursor _cursor;
        [SerializeField] private Transform _tiles;

        private List<Tile> _unfixTiles = new List<Tile>();

        private void Awake()
        {
            foreach (Transform tileTransform in _tiles)
            {
                Tile tile = tileTransform.GetComponent<Tile>();
                _unfixTiles.Add(tile);
            }
        }

        private void Start()
        {
            StartCoroutine(ProcessTilesSequentially());
        }

        IEnumerator ProcessTilesSequentially()
        {
            Debug.Log("Starting ProcessTilesSequentially");

            yield return 0.25f.Wait();

            while (_unfixTiles.Count > 0)
            {
                Tile targetTile = _unfixTiles[Random.Range(0, _unfixTiles.Count)];
                _unfixTiles.Remove(targetTile);

                Debug.Log("Selected tile: " + targetTile.name + " | Remaining tiles: " + _unfixTiles.Count);

                yield return MoveCursorAndInteractWithTile(targetTile);
            }

            Debug.Log("Finished processing all tiles");
        }

        IEnumerator MoveCursorAndInteractWithTile(Tile targetTile)
        {
            Debug.Log("Moving cursor to tile: " + targetTile.name);

            Vector2 direction = targetTile.transform.position - _cursor.transform.position;
            direction = direction.normalized;
            _cursor.UpdateFrameInput(direction.x, direction.y, false, false, false);

            float distance = Vector2.Distance(_cursor.transform.position, targetTile.transform.position);

            while (distance > 0.5f)
            {
                yield return null;
                distance = Vector2.Distance(_cursor.transform.position, targetTile.transform.position);
            }

            Debug.Log("Cursor reached tile: " + targetTile.name);

            yield return 0.1f.Wait();

            _cursor.UpdateFrameInput(0f, 0f, false, false, true);

            Debug.Log("Interacting with tile: " + targetTile.name);

            yield return 0.25f.Wait();

            while (targetTile.tileRect.transform.eulerAngles.z % 360 > 1)
            {
                bool left = targetTile.tileRect.transform.eulerAngles.z % 360 > 180;
                _cursor.UpdateFrameInput(0f, 0f, left, !left, false);

                Debug.Log("Adjusting tile: " + targetTile.name + " | Rotating to the " + (left ? "left" : "right"));

                yield return 0.75f.Wait();
            }

            _cursor.UpdateFrameInput(0f, 0f, false, false, true);

            Debug.Log("Finished interacting with tile: " + targetTile.name);

            yield return 0.2f.Wait();
        }
    }
}