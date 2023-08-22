using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class SwapTile : VariableTile
{
    public PlayerColor swapColor;

    public override void Landed(PlayerScript playerScript)
    {
        if (playerScript.Color == swapColor)
            return;

        PlayerScript swapPlayer = GameManager.Instance.GetPlayer(swapColor);
        Vector3 position = playerScript.transform.position;
        TileCheck direction = playerScript.NextTile;

        playerScript.targetPos = swapPlayer.transform.position;
        playerScript.NextTile = swapPlayer.NextTile;
        swapPlayer.targetPos = position;
        swapPlayer.NextTile = direction;
    }

#if UNITY_EDITOR
    [MenuItem("Assets/Create/2D/Custom Tiles/Swap Tile")]
    public static void CreateSwapTile()
    {
        string path = EditorUtility.SaveFilePanelInProject($"Save Swap Tile", "New Swap Tile", "Asset", "Save Swap Tile", "Assets/Tile");
        if (path == string.Empty)
            return;

        AssetDatabase.CreateAsset(CreateInstance<SwapTile>(), path);
    }
#endif
}
