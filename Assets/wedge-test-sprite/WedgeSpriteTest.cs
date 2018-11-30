using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WedgeSpriteTest : MonoBehaviour {

    public GameObject  wedge;

    // Use this for initialization
    void Start () {
        Shapes2D.Shape w_1 = wedge.GetComponent<Shapes2D.Shape>();

        w_1.settings.startAngle = 0;
        w_1.settings.endAngle = 90;

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
