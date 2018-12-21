using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReviveView : MonoBehaviour {
    private Text timeText;
    private float timeVal;
    private Button shareBtn;
    private Button videoBtn;
    private Button jumpBtn;
    void Awake() {
        timeText = transform.Find("Bg/Time").GetComponent<Text>();
        shareBtn = transform.Find("Bg/Btns/Share").GetComponent<Button>();
        videoBtn = transform.Find("Bg/Btns/Video").GetComponent<Button>();
        jumpBtn = transform.Find("Bg/Btns/Jump").GetComponent<Button>();
        shareBtn.onClick.AddListener(() => {
            menuClick(0);
        });
        videoBtn.onClick.AddListener(() => {
            menuClick(1);
        });
        jumpBtn.onClick.AddListener(() => {
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
        timeVal = 20;
        timeText.text = " " + timeVal;
        InvokeRepeating("countGameTime", 1, 1);
        jumpBtn.gameObject.SetActive(false);
    }

    void OnDisable() {
        CancelInvoke("countGameTime");
    }

    //计算倒计时
    void countGameTime() {
        timeVal = timeVal - 1;
        timeText.text = " " + timeVal;
        if (timeVal == 0) {
            menuClick(2);
            CancelInvoke("countGameTime");
        }
        if (timeVal == 17) {
            jumpBtn.gameObject.SetActive(true);
        }
    }

    //点击事件处理
    void menuClick(int type) {
        SoundManager.GetInstance.PlaySound("btnClick");
        if (type == 0) {
            GameApplication.GetInstance.onShareBtnClick((isOk) => {
                if (isOk) {
                    ViewManager.GetInstance.popView("ReviveView", false, null);
                    GameView.GetInstance.BoomCard();
                }
            });
        }
        else if (type == 1) {
            GameApplication.GetInstance.onVideoBtnClick((isOk) => {
                if (isOk == 2) {
                    GameView.GetInstance.BoomCard();
                    ViewManager.GetInstance.popView("ReviveView", false, null);
                }
            }, 0);
        }
        else if (type == 2) {
            ViewManager.GetInstance.popView("ReviveView", false, null);
            //ViewManager.GetInstance.popView("OverView", true, null);
            PlayerPrefs.SetString("gameData", "0");
            GameView.GetInstance.gameStart();
        }
    }
}
