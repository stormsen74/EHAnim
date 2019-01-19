using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour {

    private Animator anim;

    enum Clip { Red, Green, Blue };

    void Start() {
        anim = GetComponent<Animator>();
        //anim.speed = .5f;

    }

    private void setClip(Clip clip) {

        float currentClipTime = anim.GetCurrentAnimatorStateInfo(0).normalizedTime;

        switch (clip) {
            case Clip.Red:
                //anim.SetTrigger("red");
                anim.Play("redA", 0, currentClipTime);
                break;
            case Clip.Green:
                //anim.SetTrigger("green");
                anim.Play("greenA", 0, currentClipTime);
                break;
            case Clip.Blue:
                //anim.SetTrigger("blue");
                anim.Play("blueA", 0, currentClipTime);
                break;

        }

    }

    void Update() {

        if (Input.GetKeyDown(KeyCode.Alpha1)) setClip(Clip.Red);
        if (Input.GetKeyDown(KeyCode.Alpha2)) setClip(Clip.Green);
        if (Input.GetKeyDown(KeyCode.Alpha3)) setClip(Clip.Blue);

    }
}
