using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
    public class AStar
    {
        private Grid grid;
        private List<Node> openNodes;
        private HashSet<Node> closedNodes;

        public AStar(Grid grid)
        {
            this.grid = grid;

            openNodes = new List<Node>();
            closedNodes = new HashSet<Node>();
        }

        public List<Node> FindPath(Vector2Int start, Vector2Int end)
        {
            openNodes.Clear();
            closedNodes.Clear();

            openNodes.Add(grid.GetNode(start));

            while (openNodes.Count > 0)
            {
                var currentNode = GetCheapestNode();
                openNodes.Remove(currentNode);
                closedNodes.Add(currentNode);

                if (currentNode.Position == end)
                    return RetracePath(grid.GetNode(start), grid.GetNode(end));

                foreach (var neighbor in grid.GetNeighbors(currentNode))
                {
                    if (!neighbor.Walkable || closedNodes.Contains(neighbor)) continue;

                    if (grid.IsDiagonal(currentNode, neighbor) && !grid.IsDiagonalWalkable(currentNode, neighbor)) continue;

                    int newMovementCostToNeighbour = currentNode.GCost + grid.GetDistance(currentNode.Position, neighbor.Position);
                    if (newMovementCostToNeighbour < neighbor.GCost || !openNodes.Contains(neighbor))
                    {
                        neighbor.GCost = newMovementCostToNeighbour;
                        neighbor.HCost = grid.GetDistance(neighbor.Position, end);
                        neighbor.Parent = currentNode;

                        if (!openNodes.Contains(neighbor))
                            openNodes.Add(neighbor);
                    }
                }
            }

            return null;
        }

        private Node GetCheapestNode()
        {
            var cheapestNode = openNodes[0];

            for (int i = 1; i < openNodes.Count; i++)
            {
                var node = openNodes[i];

                if (node.FCost < cheapestNode.FCost)
                    cheapestNode = node;
            }

            return cheapestNode;
        }

        private List<Node> RetracePath(Node startNode, Node endNode)
        {
            List<Node> path = new List<Node>();
            Node currentNode = endNode;

            while (currentNode != startNode)
            {
                path.Add(currentNode);
                currentNode = currentNode.Parent;
            }

            path.Reverse();
            return path;
        }
    }
}
