using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class NodeData
{
    private Tilemap tilemap;
    private Vector3Int gridPos;
    private Vector3Int cellPos;

    private int? gCost = null;
    private int? hCost = null;
    private int? fCost = null;

    private bool isWalkable;
    private NodeData cameFromNode;

    private List<NodeData> neighborNodes;

    public NodeData(Tilemap tilemap, Vector3Int gridPos, Vector3Int cellPos)
    {
        this.tilemap = tilemap;
        this.gridPos = gridPos;
        this.cellPos = cellPos;
        this.neighborNodes = new List<NodeData>();
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

    public Vector3Int GetGridPos()
    {
        return gridPos;
    }

    public List<NodeData> GetNeighborNodes()
    {
        return neighborNodes;
    }

    public void SetNeighborNodes(NodeData neighbor)
    {
        neighborNodes.Add(neighbor);
    }

    public void CalculateFCost()
    {
        fCost = gCost + hCost;
    }
}