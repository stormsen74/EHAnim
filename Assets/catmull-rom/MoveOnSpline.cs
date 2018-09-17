using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveOnSpline : MonoBehaviour {

    public JPBotelho.CatmullRom spline;
    public int currentWaypointID = 0;
    public float speed;
    public float rotationSpeed = 5.0f;
    public float reachDistance = 1.0f;
    public string pathName;

    Vector3 lastPosition;
    Vector3 currentPosition;

    // Use this for initialization
    void Start () {
        lastPosition = transform.position;
    }
	
	// Update is called once per frame
	void Update () {

        //float distance = Vector3.Distance(spline[currentWaypointID].position, transform.position);
        //transform.position = Vector3.MoveTowards(transform.position, pathToFollow.pathObjects[currentWaypointID].position, Time.deltaTime * speed);

        //var rotation = Quaternion.LookRotation(pathToFollow.pathObjects[currentWaypointID].position - transform.position);
        //transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);

        //if (distance <= reachDistance) {
        //    currentWaypointID++;
        //}

        //if (currentWaypointID >= pathToFollow.pathObjects.Count) {
        //    currentWaypointID = 0;
        //}


    }
}
