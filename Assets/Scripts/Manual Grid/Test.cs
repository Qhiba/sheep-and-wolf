using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using CodeMonkey;
using UnityEditor.Experimental.GraphView;

public class Test : MonoBehaviour
{
    private MPathfinding pathfinding;

    private void Start()
    {
        pathfinding = new MPathfinding(10, 10);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorldPosition = UtilsClass.GetMouseWorldPosition();
            pathfinding.GetGrid().GetXY(mouseWorldPosition, out int x, out int y);
            List<MPathNode> path = pathfinding.FindPath(0, 0, x, y);
            Debug.Log(path.Count);

            if (path != null)
            {
                for (int i = 0; i < path.Count - 1; i++)
                {
                    Debug.DrawLine(new Vector3(path[i].x, path[i].y) + Vector3.one * 0.5f, new Vector3(path[i + 1].x, path[i + 1].y) + Vector3.one * 0.5f, Color.green, 100f);
                }
            }
        }
    }
}
