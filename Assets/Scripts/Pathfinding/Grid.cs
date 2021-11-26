using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
    public class Grid
    {
        public readonly Vector2Int Size;

        public int Width => Size.x;
        public int Height => Size.y;

        public readonly Node[,] Nodes;

        public Grid(int width, int height)
        {
            Size = new Vector2Int(width, height);
            Nodes = new Node[width, height];

            for (int x = 0; x < Width; x++)
                for (int y = 0; y < Height; y++)
                    Nodes[x, y] = new Node(x, y);
        }

        public Grid(Vector2Int size) : this(size.x, size.y) { }

        public Node GetNode(Vector2Int position)
        {
            return Nodes[position.x, position.y];
        }

        public List<Node> GetNeighbors(Node currentNode)
        {
            List<Node> neighbors = new List<Node>();

            for (int x = Mathf.Max(currentNode.Position.x - 1, 0); x <= Mathf.Min(currentNode.Position.x + 1, Width - 1); x++)
            {
                for (int y = Mathf.Max(currentNode.Position.y - 1, 0); y <= Mathf.Min(currentNode.Position.y + 1, Height - 1); y++)
                {
                    var neighbor = Nodes[x, y];
                    if (neighbor != currentNode)
                        neighbors.Add(neighbor);
                }
            }  

            return neighbors;
        }

        public int GetDistance(Vector2Int positionA, Vector2Int positionB)
        {
            int deltaX = Mathf.Abs(positionA.x - positionB.x);
            int deltaY = Mathf.Abs(positionA.y - positionB.y);

            int pathLength = Mathf.Max(deltaX, deltaY);
            int diagonals = Mathf.Min(deltaX, deltaY);
            return pathLength * 10 + diagonals * 4; // straight lines = 10, diagonals = 14 { ~sqrt(2) * 10 }
        }

        public bool HasNode(Vector2Int position)
        {
            return 0 <= position.x && position.x < Width
                && 0 <= position.y && position.y < Height;
        }

        public bool IsDiagonal(Node nodeA, Node nodeB)
        {
            return nodeA.Position.x != nodeB.Position.x
                && nodeA.Position.y != nodeB.Position.y;
        }

        public bool IsDiagonalWalkable(Node nodeA, Node nodeB)
        {
            return Nodes[nodeA.Position.x, nodeB.Position.y].Walkable
                || Nodes[nodeB.Position.x, nodeA.Position.y].Walkable;
        }
    }
}
