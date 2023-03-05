using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour
{
    public Tilemap groundTile;
    public Vector2Int gridSize;

    private NodeData node;

    public void Start()
    {
        var bounds = groundTile.localBounds;
        Debug.Log(bounds);
    }

}
