using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PassView : MonoBehaviour {
    private Text curLvText;
    private Button okayBtn;
    void Awake() {
        curLvText = transform.Find("Bg/Star/Sprite/Level").GetComponent<Text>();
        okayBtn = transform.Find("Bg/Okay").GetComponent<Button>();
        okayBtn.onClick.AddListener(() => {
            SoundManager.GetInstance.PlaySound("btnClick");
            ViewManager.GetInstance.popView("PassView", false, null);
            GameApplication.GetInstance.onGiftBtnClick(null);
            if (GameView.GetInstance.levelNum[ 0 ] % 5 == 0) {
                ViewManager.GetInstance.popView("SkinsView", true, null);
            }
        });
    }


    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    void OnEnable() {
        //升级礼花卡片
        EffectManager.GetInstance.ParticleShow(GameView.GetInstance.levelPro.transform.position, "LevelUpBoom");
        SoundManager.GetInstance.PlaySound("LevelUp");
        curLvText.text = "LV " + (GameView.GetInstance.levelNum[ 0 ]);
    }

    void OnDisable() {
    }
}
