using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawWedge : MonoBehaviour {

    public LineRenderer lineRenderer_1;
    public LineRenderer lineRenderer_2;
    public LineRenderer lineRenderer_3;




    public GameObject center;

    [Range(6, 180)]
    public int resolution = 60;

    [Range(3, 20)]
    public float radius = 5f;

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

        Tick();

    }


    void Tick() {
        lineRenderer_1.positionCount = resolution;
        var points = new Vector3[resolution + 1];
        for (int i = 0; i <= resolution; i++) {
            float stepPhi = Mathf.PI * 2 / resolution;
            float phi = 1f + i * stepPhi;
            float xpos = center.transform.position.x + radius * Mathf.Sin(phi);
            float zpos = center.transform.position.z + radius * Mathf.Cos(phi);
            points[i] = new Vector3(xpos, 0f, zpos);
        }

        lineRenderer_1.SetPositions(points);
    }
}
