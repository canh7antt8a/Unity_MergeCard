using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainView : MonoBehaviour {
    public static MainView GetInstance { get; private set; }

    void Awake() {
        //单例初始化
        GetInstance = this;
    }

    // Use this for initialization
    void Start() {

    }

    //点击事件处理
    public void menuClick(int type) {
        SoundManager.GetInstance.PlaySound("btnClick");
        if (type == 0) {
            ViewManager.GetInstance.popView("RankView", true, null);
        }
        else if (type == 1) {
            GameApplication.GetInstance.onShareBtnClick((isOk) => {
            });
        }
        else if (type == 2) {
            ViewManager.GetInstance.showView("GameView", true, true);
        }
        else if (type == 3) {
            //退出游戏
            UPLTV.GetInstance.ExitGame();
            StartCoroutine(GameApplication.GetInstance.DelayedAction(() => {
#if UNITY_EDITOR
 UnityEditor.EditorApplication.isPlaying = false;
#else
 Application.Quit();
#endif
            }, 3f));
        }
    }


    // Update is called once per frame
    void Update() {

    }
}
