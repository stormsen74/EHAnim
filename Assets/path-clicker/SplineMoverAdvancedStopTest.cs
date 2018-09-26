using System.Collections.Generic;
using UnityEngine;

// https://www.habrador.com/tutorials/interpolation/1-catmull-rom-splines/


[ExecuteInEditMode]
public class SplineMoverAdvancedStopTest : MonoBehaviour {
    public CatmullRom spline;

    public Transform pointHolder;

    [Range(2, 25)]
    public int resolution = 6;

    [Range(0, 20)]
    public float normalExtrusion = 20;

    [Range(0, 20)]
    public float tangentExtrusion = 20;

    [Range(0, 1)]
    public float globalTime = 0f;

    [Range(0, 1)]
    public float vesselSpeed = 0f;

    [Range(1, 20)]
    public float duration = 5.0f;


    public bool drawNormal, drawTangent, showControlPoints, useDuration;

    // -----------------
    private float timeStep, pointsLength, currentProgress;
    private int currentSegment, currentPathSegment;
    public int currentPathSegmentDisplay = 0;
    public int currentSegmentDisplay = 0;

    private TrailRenderer trail;
    private Transform[] controlPoints;

    // ------------------

    void Start() {

        //trail = transform.GetComponentInChildren<TrailRenderer>();

        controlPoints = new Transform[pointHolder.childCount];
        for (int i = 0; i < controlPoints.Length; i++) {
            controlPoints[i] = pointHolder.GetChild(i);
            //Debug.Log(controlPoints[i].transform.position.x);
        }

        if (showControlPoints) {
            for (int i = 0; i < controlPoints.Length; i++) {
                controlPoints[i].GetChild(0).localScale = new Vector3(.05f, .05f, .05f);
            }
        } else {
            for (int i = 0; i < controlPoints.Length; i++) {
                controlPoints[i].GetChild(0).localScale = new Vector3(.0f, .0f, .0f);
            }
        }


        if (spline == null && controlPoints.Length > 2) {
            spline = new CatmullRom(controlPoints, resolution);
        }

    }


    private void UpdatePosition(float time) {

        pointsLength = spline.GetPoints().Length - 2;
        timeStep = 1.0f / pointsLength;
        currentSegment = Mathf.CeilToInt(time / timeStep);
        currentPathSegment = currentSegment / resolution;
        currentProgress = (time % timeStep / timeStep);
        currentSegmentDisplay = currentSegment;
        currentPathSegmentDisplay = currentPathSegment;

        int[] stops = new int[2];
        stops[0] = 6;
        stops[1] = 10 - 1;

        if (currentSegment < pointsLength + 1) {
            Vector3 posStart = spline.GetPoints()[currentSegment].position;
            Vector3 tanStart = spline.GetPoints()[currentSegment].tangent;

            Vector3 posEnd = spline.GetPoints()[currentSegment + 1].position;
            Vector3 tanEnd = spline.GetPoints()[currentSegment + 1].tangent;

            if (currentPathSegment < stops[0] || currentPathSegment > stops[1]) {
                transform.rotation = Quaternion.Slerp(Quaternion.LookRotation(tanStart), Quaternion.LookRotation(tanEnd), currentProgress);
                transform.position = Vector3.Lerp(posStart, posEnd, currentProgress);
            }

            if (currentSegment == stops[1] * resolution) {
                int lastSegment = (stops[0] + 1) * resolution - 1;
                transform.rotation = Quaternion.Slerp(Quaternion.LookRotation(spline.GetPoints()[lastSegment].tangent), Quaternion.LookRotation(tanEnd), currentProgress);
            }


        }


    }


    private float GetTimeScale() {
        return 1 / duration;
    }

    private float GetSpeed() {
        return vesselSpeed;
    }

    private float UpdateProgress() {
        return !useDuration ? GetSpeed() : GetTimeScale();
    }

    void Update() {
        if (spline != null) {
            spline.Update(controlPoints);
            spline.Update(resolution);
            spline.DrawSpline(Color.white);

            if (drawNormal)
                spline.DrawNormals(normalExtrusion, Color.red);

            if (drawTangent)
                spline.DrawTangents(tangentExtrusion, Color.cyan);
        } else {
            spline = new CatmullRom(controlPoints, resolution);
        }


        if (globalTime <= 1) {
            globalTime += Time.deltaTime * UpdateProgress();
        } else {
            globalTime = .001f;
        }

        UpdatePosition(globalTime);

    }
}
