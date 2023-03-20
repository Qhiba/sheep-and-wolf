using CodeMonkey.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowPathfinding : MonoBehaviour
{
    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;

    private List<NodeData> openList;
    private HashSet<NodeData> closeList;

    private GridManager gridManager;

    private void Start()
    {
        gridManager = GridManager.Instance;

    }

    #region Pathfinding
    public List<Vector3> FindPath(Vector3 startWorldPosition, Vector3 endWorldPosition)
    {
        #nullable enable
        NodeData? startNode = gridManager.GetNode(startWorldPosition);
        NodeData? endNode = gridManager.GetNode(endWorldPosition);
        #nullable disable

        if (startNode == null || endNode == null || !endNode.isWalkable) return null;

        Debug.Log("Start Node: " + startNode.GetGridPos() + " End Node: " + endNode.GetGridPos());
        List<NodeData> path = FindPath(startNode.GetGridPos().x, startNode.GetGridPos().y, endNode.GetGridPos().x, endNode.GetGridPos().y);

        if (path == null) return null;

        List<Vector3> vectorPath = new List<Vector3>();
        foreach (NodeData node in path)
        {
            vectorPath.Add(node.GetWorldPosition());
        }

        return vectorPath;
    }

    // Find Path in Slow Mode
    public List<NodeData> FindPath(int startX, int startY, int endX, int endY, Action<List<NodeData>> callback)
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
        startNode.nodeColor = Color.green;

        //Set End Node Color
        endNode.nodeColor = Color.magenta;

        StartCoroutine(SearchPossibleNode(endNode, callback));
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

    IEnumerator SearchPossibleNode(NodeData endNode, Action<List<NodeData>> callback)
    {
        // Search all Node
        while (openList.Count > 0)
        {
            NodeData currentNode = GetLowestFCostNode(openList);

            if (currentNode == endNode)
            {
                List<NodeData> path = CalculatePath(endNode);
                callback(path);
                yield break;
            }

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
                    neighborNode.nodeColor = Color.yellow;

                    if (!openList.Contains(neighborNode))
                    {
                        openList.Add(neighborNode);
                    }
                }

                //yield return new WaitForSeconds(1);
            }

            yield return null;
        }

        callback(null);
        yield break;
    }

    private int CalculateDistanceCost(NodeData currentNode, NodeData endNode)
    {
        int xDistance = Mathf.Abs(currentNode.GetGridPos().x - endNode.GetGridPos().x);
        int yDistance = Mathf.Abs(currentNode.GetGridPos().y - endNode.GetGridPos().y);
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
    #endregion
}
