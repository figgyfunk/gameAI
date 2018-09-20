using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class houseCircle : MonoBehaviour {

    public int numSegments;
    public float radius;
    public void DoRenderer()
    {
        LineRenderer lineRenderer = gameObject.GetComponent<LineRenderer>();
        lineRenderer.positionCount = (numSegments + 1);
        lineRenderer.useWorldSpace = false;

        float deltaTheta = (float)(2.0 * Mathf.PI) / numSegments;
        float theta = 0f;

        for (int i = 0; i < numSegments + 1; i++)
        {
            float x = radius * Mathf.Cos(theta);
            float z = radius * Mathf.Sin(theta);
            Vector3 pos = new Vector3(x, z, 0);
            lineRenderer.SetPosition(i, pos);
            theta += deltaTheta;
        }
    }
}
