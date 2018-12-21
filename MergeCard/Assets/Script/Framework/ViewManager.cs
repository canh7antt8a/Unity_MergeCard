using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ViewManager : MonoBehaviour {
    public static ViewManager GetInstance { get; private set; }
    //当前界面切换的方向
    private int curDir = 0;
    //存取对应的界面Idx
    //private Dictionary<string, int> idxList = new Dictionary<string, int>();
    //存取对应idx的界面
    private Dictionary<string, Transform> viewList = new Dictionary<string, Transform>();
    //存取对应idx的界面
    private Dictionary<string, Transform> popSaver = new Dictionary<string, Transform>();
    //弹窗队列
    private Stack<string> curPopList = new Stack<string>();


    private void Awake() {
        GetInstance = this;
    }

    void judgeDir(bool isFOrB = true) {
        var ram = Random.Range(0f, 2f);
        ram = 2;
        if (isFOrB) {
            if (ram <= 1) {
                this.curDir = 1;
            }
            else if (ram <= 2) {
                this.curDir = 2;
            }
        }
        else {
            if (ram <= 1) {
                curDir = 3;
            }
            else if (ram <= 2) {
                curDir = 4;
            }
        }
    }

    //将生成的界面注册
    void RegView(string name, Transform cavView) {
        //添加索引并放置队尾
        viewList.Add(name, cavView);
    }

    /**
     * 本函数：界面的切换
     * name:node相对于Canvas的路径
     * inOut:显示还是隐藏
     * isFOrB:是前进的界面还是后退的界面
     * dir:指定切换方式
     * cb:延时需要做的操作
     * delayTrue：是否需要延时进行别的操作
     */
    public void showView(string name, bool inOut, bool isFOrB, System.Action<Transform> cb = null, float delayTrue = 0, int dir = -1) {
        if (dir == -1) {
            judgeDir(isFOrB);
        }
        else {
            curDir = dir;
        }
        //判断是否生成过，生成过则获取idx
        var idx = -1;
        if (viewList.ContainsKey(name)) {
            idx = 0;
        }
        Transform Canvas = GameObject.Find("Canvas").transform;
        //idx获取失败则生成
        if (idx == -1) {
            //首先从场景中尝试获取
            Transform cavView = Canvas.Find(name);
            //获取失败则进行生成生成后重新调用该方法
            if (cavView == null) {
                ResManager.GetInstance.loadPrefab(name, (prefab) => {
                    var obj = Instantiate((GameObject)prefab);
                    if (obj != null) {
                        cavView = obj.transform;
                    }
                    if (cavView != null) {
                        cavView.parent = Canvas;
                        showView(name, inOut, isFOrB, cb, delayTrue, dir);
                    }
                });
                return;
            }
            else {
                if (cavView != null) {
                    //添加索引并放置队尾
                    //RegView(name, cavView);
                    viewList.Add(name, cavView);
                    //添加索引并重新调用该方法
                    showView(name, inOut, isFOrB, cb, delayTrue, dir);
                }
                else {
                    Debug.Log(name + " is not found");
                    return;
                }
            }
            //控制大小
            //(cavView as RectTransform).sizeDelta = (Canvas as RectTransform).sizeDelta;
        }
        else {
            Transform curView = viewList[ name ];
            //显示想要显示的
            showAnim(curView, inOut, delayTrue, cb);
            //隐藏掉不显示的
            if (inOut) {
                IDictionaryEnumerator myenumerator = viewList.GetEnumerator();
                while (myenumerator.MoveNext()) {
                    Transform tempView = (Transform)myenumerator.Value;
                    if (tempView != curView) {
                        if (tempView.gameObject.activeSelf) {
                            //停止所有动作然后显示
                            //view.stopAllActions();
                            showAnim(tempView, false, 0, null);
                        }
                    }
                }
            }
        }
    }

    //选择显示方式
    void showAnim(Transform view, bool inOut, float delayTrue, System.Action<Transform> cb) {
        //选择显示方式
        if ((inOut && view.gameObject.activeSelf == false) || (!inOut && view.gameObject.activeSelf == true)) {
            //停止所有动作
            //view.stopAllActions();
            if (inOut) {
                view.SetSiblingIndex(view.parent.childCount);
            }
            showType(2, view, inOut, delayTrue, cb);
        }
    }

    //界面切换方式
    void showType(int type, Transform view, bool isOpen, float delayTrue, System.Action<Transform> cb) {
        var during = 0.5;
        var seq = 0;
        switch (type) {
            //渐隐渐现
            case 0: {
                if (isOpen) {
                    //view.opacity = 0;
                    view.gameObject.SetActive(true);
                    if (null != cb) {
                        cb(view);
                    }
                    noBtnMask(false, view);
                }
                else {
                    noBtnMask(true, view);
                    view.gameObject.SetActive(false);
                    if (null != cb) {
                        cb(view);
                    }
                }
            }
            break;
            //放大缩小
            case 1: {
                if (isOpen) {
                    //view.scale = 0;
                    view.gameObject.SetActive(true);
                    if (null != cb) {
                        cb(view);
                    }
                    noBtnMask(false, view);
                }
                else {
                    noBtnMask(true, view);
                    view.gameObject.SetActive(false);
                    if (null != cb) {
                        cb(view);
                    }
                }
            }
            break;
            //移动1右切左,2上切下为前进，3左切右,4下切上为后退
            case 2: {
                if (isOpen) {
                    //view.scale = 0;
                    view.gameObject.SetActive(true);
                    if (null != cb) {
                        cb(view);
                    }
                    noBtnMask(false, view);
                }
                else {
                    noBtnMask(true, view);
                    view.gameObject.SetActive(false);
                    if (null != cb) {
                        cb(view);
                    }
                }
            }
            break;
        };
    }

    //按钮屏蔽器
    void noBtnMask(bool isOpen, Transform view) {
        GameObject maskObj = null;
        var mask = view.Find("noBtn");
        if (mask == null) {
            mask = new GameObject("noBtn").transform;
            mask.parent = view;
            maskObj = mask.gameObject;
            maskObj.AddComponent<Mask>();
            maskObj.GetComponent<RectTransform>().sizeDelta = (view as RectTransform).sizeDelta;
        }
        if (mask != null) {
            maskObj = mask.gameObject;
            maskObj.SetActive(isOpen);
        }
    }

    //弹出弹窗
    public void popView(string name, bool isOpen, System.Action<Transform> cb) {
        //尝试从储存中获取
        Transform view = null;
        if (popSaver.ContainsKey(name)) {
            view = popSaver[ name ];
        }
        //尝试从场景中获取
        if (view == null) {
            var canvas = GameObject.Find("Canvas").transform;
            var cavView = canvas.Find(name);
            if (cavView != null) {
                InitPop(cavView, isOpen, cb);
            }
            //需要创建
            else {
                popSaver[ name ] = new GameObject("no").transform;
                ResManager.GetInstance.loadPrefab(name,
                    (prefab) => {
                        var obj = Instantiate((GameObject)prefab);
                        if (obj != null) {
                            view = obj.transform;
                        }
                        if (view != null) {
                            InitPop(view, isOpen, cb);
                        }
                    }
                    );
            }
        }
        else {
            if (view.gameObject.name == "no") {
                return;
            }
            popAnim(view, isOpen, cb);
        }
    }

    //弹窗动画
    void popAnim(Transform view, bool isOpen, System.Action<Transform> cb) {
        //整体结构对齐
        var Bg = view.Find("Bg");
        var widget = Bg as RectTransform;
        var time = 0.5;
        //弹出
        if (isOpen) {
            widget.anchoredPosition = new Vector2(0, widget.anchoredPosition.y);
            view.gameObject.SetActive(true);
            SoundManager.GetInstance.PlaySound("pop");
            view.SetSiblingIndex(view.parent.childCount);
            if (cb != null) {
                cb(view);
            }
            //隐藏之前的弹窗
            var old = "0";
            if (curPopList.Count > 0) {
                old = curPopList.Pop();
            }
            if (old != "0") {
                //获取上一个弹窗，并隐藏
                var oldView = popSaver[ old ];
                oldView.gameObject.SetActive(false);
                //将取出来的压回去
                curPopList.Push(old);
            }
            //将自己加入Pop列表
            curPopList.Push(view.name);
        }
        //缩回 
        else {
            SoundManager.GetInstance.PlaySound("closePop");
            view.gameObject.SetActive(false);
            if (cb != null) {
                cb(view);
            }
            //将自己移除pop列表
            if (curPopList.Count > 0) {
                var name = curPopList.Pop();
            }
            //将上一个弹窗显示
            var pre = "0";
            if (curPopList.Count > 0) {
                pre = curPopList.Pop();
            }
            if (pre != "0") {
                var preView = popSaver[ pre ];
                preView.gameObject.SetActive(true);
                curPopList.Push(pre);
            }
        }
    }

    void InitPop(Transform view, bool isOpen, System.Action<Transform> cb) {
        var canvas = GameObject.Find("Canvas").transform;
        //初始化关闭按钮
        var close = view.Find("Bg/Close");
        Button closeBtn = close.gameObject.GetComponent<Button>();
        if (closeBtn == null) {
            closeBtn = close.gameObject.AddComponent<Button>();
        }
        closeBtn.onClick.AddListener(delegate () {
            SoundManager.GetInstance.PlaySound("btnClick");
            popView(view.name, false, null);
        });
        //设置父级和层级
        view.parent = canvas;
        //储存界面
        popSaver[ view.name ] = view;
        popAnim(view, isOpen, cb);
    }

    //获取目标在target的坐标
    public Vector3 GetUIPosition(Transform myNode, Transform targetNode) {
        return Vector3.one;
    }

     void Update () {
    }
}
//    //初始化弹窗
//    initPopView() {
//        //读取弹窗信息
//        resManager.loadConfig("ResourceList", function (results) {
//            var cof = results.popViewList;
//            this.closeCount = 0;
//            this.loadAction(cof, cof.length, 0);
//        }.bind(this))
//    },

//    //生成并显示然后隐藏
//    loadAction(viewList, length, i) {
//        this.popView(
//            viewList[i],//界面名
//            true,//是否显示
//            //回调函数
//            function () {
//                var closeIdx = i;
//                i = i + 1;
//                if (i < length) {
//                    this.loadAction(viewList, length, i);
//                }
//                this.popAnim(this.popSaver[viewList[closeIdx]], false, function () {
//                    this.closeCount = this.closeCount + 1;
//                    //发出通知
//                    this.node.emit("loadPop", { pro: ((this.closeCount) / length) });
//                }.bind(this), true);
//            }.bind(this)
//        );
//    },


//    //移除界面
//    removeView(name) {
//        var view = this.viewList[this.idxList[name]];
//        this.idxList[name] = null;
//        for (var i = 0; i < this.viewList.length; i = i + 1) {
//            if (view == this.viewList[i]) {
//                this.viewList[i] = null;
//                //this.viewList.splice(i, 1);
//            }
//        }
//    },

//    /**
//     * 本函数：对一个数值进行平滑的变化
//     * start:起始值
//     * disVal:位移量
//     * time:时间
//     * callback:回调传回一个代理，代理的X参数即为我们需要的渐变值
//     * id:每个独立的操作都拥有一个Id来标识
//     */
//    lerpAction(start, disVal, time, callback, id) {
//        if (this.agent == null) {
//            this.agent = {};
//        }
//        if (this.agent[id] == null) {
//            this.agent[id] = new cc.Node("agent");
//            this.agent[id].x = 0;
//        }
//        this.agent[id].stopAllActions();
//        let ob = this.agent[id];
//        if (start != null) {
//            ob.x = start;
//        }
//        let repeatTime = time / 0.02;
//        let repeatVal = disVal / repeatTime;
//        ob.runAction(cc.repeat(
//            cc.sequence(
//                cc.callFunc(function () {
//                    callback(ob);
//                }.bind(this), this),
//                cc.moveBy(0.02, cc.v2(repeatVal, 0)),
//            )
//            , repeatTime + 1));
//    },

//    //停止一个渐变函数
//    stopLerpAction(id) {
//        if (this.agent[id] != null) {
//            this.agent[id].stopAllActions();
//        }
//    },

//    //停止所有渐变函数
//    stopAllLerpActions() {
//        for (var index in this.agent) {
//            if (this.agent[index] != null) {
//                this.agent[index].stopAllActions();
//            }
//        }
//    },

//    onLoad() {
//        window.viewManager = this;
//    },

//    //start() {},

//});
