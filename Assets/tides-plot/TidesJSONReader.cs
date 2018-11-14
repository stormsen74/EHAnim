using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct JSONTides {
    public string meta;
    public PointData[] plotPoints;
}


[System.Serializable]
public struct PointData {
    public float step;
    public float y;
}


public class TidesJSONReader : MonoBehaviour {
    [SerializeField]
    private TextAsset asset;

    private JSONTides data;

    public PointData[] Points {
        get {
            return data.plotPoints;
        }
    }

    private void Awake() {
        string json = asset.ToString();
        data = JsonUtility.FromJson<JSONTides>(json);

    }
}