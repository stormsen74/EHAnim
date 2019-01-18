using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DotTweenSetup : MonoBehaviour {
    public Transform cube;

    Sequence InSequence;
    Sequence OutSequence;

    void Start() {


    }

    private void In() {
        float tweenTime;
        if (OutSequence != null && OutSequence.IsActive()) {
            tweenTime = OutSequence.ElapsedDirectionalPercentage() * 2.5f;
            OutSequence.Kill();
        } else {
            tweenTime = 2.5f;
        }


        Tween _transform = cube.DOMove(new Vector3(10, 0, 0), tweenTime).SetEase(Ease.InOutSine);
        Tween _rotate = cube.DORotate(new Vector3(0, 180, 0), tweenTime).SetEase(Ease.InOutSine);

        InSequence = DOTween.Sequence();
        InSequence.OnComplete(OnInSequenceComplete);
        InSequence.Insert(0, _transform);
        InSequence.Insert(0, _rotate);

    }

    private void Out() {
        float tweenTime;
        if (InSequence != null && InSequence.IsActive()) {
            tweenTime = InSequence.ElapsedDirectionalPercentage() * 1f;
            InSequence.Kill();
        } else {
            tweenTime = 1f;
        }

        Tween _transform = cube.DOMove(new Vector3(0, 0, 0), tweenTime).SetEase(Ease.InOutSine);
        Tween _rotate = cube.DORotate(new Vector3(0, 0, 0), tweenTime).SetEase(Ease.InOutSine);

        OutSequence = DOTween.Sequence();
        OutSequence.OnComplete(OnOutSequenceComplete);
        OutSequence.Insert(0, _transform);
        OutSequence.Insert(0, _rotate);
    }

    private void OnInSequenceComplete() {
        Debug.Log("OnInSequenceComplete");
    }

    private void OnOutSequenceComplete() {
        Debug.Log("OnOutSequenceComplete");
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.A)) {
            In();
        }

        if (Input.GetKeyDown(KeyCode.S)) {
            Out();
        }
    }
}
