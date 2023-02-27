using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MPathfinding
{
    private MGrid<MPathNode> mGrid;
    private MGrid<MPathNode> xGrid;


    public MPathfinding(int width, int height)
    {
        mGrid = new MGrid<MPathNode>(width, height, 1.0f, Vector3.zero, (MGrid<MPathNode> g, int x, int y) => new MPathNode(g, x, y));
        xGrid = new MGrid<MPathNode>(width, height, 1.0f, Vector3.zero, (MGrid<MPathNode> g, int x, int y) => new MPathNode(g, x, y));

        if (mGrid == xGrid)
        {
            Debug.Log("Maybe the same");
        }
        else
        {
            Debug.Log("not the same");
        }
    }
}
