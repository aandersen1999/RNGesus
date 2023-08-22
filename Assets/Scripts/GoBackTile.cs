using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class GoBackTile : VariableTile
{
    public int spacesToGoBack;
    public override void Landed(PlayerScript playerScript)
    {
        if (spacesToGoBack == 0) return;
        playerScript.GoBackSpaces(spacesToGoBack);
    }

#if UNITY_EDITOR
    [MenuItem("Assets/Create/2D/Custom Tiles/GoBack Tile")]
    public static void CreateAdvanceTile()
    {
        string path = EditorUtility.SaveFilePanelInProject($"Save GoBack Tile", "New GoBack Tile", "Asset", "Save GoBack Tile", "Assets/Tile");
        if (path == string.Empty)
            return;

        AssetDatabase.CreateAsset(CreateInstance<GoBackTile>(), path);
    }
#endif
}