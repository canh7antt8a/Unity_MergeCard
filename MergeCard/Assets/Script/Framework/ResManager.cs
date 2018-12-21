using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ResManager : MonoBehaviour {

    //一个加载
    private class loadAction {
        public int type = 0;
        public string name = "";
        public System.Action<Object> cbAction = null;
    }
    //單例模式
    public static ResManager GetInstance { get; private set; }
    

    void Awake() {
        GetInstance = this;
    }

    private Stack actionList = new Stack();
    private Dictionary<string, Object> resSaver = new Dictionary<string, Object>();
    private bool isFree = true;

    //读取图片资源并生成储存
    public void loadSprite(string name, System.Action<Object> cb) {
        pushAction(1, name, cb);
    }

    //读取配置资源并生成储存
    public void loadConfig(string name, System.Action<Object> cb) {
        pushAction(2, name, cb);
    }

    //读取预制件资源并生成储存
    public void loadPrefab(string name, System.Action<Object> cb) {
        this.pushAction(3, name, cb);
    }

    //读取音频资源并生成储存
    public void loadClip(string name, System.Action<Object> cb) {
        pushAction(4, name, cb);
    }

    //读取龙骨资源并生成储存
    public void loadBone(string name, System.Action<Object> cb) {
        pushAction(5, name, cb);
    }

    //读取事件压入队列
    void pushAction(int type, string name, System.Action<Object> cb) {
        //读取缓存
        Object val = null;
        if (resSaver.TryGetValue(name, out val)) {
            cb(val);
            return;
        }
        //事件储存
        var loadAction = new loadAction();
        loadAction.type = type;
        loadAction.name = name;
        loadAction.cbAction = cb;
        actionList.Push(loadAction);
        //判断是否空闲
        if (isFree) {
            dealAction();
        }
    }

    //处理资源
    void dealAction() {
        var popAction = actionList.Pop();
        if (popAction != null) {
            isFree = false;
            loadAction action = (loadAction)popAction;
            if (action.name == "" || action.name == null) {
                Debug.Log("action.name is empty");
                dealAction();
                return;
            }
            Object res = null;
            switch (action.type) {
                case 1: {
                    //读取
                    res = Resources.Load<Object>("Texture/" + action.name);
                    if (res != null) {
                        Sprite sp = Sprite.Create(
                        res as Texture2D,
                        new Rect(0, 0, (res as Texture2D).width,
                        (res as Texture2D).height),
                        new Vector2(0.5f, 0.5f));
                        res = (Object)sp;
                    }
                }
                break;
                case 2: {
                    //读取
                    res = Resources.Load<Object>("Config/" + action.name);
                }
                break;
                case 3: {
                    //读取
                    res = Resources.Load<Object>("Prefab/" + action.name);
                }
                break;
                case 4: {
                    //读取
                    res = Resources.Load<Object>("Sound/" + action.name);
                }
                break;
                case 5: {
                    //读取
                    res = Resources.Load<Object>("Bone/" + action.name);
                }
                break;
            };
            //储存并回调
            if (res != null) {
                if (!resSaver.ContainsKey(action.name)) {
                    resSaver.Add(action.name, res);
                }
                action.cbAction(res);
            }
            else {
                Debug.LogWarning(action.name);
                action.cbAction(null);
            }
            isFree = true;
        }
        else {
            isFree = true;
        }
    }

    void Update() {
        if (actionList.Count > 0 && isFree) {
            dealAction();
        }
    }
}