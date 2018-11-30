using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTest : MonoBehaviour {


    //public Transform cube;

    public AnimationCurve animationCurve;
    public Vector3 vr1 = new Vector3(0, 0, 0);
    public Vector3 vr2 = new Vector3(0, 180, 0);

    // Use this for initialization
    void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            StartCoroutine(Move(vr1, vr2, animationCurve, 5.0f));
        }
    }

    IEnumerator Move(Vector3 vr1, Vector3 vr2, AnimationCurve ac, float time) {
        float timer = 0.0f;
        while (timer <= time) {
            Vector3 result = Vector3.Lerp(vr1, vr2, ac.Evaluate(timer / time));
            transform.rotation = Quaternion.Euler(result);
            timer += Time.deltaTime;
            yield return null;
        }
    }
}
