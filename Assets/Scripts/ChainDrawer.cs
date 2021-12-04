using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class ChainDrawer : MonoBehaviour
{
    private PathCreator pathCreator;
    private List<Vector3> pathSamplePositions;
    private LineRenderer lineRenderer;

    void Start()
    {
        pathCreator = GetComponent<PathCreator>();
        lineRenderer = GetComponent<LineRenderer>();
        pathSamplePositions = new List<Vector3>();

        for (float i = 0.0f; i <= pathCreator.path.length; i += 0.1f)
        {
            pathSamplePositions.Add(pathCreator.path.GetPointAtDistance(i));
        }
        lineRenderer.positionCount = pathSamplePositions.Count;
        lineRenderer.SetPositions(pathSamplePositions.ToArray());
    }

}
