using CodeMonkey.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class SlowPathfinding : MonoBehaviour
{
    #region Pathfinding Variables
    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;

    private List<NodeData> openList;
    private HashSet<NodeData> closeList;

    private GridManager gridManager;
    private List<Vector3> vectorPath;
    #endregion

    [SerializeField] float speed;

    private bool isMoving = false;

    private void Start()
    {
        gridManager = GridManager.Instance;
        vectorPath = new List<Vector3>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isMoving)
        {
            Vector3 mouseWorldPosition = UtilsClass.GetMouseWorldPosition();
            FindPath(transform.position, mouseWorldPosition); 
        }
    }

    // After finding a path, get path Vector position and move the character.
    public void OnPathFindingComplete(List<NodeData> path)
    {
        if (path == null) vectorPath = null;

        foreach (NodeData node in path)
        {
            vectorPath.Add(node.GetWorldPosition());
        }

        Debug.Log("Pathfinding Finished...");

        // Move Character
        if (vectorPath != null)
        {
            //Draw path Line
            for (int i = 0; i < vectorPath.Count - 1; i++)
            {
                Debug.DrawLine(vectorPath[i] + GridManager.Instance.GetCellSize() * .5f, vectorPath[i + 1] + GridManager.Instance.GetCellSize() * .5f, Color.green, 100.0f);
            }

            StartCoroutine(MoveAlongPath(vectorPath));
        }
        else
        {
            Debug.Log("No Path Found!");
        }
    }

    IEnumerator MoveAlongPath(List<Vector3> path)
    {
        isMoving = true;
        foreach (Vector3 p in path)
        {
            Vector3 targetPath = p + GridManager.Instance.GetCellSize() * .5f;
            while (transform.position != targetPath)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPath, speed * Time.deltaTime);
                yield return null;
            }
        }

        isMoving = false;
        yield break;
    }

    #region Pathfinding
    public void FindPath(Vector3 startWorldPosition, Vector3 endWorldPosition)
    {
        #nullable enable
        NodeData? startNode = gridManager.GetNode(startWorldPosition);
        NodeData? endNode = gridManager.GetNode(endWorldPosition);
        #nullable disable

        if (startNode == null || endNode == null || !endNode.isWalkable) return;

        Debug.Log("Start Node: " + startNode.GetGridPos() + " End Node: " + endNode.GetGridPos());
        FindPath(startNode.GetGridPos().x, startNode.GetGridPos().y, endNode.GetGridPos().x, endNode.GetGridPos().y, OnPathFindingComplete);
    }

    // Find Path in Slow Mode
    public void FindPath(int startX, int startY, int endX, int endY, Action<List<NodeData>> callback)
    {
        Debug.Log("Pathfinding Start...");
        NodeData startNode = gridManager.GetNode(startX, startY);
        NodeData endNode = gridManager.GetNode(endX, endY);

        openList = new List<NodeData> { startNode };
        closeList = new HashSet<NodeData>();

        SetAllNode();

        StartCoroutine(SearchPossibleNode(startNode, endNode, callback));
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

    IEnumerator SearchPossibleNode(NodeData startNode, NodeData endNode, Action<List<NodeData>> callback)
    {

        //Set Start Node
        startNode.gCost = 0;
        startNode.hCost = CalculateDistanceCost(startNode, endNode);
        startNode.CalculateFCost();
        startNode.nodeColor = Color.green;
        startNode.ShowNodeColor();

        yield return new WaitForSeconds(1);

        //Set End Node Color
        endNode.nodeColor = Color.magenta;
        endNode.ShowNodeColor();

        yield return new WaitForSeconds(1);

        // Search all Node
        while (openList.Count > 0)
        {
            NodeData currentNode = GetLowestFCostNode(openList);
            currentNode.nodeColor = Color.cyan;
            currentNode.ShowNodeColor();

            yield return new WaitForSeconds(1);

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
                Debug.Log(string.Format("Current Node {0} - g-cost = {1} \n" +
                    "Neighbor Node {2} -> g-cost = {3} \n" +
                    "Tentative G Cost = {4} \n" +
                    "---------------------------------", currentNode.GetGridPos(), currentNode.gCost, neighborNode.GetGridPos(), neighborNode.gCost, tentativeGCost));
                if (tentativeGCost >= neighborNode.gCost) continue;
                
                neighborNode.cameFromNode = currentNode;
                neighborNode.gCost = tentativeGCost;
                neighborNode.hCost = CalculateDistanceCost(neighborNode, endNode);
                neighborNode.CalculateFCost();

                if (neighborNode != endNode)
                {
                    neighborNode.nodeColor = Color.yellow;
                    neighborNode.ShowNodeColor();
                }                    

                if (!openList.Contains(neighborNode))
                {
                    openList.Add(neighborNode);
                }

                yield return new WaitForSeconds(1);
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

        return path;
    }
    #endregion
}
