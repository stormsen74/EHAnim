using TMPro;
using UnityEngine;
using DG.Tweening;

public class TextFade : MonoBehaviour {


    [SerializeField]
    private TextMeshProUGUI TMText;

    Color textColor0 = new Color32(255, 255, 255, 0);
    Color textColor1 = new Color32(255, 255, 255, 255);

    void Start () {

        TMText.color = textColor0;

    }

    private void FadeIn() {
        TMText.DOColor(textColor1, 1).SetEase(Ease.InOutCubic);
    }

    private void FadeOut() {
        TMText.DOColor(textColor0, 1).SetEase(Ease.InOutCubic);
    }

    void Update () {

        if (Input.GetKeyDown(KeyCode.A)) {
            FadeIn();
        }

        if (Input.GetKeyDown(KeyCode.S)) {
            FadeOut();
        }

    }
}
