using UnityEngine;

namespace Pathfinding
{
    public class Node
    {
        public readonly Vector2Int Position;
        public bool Walkable;

        public Node Parent;

        public int GCost;
        public int HCost;

        public int FCost => GCost + HCost;

        public Node(int x, int y, bool walkable = true)
        {
            Position = new Vector2Int(x, y);
            Walkable = walkable;
        }

        public Node(Vector2Int position, bool walkable = true) : this(position.x, position.y, walkable ) { }
    }
}