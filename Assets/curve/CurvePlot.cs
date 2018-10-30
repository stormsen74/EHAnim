using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurvePlot : MonoBehaviour {

    public LineRenderer lineRenderer;
    public GameObject Point;

    [Range(0f, 1f)]
    public float T = 0;

    [Range(1f, .1f)]
    public float speedDamp = .5f;

    public int numLinePoints = 20;

    public bool useTime = true;

    void Start() {
    }

    void Update() {
        lineRenderer.positionCount = numLinePoints;
        var points = new Vector3[numLinePoints];
        for (int i = 0; i < numLinePoints; i++) {
            float xstep = Mathf.PI * 2 / numLinePoints;
            float xpos = i * xstep;
            float ypos = Mathf.Sin(xpos);
            points[i] = new Vector3(xpos, ypos, 0.0f);
        }
        lineRenderer.SetPositions(points);


        float t, x;
        if (useTime) {
            t = Time.time * speedDamp;
            x = (t % 1) * Mathf.PI * 2;
        } else {
            x = T * Mathf.PI * 2;
        }

        Point.transform.position = new Vector3(x, Mathf.Sin(x), 0f);
    }
}
