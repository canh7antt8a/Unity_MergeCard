using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OverView : MonoBehaviour {
    private Text curScoreText;
    private Text bestScoreText;
    private Button restartBtn;
    private Button shareChallengeBtn;
    private Button backBtn;

    void Awake() {

        curScoreText = transform.Find("Bg/CurScore").GetComponent<Text>();
        bestScoreText = transform.Find("Bg/BestScore").GetComponent<Text>();
        restartBtn = transform.Find("Bg/Btns/Replay").GetComponent<Button>();
        shareChallengeBtn = transform.Find("Bg/Btns/Challenge").GetComponent<Button>();
        backBtn = transform.Find("Bg/Btns/Back").GetComponent<Button>();
        restartBtn.onClick.AddListener(() => {
            menuClick(0);
        });
        shareChallengeBtn.onClick.AddListener(() => {
            menuClick(1);
        });
        backBtn.onClick.AddListener(() => {
            menuClick(2);
        });
    }

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    void OnEnable() {
        bestScoreText.text = GameView.GetInstance.bestScore + "";
        curScoreText.text = GameView.GetInstance.curScore + "";
        //this.scheduleOnce(function(){
        //    gameApplication.onGiftBtnClick(null, 1);
        //}.bind(this),1);
    }

    //点击事件处理
    void menuClick(int type) {
        SoundManager.GetInstance.PlaySound("btnClick");
        if (type == 0) {
            ViewManager.GetInstance.popView("OverView", false, null);
            PlayerPrefs.SetString("gameData", "0");
            GameView.GetInstance.gameStart();
        }
        else if (type == 1) {
            GameApplication.GetInstance.onShareBtnClick(null);
        }
        else if (type == 2) {
            PlayerPrefs.SetString("gameData", "0");
            ViewManager.GetInstance.showView("MainView", true, false);
            ViewManager.GetInstance.popView("OverView", false, null);
        }
    }

}
