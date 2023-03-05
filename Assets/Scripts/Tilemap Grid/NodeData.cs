using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeData
{
    private int x;
    private int y;

    private int gCost;
    private int hCost;
    private int fCost;

    private bool isWalkable;
    private NodeData cameFromNode;

    public NodeData(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
}
