using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class DirectionChangeTile : VariableTile
{
    public TileCheck newDirection;

    public override void Landed(PlayerScript playerScript)
    {
        playerScript.NextTile = newDirection;
    }

#if UNITY_EDITOR
    [MenuItem("Assets/Create/2D/Custom Tiles/Direction Tile")]
    public static void CreateDirectionTile()
    {
        string path = EditorUtility.SaveFilePanelInProject("Save Direction Tile", "New Direction Tile", "Asset", "Save Direction Tile", "Assets");
        if (path == string.Empty)
            return;

        AssetDatabase.CreateAsset(CreateInstance<DirectionChangeTile>(), path);
    }
#endif
}
