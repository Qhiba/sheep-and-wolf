using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ShorcutKeyInput : MonoBehaviour
{
    private bool isWalkableVisualize = false;
    private bool isGridLineShown = false;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            if (isWalkableVisualize)
            {
                GridManager.Instance.ResetColor();
            }
            else
            {
                GridManager.Instance.VisualizeWalkablePath();
            }

            isWalkableVisualize = !isWalkableVisualize;
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            isGridLineShown = !isGridLineShown;
            GridManager.Instance.ShowGridLine(isGridLineShown);
        }
    }
}
