using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class NodeData
{
    private Tilemap tilemap;
    private int x;
    private int y;
    private Vector3Int cellPos;

    private int gCost;
    private int hCost;
    private int fCost;

    private bool isWalkable;
    private NodeData cameFromNode;

    public NodeData(Tilemap tilemap, int x, int y, Vector3Int cellPos)
    {
        this.tilemap = tilemap;
        this.x = x;
        this.y = y;
        this.cellPos = cellPos;
    }

    public void SetWalkable(bool isWalkable)
    {
        this.isWalkable = isWalkable;
    }

    public bool GetWalkable()
    {
        return this.isWalkable;
    }

    public Vector3Int GetCellPosition()
    {
        return this.cellPos;
    }

    public Vector3 GetWorldPosition()
    {
        return tilemap.CellToWorld(cellPos);
    }
}
