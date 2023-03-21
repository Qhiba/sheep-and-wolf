using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShorcutKeyInput : MonoBehaviour
{
    private bool isWalkableVisualize = false;
    private bool isPathfindingProgressionVisualize = false;
    private bool isGridLineShown = false;
    private bool isAStarVariableShown = false;

    private void Start()
    {
        //isWalkableVisualize = false;
        //isGridLineShown = false;
        //isAStarVariableShown = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            isWalkableVisualize = !isWalkableVisualize;

            if (isWalkableVisualize)
            {
                Debug.Log("Enable Walkable Path Visualization");
                VisualizationManager.Instance.VisualizeWalkablePath();
            }
            else
            {
                Debug.Log("Disable Walkable Path Visualization");
                VisualizationManager.Instance.ResetColor();
            }
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            isPathfindingProgressionVisualize = !isPathfindingProgressionVisualize;

            if (isPathfindingProgressionVisualize)
            {
                Debug.Log("Enable Pathfinding Visualization");
                VisualizationManager.Instance.VisualizeFindingPathProgression();
            }
            else
            {
                Debug.Log("Disable Pathfinding Visualization");
                VisualizationManager.Instance.ResetColor();
            }
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            isGridLineShown = !isGridLineShown;
            Debug.Log("Show Grid Line -> " + isGridLineShown);
            VisualizationManager.Instance.ShowGridLine(isGridLineShown);
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            isAStarVariableShown = !isAStarVariableShown;
            Debug.Log("Show A* Variables -> " + isAStarVariableShown);
            VisualizationManager.Instance.ShowAStarVariables(isAStarVariableShown);
        }
    }
}
