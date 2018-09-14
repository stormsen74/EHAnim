using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// https://www.youtube.com/watch?v=Xwj8_z9OrFw
// https://www.youtube.com/watch?v=1aBjTa3xQzE

public class EditorPathScript : MonoBehaviour {

    public Color rayColor = Color.white;
    public List<Transform> pathObjects = new List<Transform>();
    Transform[] transformArray;


    private void OnDrawGizmos() {
        Gizmos.color = rayColor;
        transformArray = GetComponentsInChildren<Transform>();
        pathObjects.Clear();


        foreach (Transform pathObject in transformArray) {

            if (pathObject != this.transform) {
                pathObjects.Add(pathObject);
            }

            for (int i = 0; i < pathObjects.Count; i++) {

                Vector3 position = pathObjects[i].position;

                if (i > 0) {
                    Vector3 previousPosition = pathObjects[i - 1].position;
                    Gizmos.DrawLine(previousPosition, position);
                    Gizmos.DrawWireSphere(position, .3f);

                }

            }

        }
    }

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }
}
