using System.Collections.Generic;
using UnityEngine;

// https://www.habrador.com/tutorials/interpolation/1-catmull-rom-splines/

namespace JPBotelho {
    [ExecuteInEditMode]
    public class SplineTester : MonoBehaviour {
        public CatmullRom spline;

        public Transform[] controlPoints;

        [Range(2, 25)]
        public int resolution;
        public bool closedLoop;

        [Range(0, 20)]
        public float normalExtrusion;

        [Range(0, 20)]
        public float tangentExtrusion;

        [Range(0, 1)]
        public float time;

        public bool drawNormal, drawTangent;

        // -----------------
        public int currentWaypointID = 0;
        public float speed = 10;
        public float rotationSpeed = 2.5f;
        public float reachDistance = 1.0f;

        Vector3 lastPosition;
        Vector3 currentPosition;

        // ------------------

        void Start() {
            if (spline == null) {
                spline = new CatmullRom(controlPoints, resolution, closedLoop);
            }

            lastPosition = transform.position;

            Debug.Log(spline.GetPoints()[1].position);
            Debug.Log(spline.GetPoints()[1].tangent);



        }

        void Update() {
            if (spline != null) {
                spline.Update(controlPoints);
                spline.Update(resolution, closedLoop);
                spline.DrawSpline(Color.white);

                if (drawNormal)
                    spline.DrawNormals(normalExtrusion, Color.red);

                if (drawTangent)
                    spline.DrawTangents(tangentExtrusion, Color.cyan);
            } else {
                spline = new CatmullRom(controlPoints, resolution, closedLoop);
            }


            if (speed > 0) {
                Debug.Log(spline.GetPoints().Length);

                float distance = Vector3.Distance(spline.GetPoints()[currentWaypointID].position, transform.position);
                transform.position = Vector3.MoveTowards(transform.position, spline.GetPoints()[currentWaypointID].position, Time.deltaTime * speed);

                var rotation = Quaternion.LookRotation(spline.GetPoints()[currentWaypointID].position - transform.position);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);

                if (distance <= reachDistance) {
                    currentWaypointID++;
                }

            }

            if (currentWaypointID >= spline.GetPoints().Length) {
                currentWaypointID = 0;
                transform.position = spline.GetPoints()[currentWaypointID].position;
                transform.rotation = Quaternion.LookRotation(spline.GetPoints()[currentWaypointID].position - transform.position);
            }
        }
    }
}