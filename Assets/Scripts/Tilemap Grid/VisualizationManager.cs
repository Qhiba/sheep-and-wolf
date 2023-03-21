using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using CodeMonkey.Utils;
using UnityEngine.UI;

public class VisualizationManager : MonoBehaviour
{
    private static VisualizationManager instance = null;
    public static VisualizationManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<VisualizationManager>();
            }
            return instance;
        }
    }

    private NodeData[,] nodes;
    private Tilemap groundTile;

    private LineRenderer gridLine;
    private Color gridLineColor = Color.white;
    private float gridLineWidth = 0.025f;
    private bool isGridLineCreated = false;

    public GameObject textParent;
    public Text slowModeText;

    private void Start()
    {
        nodes = GridManager.Instance.GetAllNodeData();
        groundTile = GridManager.Instance.GetTilemap();
        
        gridLine = GetComponent<LineRenderer>();
    }

    public void VisualizeWalkablePath()
    {
        for (int x = 0; x < nodes.GetLength(0); x++)
        {
            for (int y = 0; y < nodes.GetLength(1); y++)
            {
                Vector3Int cellPos = nodes[x, y].GetCellPosition();
                Color color = nodes[x, y].isWalkable ? Color.green : Color.red;
                groundTile.SetTileFlags(cellPos, TileFlags.None);
                groundTile.SetColor(cellPos, color);
            }
        }
    }

    public void VisualizeFindingPathProgression()
    {
        for (int x = 0; x < nodes.GetLength(0); x++)
        {
            for (int y = 0; y < nodes.GetLength(1); y++)
            {
                nodes[x, y].isShowingColor = true;
            }
        }
    }

    public void ResetColor()
    {
        for (int x = 0; x < nodes.GetLength(0); x++)
        {
            for (int y = 0; y < nodes.GetLength(1); y++)
            {
                nodes[x, y].isShowingColor = false;
                Vector3Int cellPos = nodes[x, y].GetCellPosition();
                groundTile.SetColor(cellPos, Color.white);
            }
        }
    }

    public void ShowGridLine(bool isActive)
    {
        if (!isGridLineCreated)
        {
            CreateGridLine();
        }

        gridLine.enabled = isActive;
    }

    private void CreateGridLine()
    {
        Vector3 cellSize = groundTile.cellSize - Vector3.forward;
        int width = nodes.GetLength(0);
        int height = nodes.GetLength(1);

        gridLine.startColor = gridLineColor;
        gridLine.endColor = gridLineColor;
        gridLine.startWidth = gridLineWidth;
        gridLine.endWidth = gridLineWidth;

        int numLines = 3 * ((width + 1) + (height - 1)); // 1 Line need 3 dots or index.
        gridLine.positionCount = numLines;

        int index = 0;
        Vector3 startPos;
        Vector3 endPos;

        for (int x = 0; x < width; x++)
        {
            startPos = nodes[x, 0].GetWorldPosition() - Vector3.forward;
            endPos = nodes[x, height - 1].GetWorldPosition() + new Vector3(0.0f, cellSize.y, -1.0f);

            gridLine.SetPosition(index++, startPos);
            gridLine.SetPosition(index++, endPos);
            gridLine.SetPosition(index++, startPos);
        }

        startPos = nodes[width - 1, 0].GetWorldPosition() + new Vector3(cellSize.x, 0.0f, -1.0f);
        endPos = nodes[width - 1, height - 1].GetWorldPosition() + cellSize;

        gridLine.SetPosition(index++, startPos);
        gridLine.SetPosition(index++, endPos);

        //Set Line to Top Left Edge of Grid
        gridLine.SetPosition(index++, nodes[0, height - 1].GetWorldPosition() + new Vector3(0.0f, cellSize.y, -1.0f));

        for (int y = height - 1; y > 0; y--)
        {
            startPos = nodes[0, y].GetWorldPosition();
            endPos = nodes[width - 1, y].GetWorldPosition() + new Vector3(cellSize.x, 0.0f, -1.0f);

            gridLine.SetPosition(index++, startPos);
            gridLine.SetPosition(index++, endPos);
            gridLine.SetPosition(index++, startPos);
        }

        isGridLineCreated = true;
    }

    public void ShowAStarVariables(bool isActive)
    {
        foreach (var node in nodes)
        {
            node.ShowAStarVariables(isActive);
        }
    }

    public void ShowSlowModeText(bool isActive)
    {
        slowModeText.text = "Path Finding Currently in Slow Mode.";
        slowModeText.gameObject.SetActive(isActive);
    }
}
