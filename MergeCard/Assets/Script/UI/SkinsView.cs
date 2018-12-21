using SimpleJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkinsView : MonoBehaviour {
    public static SkinsView GetInstance { get; private set; }
    private Text unlockNum;
    private GridLayoutGroup SkinsContent;
    private GameObject SkinItem;
    private int[] skinsNew;
    private int skinCount = 0;
    private int curSkinIdx = 100;
    private List<Skin> skinsList = new List<Skin>();

    class Skin {
        public GameObject main;
        public Image mySprite;
        public GameObject newSprite;
        public GameObject lockSprite;
        public GameObject select;
        public LocalizationText desc;
    }

    void Awake() {
        var canvas = GameObject.Find("Canvas").GetComponent<RectTransform>();
        GetInstance = this;
        SkinsContent = transform.Find("Bg/Skins/Viewport/Content").GetComponent<GridLayoutGroup>();
        SkinItem = transform.Find("Bg/Skins/Viewport/Content/SkinItem").gameObject;
        var width = ( (canvas.sizeDelta.x - 180) - (130 * 3)) * 0.5f;
        SkinsContent.spacing = new Vector2(width, 10);
        unlockNum = transform.Find("Bg/Unlock/Num").GetComponent<Text>();
        //获取皮肤
        skinsNew = GameView.GetInstance.skinsNew;
    }


    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    void OnEnable() {
        skinCount = 0;
        unlockNum.text = skinCount + "/" + skinsNew.Length;
        if (skinsNew != null && skinsNew.Length > 0) {
            showSkins();
        }
    }

    void OnDisable() {
        GameView.GetInstance.checkNewSkin();
    }

    void showSkins() {
        for (var i = 0; i < skinsNew.Length; i = i + 1) {
            LoadSkinItem(i);
        }
    }

    //点击事件处理
    void LoadSkinItem(int idx) {
        Skin item;
        if (skinsList.Count <= idx) {
            skinsList.Add(new Skin());
            skinsList[ idx ].main = Instantiate(SkinItem);
            skinsList[ idx ].main.transform.SetParent(SkinsContent.transform);
            skinsList[ idx ].desc = skinsList[ idx ].main.transform.Find("Pros").GetComponent<LocalizationText>();
            skinsList[ idx ].desc.key = "Skin" + idx;
            skinsList[ idx ].desc.setValue();
            skinsList[ idx ].mySprite = skinsList[ idx ].main.transform.Find("SkinImage").GetComponent<Image>();
            skinsList[ idx ].select = skinsList[ idx ].main.transform.Find("Select").gameObject;
            skinsList[ idx ].lockSprite = skinsList[ idx ].main.transform.Find("LockSprite").gameObject;
            skinsList[ idx ].newSprite = skinsList[ idx ].main.transform.Find("NewSprite").gameObject;

            ResManager.GetInstance.loadSprite("GameSp/" + idx + "-card16", (sf) => {
                skinsList[ idx ].mySprite.sprite = sf as Sprite;
            });

            skinsList[ idx ].main.AddComponent<Button>().onClick.AddListener(() => {
                SoundManager.GetInstance.PlaySound("btnClick");
                //满足条件
                if (GameView.GetInstance.levelNum[ 0 ] >= GameView.GetInstance.skinsNeed[idx]) {
                    curSkinIdx = idx;
                    //选择框变换
                    if (GameView.GetInstance.curSkinIdx != 100) {
                        skinsList[ GameView.GetInstance.curSkinIdx ].select.SetActive(false);
                    }
                    skinsList[ idx ].select.SetActive(true);

                    //调用Gamevire方法加载皮肤
                    GameView.GetInstance.LoadSkin(idx);
                    skinsNew[ idx ] = 1;
                    GameView.GetInstance.skinsNew[ idx ] = 1;
                    skinsList[ idx ].newSprite.SetActive(false);

                    //保存使用记录
                    var dataString = SimpleJson.SimpleJson.SerializeObject(skinsNew);
                    PlayerPrefs.SetString("SkinNew", dataString);
                }
            });
        }
        item = skinsList[ idx ];
        if (GameView.GetInstance.curSkinIdx == idx) {
            item.select.SetActive(true);
        }
        else {
            item.select.SetActive(false);
        }
        if (GameView.GetInstance.levelNum[ 0 ] >= GameView.GetInstance.skinsNeed[ idx ]) {
            item.lockSprite.SetActive(false);
            item.mySprite.gameObject.SetActive(true);
            skinCount++;
            unlockNum.text = skinCount + "/" + GameView.GetInstance.skinsNeed.Length;
            item.desc.gameObject.SetActive(false);
            if (skinsNew[ idx ] == 0) {
                item.newSprite.SetActive(true);
            }
            else {
                item.newSprite.SetActive(false);
            }
        }
        else {
            item.lockSprite.SetActive(true);
            item.mySprite.gameObject.SetActive(false);
            item.desc.gameObject.SetActive(true);
            item.mySprite.gameObject.SetActive(false);
        }
        item.main.SetActive(true);
    }
}
