using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class EnemyController : MonoBehaviour
{
    bool isMouseControllOn = true;

    private Pathfinding pathfinding;

    // Start is called before the first frame update
    void Start()
    {
        pathfinding = new Pathfinding();
    }

    // Update is called once per frame
    void Update()
    {
        if (isMouseControllOn)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 mouseWorldPosition = UtilsClass.GetMouseWorldPosition();
                List<Vector3> path = pathfinding.FindPath(transform.position, mouseWorldPosition);
                if (path != null)
                {
                    for (int i = 0; i < path.Count - 1; i++)
                    {
                        Debug.DrawLine(path[i] + Vector3.one, path[i + 1] + Vector3.one, Color.green, 100.0f);
                    }
                }
            }
        }
    }
}
