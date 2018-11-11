using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour {

    public Transform pStart;
    public Transform pEnd;

    public LineRenderer lineRenderer;

    // Use this for initialization
    void Start() {
        lineRenderer.material = new Material(Shader.Find("Unlit/Color"));
        lineRenderer.material.color = Color.blue;
    }

    // Update is called once per frame
    void Update() {
        lineRenderer.SetPosition(0, pStart.position);
        lineRenderer.SetPosition(1, pEnd.position);
    }
}








