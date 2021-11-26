using UnityEngine;
using UnityEngine.Tilemaps;

public class TileSet
{
    public Tile DefaultTile;
    public Tile ObstacleTile;
    public Tile StartTile;
    public Tile EndTile;
    public Tile PathTile;

    public TileSet(Sprite tileSprite)
    {
        InitializeTile(ref DefaultTile, Color.white, tileSprite);
        InitializeTile(ref ObstacleTile, Color.black, tileSprite);
        InitializeTile(ref StartTile, Color.green, tileSprite);
        InitializeTile(ref EndTile, Color.red, tileSprite);
        InitializeTile(ref PathTile, Color.blue, tileSprite);
    }

    private void InitializeTile(ref Tile tile, Color color, Sprite tileSprite)
    {
        tile = ScriptableObject.CreateInstance<Tile>();
        tile.sprite = tileSprite;
        tile.color = color;
    }
}
