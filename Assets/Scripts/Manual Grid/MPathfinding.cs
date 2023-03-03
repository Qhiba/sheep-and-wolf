using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MPathfinding
{
    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;

    private MGrid<MPathNode> mGrid;
    private List<MPathNode> openList;
    private List<MPathNode> closedList;

    public MPathfinding(int width, int height)
    {
        mGrid = new MGrid<MPathNode>(width, height, 1.0f, Vector3.zero, (MGrid<MPathNode> g, int x, int y) => new MPathNode(g, x, y));
    }

    public MGrid<MPathNode> GetGrid() { return mGrid; }

    public List<MPathNode> FindPath(int startX, int startY, int endX, int endY)
    {
        MPathNode startNode = mGrid.GetGridObject(startX, startY);
        MPathNode endNode = mGrid.GetGridObject(endX, endY);

        openList = new List<MPathNode> { startNode };
        closedList = new List<MPathNode>();

        for (int x = 0; x < mGrid.GetWidth(); x++)
        {
            for(int y = 0; y < mGrid.GetHeight(); y++)
            {
                MPathNode mPathNode = mGrid.GetGridObject(x, y);
                mPathNode.gCost = int.MaxValue;
                mPathNode.CalculateFCost();
                mPathNode.cameFromNode = null;
            }
        }

        startNode.gCost = 0;
        startNode.hCost = CalculateDistanceCost(startNode, endNode);
        startNode.CalculateFCost();

        while(openList.Count > 0)
        {
            MPathNode currentNode = GetLowestFCostNode(openList);
            if (currentNode == endNode) 
            {
                return CalculatePath(endNode);
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            foreach (MPathNode neighbourNode in GetNeighbourList(currentNode))
            {
                if (closedList.Contains(neighbourNode)) continue;
                if (!neighbourNode.isWalkable)
                {
                    closedList.Add(neighbourNode);
                    continue;
                }

                int tentativeGCost = currentNode.gCost + CalculateDistanceCost(currentNode, neighbourNode);
                if (tentativeGCost < neighbourNode.gCost)
                {
                    neighbourNode.cameFromNode = currentNode;
                    neighbourNode.gCost = tentativeGCost;
                    neighbourNode.hCost = CalculateDistanceCost(neighbourNode, endNode);
                    neighbourNode.CalculateFCost();

                    if (!openList.Contains(neighbourNode))
                    {
                        openList.Add(neighbourNode);
                    }
                }
            }
        }

        // Out of nodes on the openList
        return null;
    }

    private List<MPathNode> GetNeighbourList(MPathNode currentNode)
    {
        List<MPathNode> neighbourList = new List<MPathNode>();

        if (currentNode.x - 1 >= 0)
        {
            // Left
            neighbourList.Add(GetNode(currentNode.x - 1, currentNode.y));
            // Left Down
            if (currentNode.y - 1 >= 0)
            {
                neighbourList.Add(GetNode(currentNode.x - 1, currentNode.y - 1));
            }
            // Left Up
            if (currentNode.y + 1 < mGrid.GetHeight())
            {
                neighbourList.Add(GetNode(currentNode.x - 1, currentNode.y + 1));
            }
        }
        if (currentNode.x + 1 < mGrid.GetWidth())
        {
            // Right
            neighbourList.Add(GetNode(currentNode.x + 1, currentNode.y));
            // Right Down
            if (currentNode.y - 1 >= 0)
            {
                neighbourList.Add(GetNode(currentNode.x + 1, currentNode.y - 1));
            }
            // Right Up
            if (currentNode.y + 1 < mGrid.GetHeight())
            {
                neighbourList.Add(GetNode(currentNode.x + 1, currentNode.y + 1));
            }
        }
        //Down
        if (currentNode.y - 1 >= 0)
        {
            neighbourList.Add(GetNode(currentNode.x, currentNode.y - 1));
        }
        //Up
        if (currentNode.y + 1 < mGrid.GetHeight())
        {
            neighbourList.Add(GetNode(currentNode.x, currentNode.y + 1));
        }

        return neighbourList;
    }

    private List<MPathNode> CalculatePath(MPathNode endNode)
    {
        List<MPathNode> path = new List<MPathNode>();
        path.Add(endNode);
        MPathNode currentNode = endNode;

        while (currentNode.cameFromNode != null)
        {
            path.Add(currentNode.cameFromNode);
            currentNode = currentNode.cameFromNode;
        }

        path.Reverse();
        return path;
    }

    public MPathNode GetNode(int x, int y)
    {
       return mGrid.GetGridObject(x, y);
    }

    private int  CalculateDistanceCost(MPathNode a, MPathNode b) 
    {
        int xDistance = Mathf.Abs(a.x - b.x);
        int yDistance = Mathf.Abs(a.y - b.y);
        int remaining = Mathf.Abs(xDistance - yDistance);
        return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;
    }

    private MPathNode GetLowestFCostNode(List<MPathNode> nodeList)
    {
        MPathNode lowestFCostNode = nodeList[0];
        for (int i = 1; i < nodeList.Count; i++) 
        {
            if (nodeList[i].fCost < lowestFCostNode.fCost)
            {
                lowestFCostNode = nodeList[i];
            }
        }
        return lowestFCostNode;
    }
}
