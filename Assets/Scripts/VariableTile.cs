using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class VariableTile : Tile
{
    public Sprite[] sprites;

    private Dictionary<TileCheck, TileBase> tileNeighbors = new();

    public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject go)
    {
        return true;
    }

    public override void RefreshTile(Vector3Int position, ITilemap tilemap)
    {
        base.RefreshTile(position, tilemap);
    }

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref UnityEngine.Tilemaps.TileData tileData)
    {
        base.GetTileData(position, tilemap, ref tileData);
        GetNeighbors(position, tilemap);

        int x = Mathf.Abs(position.x);
        int y = Mathf.Abs(position.y);

        int index = (x + y) % sprites.Length;

        tileData.sprite = sprites[index];
    }

    public bool HasAdjacentNeighbor(TileCheck check)
    {
        return tileNeighbors[check] != null;
    }

    public TileBase GetNeighborTile(TileCheck check)
    {
        if (tileNeighbors[check] != null)
        {
            return tileNeighbors[check];
        }
        return null;
    }

    public void PrintData()
    {
        for (int i = 0; i < 4; i++)
        {
            TileCheck checker = (TileCheck)i;
            Debug.Log($"{checker}: {HasAdjacentNeighbor(checker)}");
        }
    }

    private void GetNeighbors(Vector3Int location, ITilemap tileMap)
    {
        
        TileBase tile;

        tile = tileMap.GetTile(location + Vector3Int.right);
        tileNeighbors[TileCheck.Right] = tile;
        tile = tileMap.GetTile(location + Vector3Int.left);
        tileNeighbors[TileCheck.Left] = tile;
        tile = tileMap.GetTile(location + Vector3Int.up);
        tileNeighbors[TileCheck.Up] = tile;
        tile = tileMap.GetTile(location + Vector3Int.down);
        tileNeighbors[TileCheck.Down] = tile;
    }

#if UNITY_EDITOR
    [MenuItem("Assets/Create/2D/Custom Tiles/Variable Tile")]
    public static void CreateVariableTile()
    {
        string path = EditorUtility.SaveFilePanelInProject($"Save Variable Tile", "New Variable Tile", "Asset", "Save Variable Tile", "Assets/Tile");
        if (path == string.Empty)
            return;
        
        AssetDatabase.CreateAsset(CreateInstance<VariableTile>(), path);
    }
#endif

    public virtual void Landed(PlayerScript playerScript)
    {
        
    }
}

public enum TileCheck
{
    Up,
    Down,
    Left,
    Right,
}