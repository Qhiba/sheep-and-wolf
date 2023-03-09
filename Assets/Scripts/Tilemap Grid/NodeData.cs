using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using CodeMonkey.Utils;
using UnityEditor.Experimental.GraphView;

public class NodeData
{
    private Tilemap tilemap;
    private Vector3Int gridPos;
    private Vector3Int cellPos;

    public int gCost;
    public int hCost;
    public int fCost;

    public bool isWalkable;
    public NodeData cameFromNode;

    private List<NodeData> neighborNodes;

    private TextMesh debugText;

    public NodeData(Tilemap tilemap, Vector3Int gridPos, Vector3Int cellPos)
    {
        this.tilemap = tilemap;
        this.gridPos = gridPos;
        this.cellPos = cellPos;
        this.neighborNodes = new List<NodeData>();

        debugText = new TextMesh();
        debugText = UtilsClass.CreateWorldText(" ", GameObject.Find("NodePath").transform, GetWorldPosition() + tilemap.cellSize * 0.5f, 0.2f, 20, Color.white, TextAnchor.MiddleCenter);
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
        if (fCost < int.MaxValue)
        {
            debugText.text = fCost.ToString();
        }
    }
}