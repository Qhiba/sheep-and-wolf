using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using UnityEditor;

public class EnemyController : MonoBehaviour
{
    [SerializeField] float speed;

    private Pathfinding pathfinding;

    private bool isMoving = false;

    // Start is called before the first frame update
    void Start()
    {
        pathfinding = new Pathfinding();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (isMoving)
            {
                StopAllCoroutines();
            }

            Vector3 mouseWorldPosition = UtilsClass.GetMouseWorldPosition();
            List<Vector3> path = pathfinding.FindPath(transform.position, mouseWorldPosition);
            if (path != null)
            {
                //Draw path Line
                for (int i = 0; i < path.Count - 1; i++)
                {

                    Debug.DrawLine(path[i] + GridManager.Instance.GetCellSize() * .5f, path[i + 1] + GridManager.Instance.GetCellSize() * .5f, Color.green, 100.0f);
                }

                StartCoroutine(MoveAlongPath(path));   
            }
            else
            {
                Debug.Log("No Path Found!");
            }
        }
    }

    IEnumerator MoveAlongPath(List<Vector3> path)
    {
        isMoving = true;
        for (int i = 1; i < path.Count; i++)
        {
            Vector3 targetPath = path[i] + GridManager.Instance.GetCellSize() * .5f;
            while (transform.position != targetPath)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPath, speed * Time.deltaTime);
                yield return null;
            }
        }

        isMoving = false;
        yield break;
    }
}
