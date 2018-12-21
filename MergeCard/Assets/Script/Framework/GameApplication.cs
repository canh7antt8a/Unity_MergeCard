using Facebook.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameApplication : MonoBehaviour {
    public static GameApplication GetInstance { get; private set; }

    //各类脚本控制器
    private ViewManager viewManager = null;
    private ResManager resManager = null;
    private SoundManager soundManager = null;
    private EffectManager effectManager = null;
    private DataManager dataManager = null;
    private Player player = null;
    private UPLTV SDK = null;

    private int missionIdx = 0;
    private int levelIdx = 0;
    private string[] unitCof = { "", "K", "M", "B", "T" };

    //异步加载场景
    [HideInInspector]
    public AsyncOperation operation = null;
    private string nextScenes = "";
    public void LoadScenes(string name, System.Action<AsyncOperation> cb) {
        nextScenes = name;
        SceneManager.LoadScene("Loading");
        StartCoroutine(AsyncLoading(cb));
    }

    //加载场景的协程
    IEnumerator AsyncLoading(System.Action<AsyncOperation> cb) {
        operation = SceneManager.LoadSceneAsync(nextScenes);
        //阻止当加载完成自动切换
        operation.allowSceneActivation = false;
        operation.completed += cb;
        yield return operation;
    }

    //获取游戏信息
    public int[] GetGameInfo() {
        int[] info = { missionIdx, levelIdx };
        return info;
    }

    //设置游戏信息
    public void SetGameInfo(int[] info) {
        if (info.Length > 1) {
            missionIdx = info[ 0 ];
            levelIdx = info[ 1 ];
            var last = PlayerPrefs.GetInt("MissionPros-" + missionIdx, 0);
            if (levelIdx > last) {
                PlayerPrefs.SetInt("MissionPros-" + missionIdx, levelIdx);
            }
            if (missionIdx == 2 && levelIdx == 45) {
                missionIdx = missionIdx + 1;
                levelIdx = 0;
            }
            else if (levelIdx == 100) {
                if (missionIdx != 5) {
                    missionIdx = missionIdx + 1;
                    levelIdx = 0;
                }
                else {
                    missionIdx = 0;
                    levelIdx = 0;
                }
            }
        }
    }

    //插屏广告
    public void InsertAd() {
        var times = PlayerPrefs.GetInt("InsertAd", 0);
        times = times + 1;
        PlayerPrefs.SetInt("InsertAd", times);
        if (times == 3) {
            PlayerPrefs.SetInt("InsertAd", 0);
        }
    }

    public void MusicClick(bool isSet, Image musicBtn) {
        if (isSet) {
            if (SoundManager.GetInstance.isOpen) {
                SoundManager.GetInstance.setIsOpen(false);
            }
            else {
                SoundManager.GetInstance.setIsOpen(true);
            }
        }

        if (SoundManager.GetInstance.isOpen) {
            ResManager.GetInstance.loadSprite("UI/musicOn", (spRes) => {
                musicBtn.sprite = spRes as Sprite;
            });
        }
        else {
            ResManager.GetInstance.loadSprite("UI/musicOff", (spRes) => {
                musicBtn.sprite = spRes as Sprite;
            });
        }
    }

    //延时方法
    public IEnumerator DelayedAction(System.Action cb, float time) {
        yield return new WaitForSeconds(time);
        if (cb != null) {
            cb();
        }
    }

    //渐变
    public IEnumerator Vec3TweenTo(Vector3 first, Vector3 tarPos, float time, System.Action<Vector3> cb) {
        while (first != tarPos) {
            first =
                Vector3.MoveTowards(
                    first,
                    tarPos,
                    tarPos.x / time * Time.deltaTime);
            cb(first);
            yield return 0;
        }
    }

    void Awake() {
        FB.Init(this.OnInitComplete, this.OnHideUnity);
    }
    private void OnInitComplete() {
        //单例初始化
        GetInstance = this;
        //防止场景加载销毁
        DontDestroyOnLoad(gameObject);
        //加载各类控制脚本
        dataManager = gameObject.AddComponent<DataManager>();
        resManager = gameObject.AddComponent<ResManager>();
        soundManager = gameObject.AddComponent<SoundManager>();
        viewManager = gameObject.AddComponent<ViewManager>();
        effectManager = gameObject.AddComponent<EffectManager>();
        player = gameObject.AddComponent<Player>();
        SDK = gameObject.AddComponent<UPLTV>();
        LoadScenes("Game", (o) => {
            if (o.isDone) {
                EffectManager.GetInstance.SetEffect();
                ViewManager.GetInstance.showView("GameView", true, true);
            }
        });
    }

    private void OnHideUnity(bool isGameShown) {
    }

    // Use this for initialization
    void Start() {
        //初始化语言
        //resManager.loadConfig("UnitList", function(cof) {
        //    //单位信息
        //    this.unitCof = cof.unitList;
        //}.bind(this));
    }

    // Update is called once per frame
    void Update() {
    }

    public void onShareBtnClick(System.Action<bool> cb) {

    }

    //计算单位
    public string[] CountUnit(float num) {
        var unit = 0;
        while (num >= 10000) {
            num = num * 0.001f;
            unit = unit + 1;
        }
        int i = (int)(num * 100);
        var money = (float)(i * 1.0) / 100;
        string[] result = { money + "", unit + "", ("$" + money + unitCof[ unit ]), (money + unitCof[ unit ]) };
        return result;
    }

    //计算游戏时长
    //    void countGameTime()
    //    {

    //        if (this.trueTime != 0 && this.trueTime != 100000000000000000)
    //        {
    //            this.trueTime = this.trueTime + 3;
    //            let nowTime = new Date().getTime() / 1000;
    //            if (Math.abs(this.trueTime - nowTime) > 3600)
    //            {
    //                this.warnTips("lang.errorTime", function(){
    //                    SDK().quit();
    //                    cc.game.end();
    //                }.bind(this))
    //                this.trueTime = 100000000000000000;
    //            }
    //        }

    //        if (this.gameBg == null)
    //        {
    //            return;
    //        }
    //        var hour = new Date().getHours();
    //        if (hour >= 6 && hour <= 16)
    //        {
    //            if (this.gameBg.spriteFrame.name != "Bg0")
    //            {
    //                resManager.loadSprite("gameBg.Bg0", function(sp) {
    //                    this.gameBg.spriteFrame = sp;
    //                }.bind(this))
    //            }
    //        }
    //        else if (hour > 16 && hour <= 19)
    //        {
    //            if (this.gameBg.spriteFrame.name != "Bg1")
    //            {
    //                resManager.loadSprite("gameBg.Bg1", function(sp) {
    //                    this.gameBg.spriteFrame = sp;
    //                }.bind(this))
    //            }
    //        }
    //        else
    //        {
    //            if (this.gameBg.spriteFrame.name != "Bg2")
    //            {
    //                resManager.loadSprite("gameBg.Bg2", function(sp) {
    //                    this.gameBg.spriteFrame = sp;
    //                }.bind(this))
    //            }
    //        }
    //    },

    //    //设置语言
    //    void setLanguage(language)
    //    {
    //        const i18n = require('LanguageData');
    //        i18n.init(language);
    //    },

    //视频奖励
    public void onVideoBtnClick(System.Action<int> cb, int idType) {
#if UNITY_EDITOR
        cb(2);
#endif
        SDK.showRewardAd(
            (type) => {
                if (type == 0) {
                    fbFail(1);
                    Debug.Log("reward no ready");
                }
                else if (type == 1) {
                    fbFail(1);
                }
                if (cb != null) {
                    cb(type);
                }
            }
        );
    }

    //    //检查日常次数限制
    //    void checkDailyCount(key, isAdd, cb)
    //    {
    //        var myDate = new Date();
    //        let month = myDate.getMonth();       //获取当前月份(0-11,0代表1月)
    //        let day = myDate.getDate();        //获取当前日(1-31)
    //        SDK().getItem(month + "_" + day + "_" + key, function(val) {
    //            if (val == null)
    //            {
    //                val = 0;
    //            }
    //            val = parseInt(val);
    //            if (isAdd)
    //            {
    //                val = val + 1
    //                    var param = { };
    //                param[month + "_" + day + "_" + key] = val;
    //                SDK().setItem(param);
    //            }
    //            if (cb != null)
    //            {
    //                cb(val);
    //            }
    //        })
    //    },

    //    //插屏广告按钮
    public void onGiftBtnClick(System.Action<int> cb) {
        SDK.showInterstitialAd(
            (type) => {
                if (type == 0) {
                    Debug.Log("insert no ready");
                }
                else if (type == 1) {
                    //this.fbFail(1);
                }
                if (cb != null) {
                    cb(type);
                }
            }
        );
    }

    //    //显示是否分享的提示框
    //    void showSharaView(cb)
    //    {
    //        if (this.SharaView == null)
    //        {
    //            var view = cc.instantiate(this.SharaView_prefab);
    //            var Canvas = cc.find("Canvas");
    //            view.parent = Canvas;
    //            view.width = window.width;
    //            view.height = window.height;
    //            this.SharaView = view;
    //        }
    //        this.SharaView.active = true;
    //        let sureBtn = this.SharaView.getChildByName("Bg").getChildByName("Sure");
    //        sureBtn.off(cc.Node.EventType.TOUCH_END);
    //        sureBtn.on(cc.Node.EventType.TOUCH_END, function(event) {
    //        this.onShareBtnClick(function(isCompleted) {
    //            cb(isCompleted)
    //                if (isCompleted)
    //            {
    //                this.SharaView.active = false;
    //            }
    //        }.bind(this));
    //        soundManager.playSound("btnClick");
    //    }, this);

    //    var laterBtn = this.SharaView.getChildByName("Bg").getChildByName("Later");
    //    laterBtn.off(cc.Node.EventType.TOUCH_END);
    //    laterBtn.on(cc.Node.EventType.TOUCH_END, function(event) {
    //        this.SharaView.active = false;
    //        soundManager.playSound("btnClick");
    //    }, this);
    //    },

    //    //分享按钮
    //    void onShareBtnClick(cb)
    //    {
    //        var score = player.itemArrayGet("pScore", 6);
    //        SDK().share(score, function(isCompleted) {
    //            if (isCompleted)
    //            {//分享激励
    //                console.log("share:" + score);
    //                if (cb != null)
    //                {
    //                    cb(true)
    //                    }
    //            }
    //            else
    //            {
    //                this.fbFail(2);
    //            }
    //        }.bind(this));
    //    },

    //    //飞行礼包
    //    void flyGift()
    //    {
    //        var randomType = Math.floor(Math.random() * 2.99);
    //        effectManager.flyGift(randomType, function(giftPos) {
    //            var val = Math.random();
    //            if (val < 0)
    //            {
    //                viewManager.popView("FlyGiftView", true, function(view) {
    //                    var bg = cc.find("Bg", view);
    //                    //初始化
    //                    var moneyView = cc.find("Bg/Money", view);
    //                    var DiamondView = cc.find("Bg/Diamond", view);
    //                    var okBtn = cc.find("Bg/OK", view);
    //                    var moreBtn = cc.find("Bg/More", view);
    //                    var okText = cc.find("Bg/OK/Text", view).getComponent("LocalizedLabel");
    //                    var moreText = cc.find("Bg/More/Text", view).getComponent("LocalizedLabel");
    //                    //绑定事件
    //                    okBtn.off("click");
    //                    okBtn.on("click", function() {
    //                        viewManager.popView("FlyGiftView", false);
    //                        moneyView.active = false;
    //                        DiamondView.active = false;
    //                    }.bind(this), this)
    //                    moreBtn.off("click");
    //                    moreBtn.on("click", function() {
    //                        //分享按钮点击
    //                        gameApplication.onShareBtnClick(function(isOK) {
    //                            if (isOK)
    //                            {
    //                                gameApplication.DataAnalytics.doEvent("flyGiftShare");
    //                                soundManager.playSound("getCoin");
    //                                player.itemArrayAdd("pCurrency", 1, 5);
    //                                effectManager.flyReward(10, 1, mainScript.diamonds.node, giftPos, null, true);
    //                                gameApplication.checkDailyCount("flyGift", true);
    //                                moneyView.active = false;
    //                                DiamondView.active = false;
    //                                viewManager.popView("FlyGiftView", false);
    //                            }
    //                        }.bind(this))
    //                    }.bind(this), this)
    //                    //按钮字
    //                    okText.dataID = "lang.noThanksText";
    //                    moreText.dataID = "lang.shareText";
    //                    //显示界面
    //                    moneyView.active = false;
    //                    DiamondView.active = true;
    //                    bg.active = true;
    //                }.bind(this));
    //            }
    //            else if (val < 0)
    //            {
    //                //随机收益
    //                var randomMul = 10 + Math.random() * 5;
    //                var totalProfit = 0;
    //                for (var idx = 0; idx < mainScript.floorInfoList.length; idx = idx + 1)
    //                {
    //                    if (mainScript.floorInfoList[idx] != null && mainScript.floorInfoList[idx] != "undefined" && mainScript.floorInfoList[idx] != undefined)
    //                    {
    //                        totalProfit = totalProfit + (buildManager.countProfit(idx) / buildManager.countProfitTime(idx));
    //                    }
    //                }
    //                //弹窗询问是否进行翻倍
    //                viewManager.popView("FlyGiftView", true, function(view) {
    //                    //获得钱
    //                    player.itemArrayAdd("pCurrency", 0, totalProfit * randomMul);

    //                    var bg = cc.find("Bg", view);
    //                    //初始化
    //                    var moneyView = cc.find("Bg/Money", view);
    //                    var DiamondView = cc.find("Bg/Diamond", view);
    //                    var okBtn = cc.find("Bg/OK", view);
    //                    var moreBtn = cc.find("Bg/More", view);
    //                    var okText = cc.find("Bg/OK/Text", view).getComponent("LocalizedLabel");
    //                    var moreText = cc.find("Bg/More/Text", view).getComponent("LocalizedLabel");
    //                    var numText = cc.find("Bg/Money/Num", view).getComponent(cc.Label);
    //                    numText.string = gameApplication.countUnit(totalProfit * randomMul)[2];
    //                    //绑定事件
    //                    okBtn.off("click");
    //                    okBtn.on("click", function() {
    //                        moneyView.active = false;
    //                        DiamondView.active = false;
    //                        viewManager.popView("FlyGiftView", false);
    //                        effectManager.flyReward(10, 0, mainScript.coins.node, giftPos, null, true);
    //                        soundManager.playSound("getCoin");
    //                    }.bind(this), this)
    //                    moreBtn.off("click");
    //                    moreBtn.on("click", function() {
    //                        //视频按钮点击
    //                        gameApplication.onVideoBtnClick(function(isOK) {
    //                            if (isOK)
    //                            {
    //                                gameApplication.DataAnalytics.doEvent("flyGiftVideo");
    //                                moneyView.active = false;
    //                                DiamondView.active = false;
    //                                player.itemArrayAdd("pCurrency", 0, totalProfit * randomMul);
    //                                effectManager.flyReward(10, 0, mainScript.coins.node, giftPos, null, true);
    //                                gameApplication.checkDailyCount("flyGift", true);
    //                                viewManager.popView("FlyGiftView", false);
    //                                soundManager.playSound("getCoin");
    //                            }
    //                        }.bind(this), 0)
    //                    }.bind(this), this)
    //                    okText.dataID = "lang.receiveText";
    //                    moreText.dataID = "lang.watchText";
    //                    moneyView.active = true;
    //                    DiamondView.active = false;
    //                    bg.active = true;
    //                }.bind(this));
    //            }
    //            else
    //            {
    //                //随机收益
    //                var randomMul = 5 + Math.random() * 5;
    //                var totalProfit = 0;
    //                for (var idx = 0; idx < mainScript.floorInfoList.length; idx = idx + 1)
    //                {
    //                    if (mainScript.floorInfoList[idx] != null && mainScript.floorInfoList[idx] != "undefined" && mainScript.floorInfoList[idx] != undefined)
    //                    {
    //                        totalProfit = totalProfit + (buildManager.countProfit(idx) / buildManager.countProfitTime(idx));
    //                    }
    //                }
    //                //获得收益
    //                player.itemArrayAdd("pCurrency", 0, totalProfit * randomMul);
    //                effectManager.flyReward(10, 0, mainScript.coins.node, giftPos, null, true);
    //                soundManager.playSound("getCoin");
    //            }
    //        }.bind(this));
    //    },

    //    //FB失败界面
    void fbFail(int type) {
        ViewManager.GetInstance.popView("FbFail", true, (view) => {
            if (type == 1) {
                view.Find("Bg/VideoText").gameObject.SetActive(true);
                view.Find("Bg/ShareText").gameObject.SetActive(false);
            }
            else {
                view.Find("Bg/VideoText").gameObject.SetActive(false);
                view.Find("Bg/ShareText").gameObject.SetActive(true);
            }
            view.gameObject.SetActive(true);
        });

    }

    //    //提示窗
    //    void warnTips(dID, closeCb)
    //    {
    //        viewManager.popView("WarnView", true, function(view) {
    //            var text = cc.find("Bg/Text", view).getComponent("LocalizedLabel");
    //            text.dataID = dID;
    //            text.node.active = true;
    //            let close = cc.find("Bg/Close", view);
    //            close.on("click", function(event) {
    //        text.node.active = false;
    //        if (closeCb != null)
    //        {
    //            closeCb();
    //            closeCb = null;
    //        }
    //    }, this);
    //    }.bind(this));
    //},


    //    //互推按钮事件
    //    void popClick(event, type)
    //{
    //    SDK().switchGameAsync(type);
    //},

    //    //获取当前时间
    //    float getCurTime()
    //{
    //    var nowTime = new Date().getTime() / 1000;
    //    return parseFloat(nowTime);
    //},

    //    //计算时间
    //    void countTime(time)
    //{
    //    var tempMin = time / 60;
    //    var hor = 0;
    //    if (tempMin >= 60)
    //    {
    //        var count = Math.floor(tempMin / 60);
    //        hor = count;
    //        tempMin = (tempMin % 60);
    //    }
    //    var min = tempMin < 10 ? "0" + Math.floor(tempMin) : "" + Math.floor(tempMin);
    //    var sec = time % 60 < 10 ? "0" + Math.floor(time % 60) : "" + Math.floor(time % 60);
    //    if (time <= 0)
    //    {
    //        min = "00";
    //        sec = "00"
    //        }
    //    var string;
    //    if (hor > 0)
    //    {
    //        string = hor + ":" + min + ":" + sec;
    //    }
    //    else
    //    {
    //        string = min + ":" + sec;
    //    }
    //    return [string, hor, min, sec];
    //},

    //    //计算单位
    //    void countUnit(num)
    //{
    //    var old = num;
    //    var unit = 0;
    //    while (num >= 10000)
    //    {
    //        num = num * 0.001;
    //        unit = unit + 1;
    //    }
    //    var money = num.toFixed(2);
    //    if (gameApplication.unitCof == null)
    //    {
    //        return [money, unit, "$" + old.toFixed(2), money];
    //    }
    //    return [money, unit, ("$" + money + gameApplication.unitCof[unit].unit), (money + gameApplication.unitCof[unit].unit)];
    //},

    //    //互推按钮时间
    //    popClick(event, type)
    //{
    //    SDK().switchGameAsync(type);
    //},
}

