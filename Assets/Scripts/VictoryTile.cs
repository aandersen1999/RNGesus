using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class VictoryTile : VariableTile
{

    public override void Landed(PlayerScript playerScript)
    {
        MusicManager.Instance.PlaySound(MusicManager.Instance.playerWinSound, 0.25f);
        playerScript.confetti.Play();
        GameManager.Instance.TickGameOver();
    }

#if UNITY_EDITOR
    [MenuItem("Assets/Create/2D/Custom Tiles/Victory Tile")]
    public static void CreateVictoryTile()
    {
        string path = EditorUtility.SaveFilePanelInProject($"Save Victory Tile", "New Victory Tile", "Asset", "Save Victory Tile", "Assets/Tile");
        if (path == string.Empty)
            return;

        AssetDatabase.CreateAsset(CreateInstance<VictoryTile>(), path);
    }
#endif
}
