using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MPathNode
{
    private MGrid<MPathNode> mGrid;
    private int x;
    private int y;

    public int gCost;
    public int hCost;
    public int fCost;

    public MPathNode cameFromNode;

    public MPathNode(MGrid<MPathNode> mGrid, int x, int y)
    {
        this.mGrid = mGrid;
        this.x = x;
        this.y = y;
    }

    public override string ToString()
    {
        Debug.Log(mGrid);
        return x + ":" + y;
    }
}
