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
            if (Input.GetKey(KeyCode.UpArrow)) y = 1;
            if (Input.GetKey(KeyCode.DownArrow)) y = -1;
            if (Input.GetKey(KeyCode.LeftArrow)) x = -1;
            if (Input.GetKey(KeyCode.RightArrow)) x = 1;

            _cursor.UpdateFrameInput(x, y);
        }
    }
}