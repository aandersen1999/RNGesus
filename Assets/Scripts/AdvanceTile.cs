using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class AdvanceTile : VariableTile
{
    public int spacesToAdvance;
    public override void Landed(PlayerScript playerScript)
    {
        if(spacesToAdvance == 0) return;
        playerScript.AdvanceSpaces(spacesToAdvance);
    }

#if UNITY_EDITOR
    [MenuItem("Assets/Create/2D/Custom Tiles/Advance Tile")]
    public static void CreateAdvanceTile()
    {
        string path = EditorUtility.SaveFilePanelInProject($"Save Advance Tile", "New Advance Tile", "Asset", "Save Advance Tile", "Assets/Tile");
        if (path == string.Empty)
            return;

        AssetDatabase.CreateAsset(CreateInstance<AdvanceTile>(), path);
    }
#endif
}
