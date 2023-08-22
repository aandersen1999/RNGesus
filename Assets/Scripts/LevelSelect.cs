using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelect : MonoBehaviour
{
    public int levelID = 0;

    public void OnClick()
    {
        StartCoroutine(GameManager.Instance.LoadLevelOnNumber(levelID));
    }
}
