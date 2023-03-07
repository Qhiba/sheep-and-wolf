using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour
{
    private static GridManager instance;
    public static GridManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GridManager>();
            }
            return instance;
        }
    }
    
    [SerializeField] private Tilemap groundTile;
    [SerializeField] private Tile walkableTile;

    //[SerializeField] private Grid grid;
    
    private NodeData[,] nodes;

    private LineRenderer gridLine;
    private Color gridLineColor = Color.white;
    private float gridLineWidth = 0.025f;

    private void Awake()
    {
        BoundsInt cellBound = groundTile.cellBounds;
        Vector3Int offset = new Vector3Int(Mathf.Abs(cellBound.min.x), Mathf.Abs(cellBound.min.y), 0);

        nodes = new NodeData[cellBound.size.x, cellBound.size.y];

        Debug.Log(groundTile.cellSize);

        for (int x = 0; x < cellBound.size.x; x++)
        {
            for (int y = 0; y < cellBound.size.y; y++)
            {
                Vector3Int cellPos = new Vector3Int(x, y, 0) - offset;
                TileBase tile = groundTile.GetTile(cellPos);

                if (tile == null) continue;

                NodeData node = new NodeData(groundTile, x, y, cellPos);
                node.SetWalkable(tile == walkableTile ?  true : false);
                nodes[x, y] = node;
            }
        }

        Debug.Log(string.Format("Tile Size : {0} -- Node Size : {1}, {2}", cellBound.size, nodes.GetLength(0), nodes.GetLength(1)));
        groundTile.RefreshAllTiles();
    }

    private void Start()
    {
        gridLine = GetComponent<LineRenderer>();
        CreateGridLine();
    }

    public void VisualizeWalkablePath()
    {
        for (int x = 0; x < nodes.GetLength(0); x++)
        {
            for (int y = 0; y < nodes.GetLength(1); y++)
            {
                Vector3Int cellPos = nodes[x, y].GetCellPosition();
                Color color = nodes[x, y].GetWalkable() ? Color.green : Color.red;
                groundTile.SetTileFlags(cellPos, TileFlags.None);
                groundTile.SetColor(cellPos, color);
            }
        }        
    }

    public void ResetColor()
    {
        for (int x = 0; x < nodes.GetLength(0); x++)
        {
            for (int y = 0; y < nodes.GetLength(1); y++)
            {
                Vector3Int cellPos = nodes[x, y].GetCellPosition();
                groundTile.SetColor(cellPos, Color.white);
            }
        }
    }

    public void ShowGridLine(bool isActive)
    {
        gridLine.enabled = isActive;
    }

    public void CreateGridLine()
    {
        Vector3 cellSize = groundTile.cellSize;
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
            startPos = nodes[x, 0].GetWorldPosition();
            endPos = nodes[x, height - 1].GetWorldPosition() + new Vector3(0.0f, cellSize.y, 0.0f);

            gridLine.SetPosition(index++, startPos);
            gridLine.SetPosition(index++, endPos);
            gridLine.SetPosition(index++, startPos);
        }

        startPos = nodes[width - 1, 0].GetWorldPosition() + new Vector3(cellSize.x, 0.0f, 0.0f);
        endPos = nodes[width - 1, height - 1].GetWorldPosition() + cellSize;

        gridLine.SetPosition(index++, startPos);
        gridLine.SetPosition(index++, endPos);

        //Set Line to Top Left Edge of Grid
        gridLine.SetPosition(index++, nodes[0, height - 1].GetWorldPosition() + new Vector3(0.0f, cellSize.y, 0.0f));

        for (int y = height - 1; y > 0; y--)
        {
            startPos = nodes[0, y].GetWorldPosition();
            endPos = nodes[width - 1, y].GetWorldPosition() + new Vector3(cellSize.x, 0.0f, 0.0f);

            gridLine.SetPosition(index++, startPos);
            gridLine.SetPosition(index++, endPos);
            gridLine.SetPosition(index++, startPos);
        }
    }
}