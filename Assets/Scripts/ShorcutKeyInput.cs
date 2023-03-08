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
                VisualizationManager.Instance.ResetColor();
            }
            else
            {
                VisualizationManager.Instance.VisualizeWalkablePath();
            }

            isWalkableVisualize = !isWalkableVisualize;
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            isGridLineShown = !isGridLineShown;
            VisualizationManager.Instance.ShowGridLine(isGridLineShown);
        }
    }
}
