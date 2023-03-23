using CodeMonkey.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding
{
    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;

    private List<NodeData> openList;
    private HashSet<NodeData> closeList;

    private GridManager gridManager;

    public Pathfinding()
    {
        gridManager = GridManager.Instance;
    }

    public List<Vector3> FindPath(Vector3 startWorldPosition, Vector3 endWorldPosition)
    {
        float pathfindingStartTime = Time.realtimeSinceStartup;

        #nullable enable
        NodeData? startNode = gridManager.GetNode(startWorldPosition);
        NodeData? endNode = gridManager.GetNode(endWorldPosition);
        #nullable disable

        //if (startNode == null || endNode == null || !endNode.isWalkable) return null;

        Debug.Log("Start Node: " + startNode.GetGridPos() + " End Node: " + endNode.GetGridPos());
        List<NodeData> path = FindPath(startNode.GetGridPos().x, startNode.GetGridPos().y, endNode.GetGridPos().x, endNode.GetGridPos().y);

        float pathfindingEndTime = Time.realtimeSinceStartup;
        float pathfindingProcessingTime = pathfindingEndTime - pathfindingStartTime;
        Debug.Log("Waktu proses A* = " + pathfindingProcessingTime);

        if (path == null) return null;

        List<Vector3> vectorPath = new List<Vector3>();
        foreach (NodeData node in path)
        {
            vectorPath.Add(node.GetWorldPosition());
        }

        return vectorPath;
    }

    // Find Path yang Lancar
    public List<NodeData> FindPath(int startX, int startY, int endX, int endY)
    {
        Debug.Log("Pathfinding Start...");
        NodeData startNode = gridManager.GetNode(startX, startY);
        NodeData endNode = gridManager.GetNode(endX, endY);

        openList = new List<NodeData> { startNode };
        closeList = new HashSet<NodeData>();

        SetAllNode();

        //Set Start Node
        startNode.gCost = 0;
        startNode.hCost = CalculateDistanceCost(startNode, endNode);
        startNode.CalculateFCost();

        return SearchPossibleNode(endNode);
    }

    private void SetAllNode()
    {
        for (int x = 0; x < gridManager.GetGridSize().x; x++)
        {
            for (int y = 0; y < gridManager.GetGridSize().y; y++)
            {
                NodeData node = gridManager.GetNode(x, y);
                node.SetInitialValue();
            }
        }
    }

    private List<NodeData> SearchPossibleNode(NodeData endNode)
    {
        // Search all Node
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
                if (tentativeGCost >= neighborNode.gCost) continue;
                
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

        return null;
    }

    private int CalculateDistanceCost(NodeData nodeA, NodeData nodeB)
    {
        int xDistance = Mathf.Abs(nodeA.GetGridPos().x - nodeB.GetGridPos().x);
        int yDistance = Mathf.Abs(nodeA.GetGridPos().y - nodeB.GetGridPos().y);
        int remaining = Mathf.Abs(xDistance - yDistance);
        return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;
    }

    private NodeData GetLowestFCostNode(List<NodeData> openList)
    {
        NodeData lowestFCostNode = openList[0];
        foreach (NodeData node in openList)
        {
            if (node.fCost < lowestFCostNode.fCost)
            {
                lowestFCostNode = node;
            }
        }
        return lowestFCostNode;
    }

    // Create the path
    private List<NodeData> CalculatePath(NodeData endNode)
    {
        List<NodeData> path = new List<NodeData> { endNode };

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
