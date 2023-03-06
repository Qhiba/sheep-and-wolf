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
    //[SerializeField] private Tile unwalkableTile;

    private Grid grid;
    private NodeData[,] nodes;

    // 
    public void Awake()
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

    public void ShowGridLine()
    {
       

        //int width = nodes.GetLength(0);
        //int height = nodes.GetLength(1);

        //for (int x = 0; x < width; x++)
        //{
        //    for (int y = 0; y < height; y++)
        //    {
        //        Debug.DrawLine(
        //            nodes[x, y].GetWorldPosition(), 
        //            nodes[x, y].GetWorldPosition() + new Vector3(groundTile.cellSize.x, 0.0f, 0.0f), 
        //            Color.white, 100f);
        //        Debug.DrawLine(
        //            nodes[x, y].GetWorldPosition(),
        //            nodes[x, y].GetWorldPosition() + new Vector3(0.0f, groundTile.cellSize.y, 0.0f),
        //            Color.white, 100f);
        //    }
        //}
        //Debug.DrawLine(
        //    new Vector3(nodes[0, 0].GetWorldPosition().x, nodes[0, height - 1].GetWorldPosition().y + groundTile.cellSize.y, 0.0f), 
        //    nodes[width - 1, height - 1].GetWorldPosition() + groundTile.cellSize, 
        //    Color.white, 100f);
        //Debug.DrawLine(
        //    new Vector3(nodes[width - 1, 0].GetWorldPosition().x + groundTile.cellSize.x, nodes[0, 0].GetWorldPosition().y, 0.0f),
        //    nodes[width - 1, height - 1].GetWorldPosition() + groundTile.cellSize,
        //    Color.white, 100f);
    }
}