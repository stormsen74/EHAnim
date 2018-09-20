using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathClicker : MonoBehaviour {

    public GameObject pathPoint;
    public GameObject pointHolder;

    private readonly float mapPixelWidth = 2048f;
    private readonly float mapPixelHeight = 1140f;
    private float mapRatio;

    private float mapWidth = 10f;
    private float mapHeight = 0f;


    void Start() {
        mapRatio = mapPixelWidth / mapPixelHeight;
        mapHeight = mapWidth / mapRatio;

        pathPoint = GameObject.Find("PathPoint");
        pointHolder = GameObject.Find("PointHolder");
    }

    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            float x = (Input.mousePosition.x / mapPixelWidth) * mapWidth;
            float z = ((mapPixelHeight - Input.mousePosition.y) / mapPixelHeight) * mapHeight;

            GameObject clone = Instantiate(pathPoint, new Vector3(x, 0f, -z), Quaternion.identity);
            clone.transform.parent = pointHolder.transform;
        }
    }



}
