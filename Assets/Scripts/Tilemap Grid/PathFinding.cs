using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pathfinding
{
    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;

    private HashSet<NodeData> openList;
    private HashSet<NodeData> closeList;

    private GridManager gridManager;

    public Pathfinding()
    {
        gridManager = GridManager.Instance;
    }

    public List<Vector3> FindPath(Vector3 startWorldPosition, Vector3 endWorldPosition)
    {
        NodeData startNode = gridManager.GetNode(startWorldPosition);
        NodeData endNode = gridManager.GetNode(endWorldPosition);

        Debug.Log("Start Node: " + startNode.GetGridPos() + " End Node: " +  endNode.GetGridPos());

        List<NodeData> path = FindPath(startNode.GetGridPos().x, startNode.GetGridPos().y, endNode.GetGridPos().x, endNode.GetGridPos().y);

        if (path == null) return null;

        List<Vector3> vectorPath = new List<Vector3>();
        foreach (NodeData node in path)
        {
            vectorPath.Add(node.GetWorldPosition());
        }
        return vectorPath;
    }
    public List<NodeData> FindPath(int startX, int startY, int endX, int endY)
    {
        Debug.Log("Pathfinding Start...");
        NodeData startNode = gridManager.GetNode(startX, startY);
        NodeData endNode = gridManager.GetNode(endX, endY);

        openList = new HashSet<NodeData> { startNode };
        closeList = new HashSet<NodeData>();

        for (int x = 0; x < gridManager.GetGridSize().x; x++)
        {
            for (int y = 0; y < gridManager.GetGridSize().y; y++)
            {
                NodeData node = gridManager.GetNode(x, y);
                node.gCost = int.MaxValue;
                node.CalculateFCost();
                node.cameFromNode = null;
            }
        }

        startNode.gCost = 0;
        startNode.hCost = CalculateDistanceCost(startNode, endNode);
        startNode.CalculateFCost();

        while (openList.Count > 0)
        {
            NodeData currentNode = GetLowestFCostNode(openList);

            if (currentNode == endNode) return CalculatePath(endNode);

            openList.Remove(currentNode);
            closeList.Add(currentNode);

            foreach (NodeData neighborNode in currentNode.GetNeighborNodes())
            {
                if (closeList.Contains(neighborNode)) continue;
                
                if (!neighborNode.isWalkable)
                {
                    closeList.Add(neighborNode);
                    continue;
                }

                int tentativeGCost = currentNode.gCost + CalculateDistanceCost(currentNode, neighborNode);
                if (tentativeGCost < neighborNode.gCost)
                {
                    neighborNode.cameFromNode = currentNode;
                    neighborNode.gCost = tentativeGCost;
                    neighborNode.hCost = CalculateDistanceCost(neighborNode, endNode);
                    neighborNode.CalculateFCost();

                    if (!openList.Contains(neighborNode))
                    {
                        openList.Add(neighborNode);
                    }
                }
            }
        }

        return null;
    }

    private int CalculateDistanceCost(NodeData startNode, NodeData endNode)
    {
        int xDistance = Mathf.Abs(startNode.GetGridPos().x - endNode.GetGridPos().x);
        int yDistance = Mathf.Abs(startNode.GetGridPos().y - endNode.GetGridPos().y);
        int remaining = Mathf.Abs(xDistance - yDistance);
        return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;
    }

    private NodeData GetLowestFCostNode(HashSet<NodeData> openList)
    {
        NodeData lowestFCostNode = openList.First<NodeData>();
        foreach (NodeData node in openList)
        {
            if (node.fCost < lowestFCostNode.fCost)
            {
                lowestFCostNode = node;
            }
        }
        return lowestFCostNode;
    }

    private List<NodeData> CalculatePath(NodeData endNode)
    {
        List<NodeData> path = new List<NodeData>();
        path.Add(endNode);

        NodeData currentNode = endNode;

        while (currentNode.cameFromNode != null)
        {
            path.Add(currentNode.cameFromNode);
            currentNode = currentNode.cameFromNode;
        }

        path.Reverse();

        Debug.Log("Pathfinding Finished...");

        return path;
    }
}
