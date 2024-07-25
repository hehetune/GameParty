using UnityEngine;

namespace MiniGames.PixFix.Scripts
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private HandCursor _cursor;

        private void Update()
        {
            UpdateInput();
        }

        private void UpdateInput()
        {
            int x = 0, y = 0;
            bool l = false, r = false, space = false;
            if (Input.GetKey(KeyCode.UpArrow)) y = 1;
            if (Input.GetKey(KeyCode.DownArrow)) y = -1;
            if (Input.GetKey(KeyCode.LeftArrow)) x = -1;
            if (Input.GetKey(KeyCode.RightArrow)) x = 1;

            if (Input.GetKeyDown(KeyCode.Space)) space = true;

            if (Input.GetKeyDown(KeyCode.Q)) l = true;
            if (Input.GetKeyDown(KeyCode.E)) r = true;

            _cursor.UpdateFrameInput(x, y, l, r, space);
        }
    }
}