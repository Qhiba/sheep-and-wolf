using System.Collections;
using System.Collections.Generic;
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
    
    [SerializeField] private Tilemap groundTilemap;
    [SerializeField] private Tile walkableTile;

    //[SerializeField] private Grid grid;
    
    private NodeData[,] nodes;

    private void Awake()
    {
        RealocateNodeToGrid();
        SearchAndSetNodeNeighbor();
    }

    public Tilemap GetTilemap()
    {
        return groundTilemap;
    }

    private void RealocateNodeToGrid()
    {
        BoundsInt cellBound = groundTilemap.cellBounds;
        Vector3Int offset = new Vector3Int(cellBound.min.x, cellBound.min.y, 0);

        nodes = new NodeData[cellBound.size.x, cellBound.size.y];

        for (int x = 0; x < cellBound.size.x; x++)
        {
            for (int y = 0; y < cellBound.size.y; y++)
            {
                Vector3Int cellPos = new Vector3Int(x, y, 0) + offset;
                TileBase tile = groundTilemap.GetTile(cellPos);

                if (tile == null) continue;

                NodeData node = new NodeData(groundTilemap, new Vector3Int(x, y, 0), cellPos);
                node.isWalkable = (tile == walkableTile ? true : false);
                nodes[x, y] = node;
            }
        }

        Debug.Log(string.Format("Tile Size : {0} -- Node Size : {1}, {2}", cellBound.size, nodes.GetLength(0), nodes.GetLength(1)));
    }

    public Vector3Int GetGridSize()
    {
        return new Vector3Int(nodes.GetLength(0), nodes.GetLength(1), 0);
    }

    public NodeData[,] GetAllNodeData()
    {
        return nodes;
    }

    public NodeData GetNode(int x, int y)
    {
        return nodes[x, y];
    }

    public NodeData GetNode(Vector3 worldPosition)
    {
        Vector3Int offset = new Vector3Int(groundTilemap.cellBounds.min.x, groundTilemap.cellBounds.min.y, 0);
        Vector3Int nodeCell = groundTilemap.WorldToCell(worldPosition);
        Vector3Int gridPos = nodeCell - offset;

        return GetNode(gridPos.x, gridPos.y);
    }

    private void SearchAndSetNodeNeighbor()
    {
        for (int x = 0; x < nodes.GetLength(0); x++)
        {
            for (int y = 0; y < nodes.GetLength(1); y++)
            {
                SearchAndSetNodeNeighbor(nodes[x, y]);
            }
        }
    }

    private void SearchAndSetNodeNeighbor(NodeData node)
    {
        Vector3Int nodeGridPos = node.GetGridPos();

        // Left Position
        if (nodeGridPos.x - 1 >= 0)
        {
            node.SetNeighborNodes(nodes[nodeGridPos.x - 1, nodeGridPos.y]);

            // Left Down Position
            if (nodeGridPos.y - 1 >= 0)
            {
                node.SetNeighborNodes(nodes[nodeGridPos.x - 1, nodeGridPos.y - 1]);
            }

            // Left Up Position
            if (nodeGridPos.y + 1 < nodes.GetLength(1))
            {
                node.SetNeighborNodes(nodes[nodeGridPos.x - 1, nodeGridPos.y + 1]);
            }
        }

        // Left Position
        if (nodeGridPos.x + 1 < nodes.GetLength(0))
        {
            node.SetNeighborNodes(nodes[nodeGridPos.x + 1, nodeGridPos.y]);

            // Left Down Position
            if (nodeGridPos.y - 1 >= 0)
            {
                node.SetNeighborNodes(nodes[nodeGridPos.x + 1, nodeGridPos.y - 1]);
            }

            // Left Up Position
            if (nodeGridPos.y + 1 < nodes.GetLength(1))
            {
                node.SetNeighborNodes(nodes[nodeGridPos.x + 1, nodeGridPos.y + 1]);
            }
        }

        // Down Position
        if (nodeGridPos.y - 1 >= 0)
        {
            node.SetNeighborNodes(nodes[nodeGridPos.x, nodeGridPos.y - 1]);
        }

        // Up Position
        if (nodeGridPos.y + 1 < nodes.GetLength(1))
        {
            node.SetNeighborNodes(nodes[nodeGridPos.x, nodeGridPos.y + 1]);
        }
    }
}