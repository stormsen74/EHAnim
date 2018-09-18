using System.Collections.Generic;
using UnityEngine;

// https://www.habrador.com/tutorials/interpolation/1-catmull-rom-splines/


[ExecuteInEditMode]
public class SplineMover : MonoBehaviour {
    public JPBotelho.CatmullRom spline;

    public Transform[] controlPoints;

    [Range(2, 25)]
    public int resolution = 16;
    public bool closedLoop;

    [Range(0, 20)]
    public float normalExtrusion;

    [Range(0, 20)]
    public float tangentExtrusion;

    [Range(0, 1)]
    public float time = 0f;

    private float timeStep;

    public bool drawNormal, drawTangent;

    // -----------------
    public int currentSegmentDisplay = 0;

    // ------------------

    void Start()
    {
        if (spline == null)
        {
            spline = new JPBotelho.CatmullRom(controlPoints, resolution, closedLoop);
        }

    }


    private void UpdatePosition(float time)
    {
        float l = spline.GetPoints().Length - 2;
        timeStep = 1.0f / l;
        int currentSegment = Mathf.CeilToInt(time / timeStep);
        //currentSegment = Mathf.Min(currentSegment, spline.GetPoints().Length - 1);

        float currentProgress = (time % timeStep / timeStep);


        currentSegmentDisplay = currentSegment;
        //Debug.Log(time % timeStep);

        if (currentSegment < l + 1)
        {

            Vector3 posStart = spline.GetPoints()[currentSegment].position;
            Vector3 tanStart = spline.GetPoints()[currentSegment].tangent;

            Vector3 posEnd = spline.GetPoints()[currentSegment + 1].position;
            Vector3 tanEnd = spline.GetPoints()[currentSegment + 1].tangent;

            Vector3 segmentPosition = JPBotelho.CatmullRom.CalculatePosition(posStart, posEnd, tanStart, tanEnd, currentProgress);
            Vector3 segmentTangent = JPBotelho.CatmullRom.CalculateTangent(posStart, posEnd, tanStart, tanEnd, currentProgress);

            transform.position = Vector3.Lerp(posStart, posEnd, currentProgress);
            transform.rotation = Quaternion.Slerp(Quaternion.LookRotation(tanStart), Quaternion.LookRotation(tanEnd), currentProgress);
        }


    }

    void Update()
    {
        if (spline != null)
        {
            spline.Update(controlPoints);
            spline.Update(resolution, closedLoop);
            spline.DrawSpline(Color.white);

            if (drawNormal)
                spline.DrawNormals(normalExtrusion, Color.red);

            if (drawTangent)
                spline.DrawTangents(tangentExtrusion, Color.cyan);
        }
        else
        {
            spline = new JPBotelho.CatmullRom(controlPoints, resolution, closedLoop);
        }





        if (time < 1)
        {
            time += Time.deltaTime * .1f;
        }
        else
        {
            time = 0;
        }

        UpdatePosition(time);
    }
}
