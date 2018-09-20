using System.Collections.Generic;
using UnityEngine;

// https://www.habrador.com/tutorials/interpolation/1-catmull-rom-splines/


[ExecuteInEditMode]
public class SplineMoverAdvanced : MonoBehaviour {
    public CatmullRom spline;

    public Transform pointHolder;

    [Range(2, 25)]
    public int resolution = 6;

    [Range(0, 20)]
    public float normalExtrusion = 20;

    [Range(0, 20)]
    public float tangentExtrusion = 20;

    [Range(0, 1)]
    public float progress = 0f;

    [Range(0, 1)]
    public float vesselSpeed = 0f;

    [Range(1, 20)]
    public float duration = 5.0f;


    public bool drawNormal, drawTangent, showControlPoints;

    // -----------------
    private float timeStep, pointsLength, currentProgress;
    private int currentSegment;
    public int currentSegmentDisplay = 0;

    private TrailRenderer trail;
    private Transform[] controlPoints;

    // ------------------

    void Start() {

        trail = transform.GetComponentInChildren<TrailRenderer>();

        controlPoints = new Transform[pointHolder.childCount];
        for (int i = 0; i < controlPoints.Length; i++) {
            controlPoints[i] = pointHolder.GetChild(i);
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
        currentProgress = (time % timeStep / timeStep);
        currentSegmentDisplay = currentSegment;

        if (currentSegment < pointsLength + 1) {
            Vector3 posStart = spline.GetPoints()[currentSegment].position;
            Vector3 tanStart = spline.GetPoints()[currentSegment].tangent;

            Vector3 posEnd = spline.GetPoints()[currentSegment + 1].position;
            Vector3 tanEnd = spline.GetPoints()[currentSegment + 1].tangent;

            Vector3 segmentPosition = CatmullRom.CalculatePosition(posStart, posEnd, tanStart, tanEnd, currentProgress);
            Vector3 segmentTangent = CatmullRom.CalculateTangent(posStart, posEnd, tanStart, tanEnd, currentProgress);

            transform.position = Vector3.Lerp(posStart, posEnd, currentProgress);
            transform.rotation = Quaternion.Slerp(Quaternion.LookRotation(tanStart), Quaternion.LookRotation(tanEnd), currentProgress);
        }


    }


    private float GetTimeScale() {
        return 1 / duration;
    }

    private float GetSpeed() {
        return vesselSpeed;
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



        if (progress < 1) {
            progress += Time.deltaTime * GetSpeed();
            if (!trail.emitting) {
                trail.Clear();
                trail.emitting = true;
            }
        } else {
            progress = .01f;
            trail.emitting = false;
        }

        UpdatePosition(progress);
    }
}
