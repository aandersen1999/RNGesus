using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour
{
    private Tilemap map;

    private void Awake()
    {
        map = GetComponent<Tilemap>();

    }

    private void Start()
    {
        GameManager.Instance.Map = map;
    }
}
