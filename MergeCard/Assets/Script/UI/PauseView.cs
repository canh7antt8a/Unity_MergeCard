using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseView : MonoBehaviour {
    public static PauseView GetInstance { get; private set; }
    private Image musicSprite;
    private void Awake() {
        GetInstance = this;
        musicSprite = transform.Find("Bg/Btns/Music").GetComponent<Image>();
    }

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    void OnEnable() {
        //this.selectView.node.height = 0;
        //声音按钮初始化
        if (SoundManager.GetInstance.isOpen) {
            ResManager.GetInstance.loadSprite("UI/musicOnSprite", (sp) => {
                musicSprite.sprite = sp as Sprite;
            });
        }
        else {
            ResManager.GetInstance.loadSprite("UI/musicOffSprite", (sp) => {
                musicSprite.sprite = sp as Sprite;
            });
        }
    }


    //点击事件处理
    public void menuClick(int type) {
        SoundManager.GetInstance.PlaySound("btnClick");
        if (type == 0) {
            ViewManager.GetInstance.popView("DescriptionView", true, null);
        }
        else if (type == 1) {
            PlayerPrefs.SetString("gameData", "0");
            GameView.GetInstance.gameStart();
            ViewManager.GetInstance.popView("PauseView", false, null);
        }
        else if (type == 2) {
            if (!SoundManager.GetInstance.isOpen) {
                SoundManager.GetInstance.setIsOpen(true);
                SoundManager.GetInstance.setBgOpen(true);
                ResManager.GetInstance.loadSprite("UI/musicOnSprite", (sp) => {
                    musicSprite.sprite = sp as Sprite;
                });
            }
            else {
                SoundManager.GetInstance.setIsOpen(false);
                SoundManager.GetInstance.setBgOpen(false);
                ResManager.GetInstance.loadSprite("UI/musicOffSprite", (sp) => {
                    musicSprite.sprite = sp as Sprite;
                });
            }
        }
        else if (type == 3) {
            ViewManager.GetInstance.popView("PauseView", false, null);
            //ViewManager.GetInstance.showView("GameView", false, true);
            ViewManager.GetInstance.showView("MainView", true, false);
        }
        else if (type == 4) {
            ViewManager.GetInstance.popView("PauseView", false, null);
        }
    }
}
