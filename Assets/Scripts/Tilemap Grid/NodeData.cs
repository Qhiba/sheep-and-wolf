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

    private TextMesh gCostText;
    private TextMesh hCostText;
    private TextMesh fCostText;

    public NodeData(Tilemap tilemap, Vector3Int gridPos, Vector3Int cellPos)
    {
        this.tilemap = tilemap;
        this.gridPos = gridPos;
        this.cellPos = cellPos;
        this.neighborNodes = new List<NodeData>();

        fCostText = new TextMesh();
        gCostText = new TextMesh();
        hCostText = new TextMesh();

        fCostText = UtilsClass.CreateWorldText("F", GameObject.Find("NodePath").transform, GetWorldPosition() + tilemap.cellSize * 0.5f, 0.2f, 20, Color.white, TextAnchor.MiddleCenter);
        gCostText = UtilsClass.CreateWorldText("G", GameObject.Find("NodePath").transform, GetWorldPosition() + tilemap.cellSize * 0.80f, 0.2f, 10, Color.blue, TextAnchor.MiddleCenter);
        hCostText = UtilsClass.CreateWorldText("H", GameObject.Find("NodePath").transform, GetWorldPosition() + tilemap.cellSize * 0.20f, 0.2f, 10, Color.red, TextAnchor.MiddleCenter);
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
            gCostText.text = gCost.ToString();
            hCostText.text = hCost.ToString();
            fCostText.text = fCost.ToString();
        }
    }
}