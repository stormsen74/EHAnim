using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TidesPlot : MonoBehaviour {

    public PlotCatmullRom spline;
    public Vector3[] controlPoints;

    private Vector3[] linePoints;

    [Range(2, 25)]
    public int resolution = 6;

    [Range(0, .999f)]
    public float progress = 0f;

    [Range(0, 1)]
    public float vesselSpeed = 0f;

    [Range(1, 60)]
    public float duration = 5.0f;

    public Transform origin;
    public Transform curvePoint;
    public GameObject cube;
    public LineRenderer lineRenderer;

    private float timeStep, pointsLength, currentProgress;
    private int currentSegment;
    public int currentSegmentDisplay = 0;

    private PointData[] plotPoints;

    // Use this for initialization
    void Start() {
        TidesJSONReader reader = GetComponent<TidesJSONReader>();
        plotPoints = reader.Points;

        Debug.Log(plotPoints[0].y);

        controlPoints = new Vector3[plotPoints.Length];

        for (int i = 0; i < plotPoints.Length; i++) {
            controlPoints[i] = new Vector3(
                origin.transform.position.x + plotPoints[i].step * 19f,
                origin.transform.position.y + plotPoints[i].y / 100f,
                0f
         );
        }


        if (spline == null) {
            spline = new PlotCatmullRom(controlPoints, resolution);
        }

        cube.GetComponent<Renderer>().material = new Material(Shader.Find("Unlit/Color"));
    }


    private void UpdatePosition(float time) {
        pointsLength = spline.GetPoints().Length - 2;
        timeStep = 1.0f / pointsLength;
        currentSegment = Mathf.CeilToInt(time / timeStep);
        currentProgress = (time % timeStep / timeStep);
        currentSegmentDisplay = currentSegment;

        if (currentSegment < pointsLength + 1) {
            Vector3 posStart = spline.GetPoints()[currentSegment].position;
            Vector3 tanStart = spline.GetPoints()[currentSegment].tangent;

            Vector3 posEnd = spline.GetPoints()[currentSegment + 1].position;
            Vector3 tanEnd = spline.GetPoints()[currentSegment + 1].tangent;

            Vector3 segmentPosition = CatmullRom.CalculatePosition(posStart, posEnd, tanStart, tanEnd, currentProgress);
            Vector3 segmentTangent = CatmullRom.CalculateTangent(posStart, posEnd, tanStart, tanEnd, currentProgress);

            curvePoint.position = Vector3.Lerp(posStart, posEnd, currentProgress);
            //cube.rotation = Quaternion.Slerp(Quaternion.LookRotation(tanStart), Quaternion.LookRotation(tanEnd), currentProgress);

            Quaternion q = Quaternion.Slerp(Quaternion.LookRotation(tanStart), Quaternion.LookRotation(tanEnd), currentProgress);
            cube.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 360f - q.eulerAngles.x));
            if (q.eulerAngles.x < 180) {
                cube.GetComponent<Renderer>().material.color = Color.red;
                Debug.Log(-q.eulerAngles.x);
            } else {
                cube.GetComponent<Renderer>().material.color = Color.blue;
                Debug.Log(360f - q.eulerAngles.x);
            }
        }




    }


    private void UpdateLinePoints() {
        //Debug.Log(spline.GetPoints().Length);
        linePoints = new Vector3[spline.GetPoints().Length];

        for (int i = 0; i < spline.GetPoints().Length; i++) {
            //Debug.Log(spline.GetPoints()[i].position);
            linePoints[i] = spline.GetPoints()[i].position;
        }

    }


    private float GetTimeScale() {
        return 1 / duration;
    }

    private float GetSpeed() {
        return vesselSpeed;
    }

    // Update is called once per frame
    void Update() {
        if (spline != null) {
            spline.Update(resolution);
            spline.Update(controlPoints);
            UpdateLinePoints();

            spline.DrawSpline(Color.magenta);

            lineRenderer.positionCount = linePoints.Length;
            lineRenderer.SetPositions(linePoints);

            //if (drawNormal)
            //    spline.DrawNormals(normalExtrusion, Color.red);

            //if (drawTangent)
            //    spline.DrawTangents(tangentExtrusion, Color.cyan);
        } else {
            spline = new PlotCatmullRom(controlPoints, resolution);
        }

        if (progress < 1) {
            progress += Time.deltaTime * GetTimeScale();
        } else {
            progress = .01f;
        }

        UpdatePosition(progress);

    }
}

