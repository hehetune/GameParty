using UnityEngine;

namespace AlgorithmLearning.Tutor._Scripts
{
    public class Node
    {
        public bool walkable;
        public Vector3 worldPosition;

        public Node(bool walkable, Vector3 worldPosition)
        {
            this.walkable = walkable;
            this.worldPosition = worldPosition;
        }
    }
}
