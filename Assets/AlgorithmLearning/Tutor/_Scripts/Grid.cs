using UnityEngine;

namespace AlgorithmLearning.Tutor._Scripts
{
    public class Grid : MonoBehaviour
    {
        public LayerMask unwalkableMask;
        public Vector2 gridWorldSize;
        public float nodeRadius;
        private Node[,] _grid;

        private float nodeDiameter;
    }
}