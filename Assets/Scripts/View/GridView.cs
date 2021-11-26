using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Tilemap))]
public class GridView : MonoBehaviour
{
    [SerializeField] private Texture2D mapTexture = default;
    [SerializeField] private Sprite tileSprite = default;

    private Tilemap tilemap;
    private TileSet tileSet;

    private Pathfinding.Grid grid;
    private Pathfinding.AStar aStar;

    private Vector2Int startPosition = new Vector2Int(-1, -1);
    private Vector2Int endPosition = new Vector2Int(-1, -1);

    private Vector2Int lastTileChanged;

    private List<Pathfinding.Node> path = new List<Pathfinding.Node>();

    private void Awake()
    {
        tilemap = GetComponent<Tilemap>();

        tileSet = new TileSet(tileSprite);

        FillGrid();
        RefreshGrid();
    }

    private void FillGrid()
    {
        grid = new Pathfinding.Grid(mapTexture.width, mapTexture.height);

        foreach (var node in grid.Nodes)
            node.Walkable = mapTexture.GetPixel(node.Position.x, node.Position.y).r > 0.5f;

        aStar = new Pathfinding.AStar(grid);
    }

    private void RefreshGrid()
    {
        foreach (var node in grid.Nodes)
            tilemap.SetTile((Vector3Int)node.Position, node.Walkable ? tileSet.DefaultTile : tileSet.ObstacleTile);

        foreach (var node in path)
            tilemap.SetTile((Vector3Int)node.Position, tileSet.PathTile);

        if (grid.HasNode(startPosition))
            tilemap.SetTile((Vector3Int)startPosition, tileSet.StartTile);

        if (grid.HasNode(endPosition))
            tilemap.SetTile((Vector3Int)endPosition, tileSet.EndTile);

        tilemap.RefreshAllTiles();
    }

    private void Update()
    {
        Vector2Int mousePosition = GetMousePosition();

        bool hasChanged = false;

        if (grid.HasNode(mousePosition))
        {
            hasChanged |= HandleMouseButton(buttonId: 0, mousePosition, ref startPosition, tileSet.StartTile);
            hasChanged |= HandleMouseButton(buttonId: 1, mousePosition, ref endPosition, tileSet.EndTile);
            hasChanged |= TryToggleObstacle(buttonId: 2, mousePosition);
        }

        if (hasChanged)
        {
            if (grid.HasNode(startPosition) && grid.HasNode(endPosition))
                path = aStar.FindPath(startPosition, endPosition);

            RefreshGrid();
        }
    }

    private bool HandleMouseButton(int buttonId, Vector2Int newPosition, ref Vector2Int position, Tile tile)
    {
        if (Input.GetMouseButtonDown(buttonId) && newPosition != position && grid.GetNode(newPosition).Walkable)
        {
            position = newPosition;
            return true;
        }

        return false;
    }

    private bool TryToggleObstacle(int buttonId, Vector2Int newPosition)
    {
        if (Input.GetMouseButton(buttonId) && newPosition != lastTileChanged)
        {
            if (newPosition != startPosition && newPosition != endPosition)
            {
                var node = grid.GetNode(newPosition);
                if (!path.Contains(node))
                {
                    node.Walkable = !node.Walkable;
                    lastTileChanged = newPosition;
                    return true;
                }  
            }
        }

        return false;
    }

    private Vector2Int GetMousePosition()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return new Vector2Int(Mathf.RoundToInt(mousePosition.x), Mathf.RoundToInt(mousePosition.y));
    }
}
