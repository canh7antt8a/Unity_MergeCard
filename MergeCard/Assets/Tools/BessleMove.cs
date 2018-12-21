using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJson;

public class BessleMove : MonoBehaviour {
    [SerializeField]
    private List<Transform> poins;
    private List<List<Vector3>> positions;

    private void Awake() {
        positions = new List<List<Vector3>>();
        LoadConfig();
    }

    void LoadConfig() {
        Object res = Resources.Load<Object>("Config/Road");
        JsonArray obj = (JsonArray)SimpleJson.SimpleJson.DeserializeObject(res.ToString());
        for (var i = 0; i < obj.Count; i++) {
            positions.Add(new List<Vector3>());
            JsonArray v3List = (JsonArray)obj[ i ];
            for (var j = 0; j < v3List.Count; j++) {
                JsonObject pointObj = (JsonObject)v3List[ j ];
                Vector3 point = new Vector3(float.Parse(pointObj[ "x" ].ToString()), float.Parse(pointObj[ "y" ].ToString()), float.Parse(pointObj[ "z" ].ToString()));
                positions[ i ].Add(point);
            }
        }
    }

    // Use this for initialization
    void Start() {
        BessleMoveAction(0,1,1f);
    }

    void BessleMoveAction(float start,float end,float speed) {
        for (float i = start; i <= end; i = i + 0.02f) {
            StartCoroutine(OneStep(i, i / speed));
        }
    }

    //延时方法
    public IEnumerator OneStep(float pros, float time) {
        yield return new WaitForSeconds(time);
        Vector3 pos = BessleCount(pros, positions[0]);
        transform.DOMove(pos, time);
    }

    Vector3 BessleCount(float pros, List<Vector3> positions) {
        List<Vector3> temp = new List<Vector3>(positions);
        while (temp.Count > 1) {
            for (var i = 0; i < temp.Count - 1; i++) {
                Vector3 pos = Vector3.Lerp(temp[ i ], temp[ i + 1 ], pros);
                temp[ i ] = pos;
            }
            temp.RemoveAt(temp.Count - 1);
        }
        return temp[ 0 ];
    }

    // Update is called once per frame
    void Update() {}
}
