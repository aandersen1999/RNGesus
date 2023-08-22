using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class DeathTile : VariableTile
{
    public bool neutral = false;
    public PlayerColor playerColor;

#if UNITY_EDITOR
    [MenuItem("Assets/Create/2D/Custom Tiles/Death Tile")]
    public static void CreateDeathTile()
    {
        string path = EditorUtility.SaveFilePanelInProject("Save Death Tile", "New Death Tile", "Asset", "Save Death Tile", "Assets");
        if (path == string.Empty)
            return;

        AssetDatabase.CreateAsset(CreateInstance<DeathTile>(), path);
    }
#endif

    public override void Landed(PlayerScript playerScript)
    {
        if (neutral || playerScript.Color == playerColor)
        {
            GameObject dummy = Instantiate(GameManager.Instance.dummyPrefab, playerScript.gameObject.transform.GetChild(0).transform.position, transform.rotation);
            dummy.GetComponent<DummyPlayerCode>().renderer.color = playerScript.spriteColor;

            playerScript.gameObject.SetActive(false);

            MusicManager.Instance.PlaySound(MusicManager.Instance.playerDeathSound, 0.25f);
            GameManager.Instance.deadPlayers++;
        }
    }
}

