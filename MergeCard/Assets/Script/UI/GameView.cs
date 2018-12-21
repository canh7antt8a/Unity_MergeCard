using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleJson;
using System;
using Random = UnityEngine.Random;
using DG.Tweening;

public class GameView : MonoBehaviour {
    public static GameView GetInstance { get; private set; }

    float cardYSpace = 50;
    int[] levelNeed = { 3000, 4000, 50, 6000, 7000 };
    Color[] colors = {
        new Color(0, 160f/255, 233f/255, 1),
        new Color(234f/255, 104f/255, 162f/255, 1),
        new Color(1, 99f/255, 51f/255, 1),
        new Color(1, 71f/255, 71f/255, 1),
        new Color(126f/255, 107f/255, 90f/255, 1),
        new Color(246f/255, 179f/255, 127f/255, 1),
        new Color(0, 153f/255, 68f/255, 1),
    };
    int curColor;

    //卡槽UI列表
    List<Transform> cardSlotList = new List<Transform>();
    //卡片的数据列表（对应每个卡槽）
    List<List<int>> cardDataList = new List<List<int>>();
    //卡片的UI列表（对应每个卡槽）
    List<List<Transform>> cardUIList = new List<List<Transform>>();
    //卡牌池的UI列表
    List<Transform> cardPoolList = new List<Transform>();
    //卡牌池的卡牌
    List<Transform> cardPoolCards = new List<Transform>();
    //卡牌预制件
    public GameObject cardPrefab;
    //当前分数
    [HideInInspector]
    public float curScore;
    //当前分数UI
    Text curScoreText;
    //最佳分数
    [HideInInspector]
    public float bestScore;
    //最佳分数UI
    Text bestScoreText;
    //预示线
    GameObject proline;
    //碰撞中的物体
    Transform collisioning;
    //是否放置在垃圾桶上
    bool onDisCard;
    //垃圾桶按钮
    Transform discardBtn;
    //垃圾桶数量UI
    int discardCount;
    //垃圾桶数量UI
    Text discardCountText;
    //是否正在剪切
    bool onCuting;
    //剪刀数量
    int cutCount;
    //剪刀数量UI
    Text cutCountText;
    //剪切提示
    GameObject cutTip;
    //万能牌数量
    int changeCount;
    //万能牌数量UI
    Text changeCountText;
    //道具要进行视频时的提示小图标
    GameObject[] adSpriteList = { null, null, null };
    //是否可以获取道具的时间
    float isGetTime;
    //是否进行引导
    bool isGuide;
    //道具特效
    public GameObject[] lightList;
    //等级进度条
    internal Slider levelPro;
    //左右两边的字
    Text[] levelTextList = { null, null };
    //等级分数
    public int[] levelNum = { 0, 0 };
    //有颜色的三个节点
    Image[] levelColorNode = { null, null, null };
    //新手引导
    GameObject guide;
    //游戏是否结束
    bool isOver;
    //当前皮肤的序号
    public int curSkinIdx;
    //皮肤配置
    internal int[] skinsNeed = { 5, 10, 15, 20, 25, 30, 35, 40, 45, 50, 55, 60, 65 };
    //皮肤是否使用过
    internal int[] skinsNew;
    //皮肤新提示
    private GameObject skinsSprite;

    void DestroyCard(Transform Card, int time) {
        StartCoroutine(GameApplication.GetInstance.DelayedAction(() => {
            EffectManager.GetInstance.ParticleShow(Card.position, "CutOff");
            Destroy(Card.gameObject);
        }, time * 0.2f));
    }

    //加载卡牌的图片
    void LoadCardSprite(int num, Image img) {
        var idx = curSkinIdx;
        if (num == 100 || num == 0) {
            idx = 100;
        }
        var name = "" + num;
        if (num == 100) {
            name = "X";
        }
        ResManager.GetInstance.loadSprite("GameSp/" + idx + "-card" + name, (spFrame) => {
            img.sprite = spFrame as Sprite;
        });
    }

    //加载皮肤
    public void LoadSkin(int idx) {
        PlayerPrefs.SetInt("skinidx", idx);
        curSkinIdx = idx;
        for (var i = 0; i < 4; i = i + 1) {
            //重载卡牌UI
            if (cardUIList.Count > i && cardUIList[ i ] != null) {
                if (cardUIList[ i ].Count > 0) {
                    for (var j = 0; j < cardUIList[ i ].Count; j = j + 1) {
                        var card = cardUIList[ i ][ j ].GetComponent<Card>();
                        LoadCardSprite(card.myNum, card.mySprite);
                    }
                }
            }
            //重载卡牌池的UI
            if (i < 2) {
                if (cardPoolCards.Count > i) {
                    if (cardPoolCards[ i ] != null) {
                        var card = cardPoolCards[ i ].GetComponent<Card>();
                        LoadCardSprite(card.myNum, card.mySprite);
                    }
                }
            }
        }
    }

    //清除数据
    internal void clearData() {
        for (var i = 0; i < 4; i = i + 1) {
            cardSlotList[ i ].gameObject.tag = "CanCollision";
            //清除卡牌的UI
            if (cardDataList.Count > i) {
                cardDataList[ i ].Clear();
            }//或者初始化
            else {
                cardDataList.Add(new List<int>());
            }
            if (cardUIList.Count > i && cardUIList[ i ] != null) {
                if (cardUIList[ i ].Count > 0) {
                    for (var j = 0; j < this.cardUIList[ i ].Count; j = j + 1) {
                        DestroyCard(cardUIList[ i ][ j ], j);
                    }
                }
                cardUIList[ i ].Clear(); ;
            }
            //清除卡牌池的UI
            if (i < 2) {
                if (cardPoolCards.Count > i) {
                    if (cardPoolCards[ i ] != null) {
                        Destroy(cardPoolCards[ i ].gameObject);
                        cardPoolCards[ i ] = null;
                    }
                }//或者初始化
                else {
                    cardPoolCards.Add(null);
                }
            }
        }
    }

    //初始化ui
    void initUI() {
        //卡槽和卡池UI
        for (var i = 0; i < 4; i = i + 1) {
            cardSlotList.Add(transform.Find("Gaming/CardSlot/Slot" + i));
            if (i < 2) {
                cardPoolList.Add(transform.Find("Gaming/CardPool/Pool" + i));
            }
        }
        //设置Y轴间隔
        var cardSlot = cardSlotList[ 0 ].parent;
        var Canvas = GameObject.Find("Canvas").GetComponent<RectTransform>();
        var height = (Canvas.sizeDelta.y + (cardSlot as RectTransform).sizeDelta.y) - 262;//-100是底部预留+172是卡牌的高度;
        cardYSpace = height * 1 / 7f;

        //道具数量UI
        discardBtn = transform.Find("UIView/Bottom/DiscardBtn");
        discardCountText = transform.Find("UIView/Bottom/DiscardBtn/Num").gameObject.GetComponent<Text>();
        cutCountText = transform.Find("UIView/Bottom/CutBtn/Num").gameObject.GetComponent<Text>();
        changeCountText = transform.Find("UIView/Bottom/ChangeBtn/Num").gameObject.GetComponent<Text>();
        cutTip = transform.Find("Gaming/Middle/CutTip").gameObject;

        //分数UI初始化
        bestScoreText = transform.Find("UIView/Top/BestScore/Num").gameObject.GetComponent<Text>();
        curScoreText = transform.Find("UIView/Top/CurScore").gameObject.GetComponent<Text>();

        //预示线
        proline = transform.Find("Gaming/CardPool/Proline").gameObject;

        //等级
        levelPro = transform.Find("UIView/Top/Level/LevelPro").GetComponent<Slider>();
        levelPro.value = 0;
        //等级的字
        levelTextList.SetValue(transform.Find("UIView/Top/Level/CurLevel/Num").gameObject.GetComponent<Text>(), 0);
        levelTextList.SetValue(transform.Find("UIView/Top/Level/NextLevel/Num").gameObject.GetComponent<Text>(), 1);
        //有颜色的节点
        levelColorNode.SetValue(transform.Find("UIView/Top/Level/CurLevel").gameObject.GetComponent<Image>(), 0);
        levelColorNode.SetValue(transform.Find("UIView/Top/Level/NextLevel").gameObject.GetComponent<Image>(), 1);
        levelColorNode.SetValue(transform.Find("UIView/Top/Level/LevelPro/Fill Area/Fill").gameObject.GetComponent<Image>(), 2);
        //视频小图标
        adSpriteList.SetValue(transform.Find("UIView/Bottom/ChangeBtn/AdSprite").gameObject, 0);
        adSpriteList.SetValue(transform.Find("UIView/Bottom/CutBtn/AdSprite").gameObject, 1);
        adSpriteList.SetValue(transform.Find("UIView/Bottom/DiscardBtn/AdSprite").gameObject, 2);

        skinsSprite = transform.Find("UIView/Top/Skins/NewSprite").gameObject;

        curSkinIdx = PlayerPrefs.GetInt("skinidx", 100);

        //随机获取等级的颜色
        var ran = Random.Range(0, colors.Length - 1);
        var color = getRandom(ran, 1, 7);
        curColor = color[ 0 ];
        levelColorNode[ 0 ].color = colors[ ran ];
        levelColorNode[ 2 ].color = colors[ ran ];
        levelColorNode[ 1 ].color = colors[ color[ 0 ] ];
    }

    //生成卡片
    Transform ProduceCard(int num) {
        var card = Instantiate(cardPrefab);
        var cardScript = card.GetComponent<Card>();
        cardScript.mask = card.transform.Find("Mask").gameObject;
        cardScript.mySprite = card.GetComponent<Image>();
        cardScript.myNum = num;
        LoadCardSprite(num, cardScript.mySprite);
        //ResManager.GetInstance.loadSprite("GameSp/card" + name, (spFrame) => {
        //    cardScript.mySprite.sprite = spFrame as Sprite;
        //});
        return card.transform;
    }

    //卡牌池生成卡牌
    void PoolCard(Action<Transform> cb) {
        var ramdon = Random.Range(0, 1f);
        int num = 0;
        if (ramdon <= 0.09) {
            num = 2;
        }
        else if (ramdon <= 0.27) {
            num = 4;
        }
        else if (ramdon <= 0.45) {
            num = 8;
        }
        else if (ramdon <= 0.63) {
            num = 16;
        }
        else if (ramdon <= 0.81) {
            num = 32;
        }
        else if (ramdon <= 0.99) {
            num = 64;
        }
        else if (ramdon <= 1) {
            num = 100;
        }
        if (num != 0) {
            //新牌的位置信息默认为第一张
            var targetIdx = 0;
            //第一张牌推前为第二张
            if (cardPoolCards[ 0 ] != null) {
                var card0 = cardPoolCards[ 0 ];
                var card0Script = card0.gameObject.GetComponent<Card>();
                card0Script.isReady = false;
                var pos = new Vector3(-95, 0, 0); //ViewManager.GetInstance.getUIPosition(card0,cardPoolList[ 1 ]);
                card0.SetParent(cardPoolList[ 1 ]);
                card0.localPosition = pos;
                var moveSqc = DOTween.Sequence();
                card0.DOScale(Vector3.one, 0.3f);
                moveSqc.Append(card0.DOLocalMove(Vector3.zero, 0.3f));
                moveSqc.AppendCallback(() => {
                    //成为第二个卡牌池
                    cardPoolCards[ 1 ] = card0;
                    card0Script.isReady = true;
                });
                moveSqc.Play();
            }
            else {
                if (cardPoolCards[ 1 ] == null) {
                    //如果第一张没有牌则默认为第二张
                    targetIdx = 1;
                }
            }

            //新牌推入第一张待命
            var card = ProduceCard(num);
            var cardScript = card.gameObject.GetComponent<Card>();
            card.SetParent(cardPoolList[ targetIdx ]);
            card.localScale = Vector3.one * 0.8f;
            if (targetIdx == 1) {
                card.localScale = Vector3.one;
            }
            card.localPosition = new Vector3(-95, 0, 0);
            card.gameObject.SetActive(true);

            //转向
            var turnSqc = DOTween.Sequence();
            turnSqc.Append(card.DOScale(new Vector3(0, 1, 1), 0.3f));
            turnSqc.AppendCallback(() => {
                cardScript.mask.SetActive(false);
            });
            turnSqc.Append(card.DOScale(Vector3.one, 0.3f));
            turnSqc.Play();

            //移动
            var moveSqc1 = DOTween.Sequence();
            moveSqc1.Append(card.DOLocalMove(Vector3.zero, 0.3f));
            moveSqc1.AppendCallback(() => {
                cardPoolCards[ targetIdx ] = card;
                if (targetIdx == 1) {
                    cardScript.isReady = true;
                }
                else {
                    JsonObject gameData = new JsonObject();
                    gameData[ "cardData" ] = cardDataList;
                    int[] pool = {
                    cardPoolCards[ 0 ].gameObject.GetComponent<Card>().myNum,
                    cardPoolCards[ 1 ].gameObject.GetComponent<Card>().myNum
                };
                    gameData[ "pool" ] = pool;
                    gameData[ "discard" ] = discardCount;
                    gameData[ "curScore" ] = curScore;
                    var dataString = SimpleJson.SimpleJson.SerializeObject(gameData);
                    PlayerPrefs.SetString("gameData", dataString);
                }
                if (cb != null) {
                    cb(card);
                }
            });
            moveSqc1.Play();
        }
    }


    //初始化数据
    void initData() {
        //PlayerPrefs.SetString("gameData", "0");
        var gameDataStr = PlayerPrefs.GetString("gameData", "0");
        if (gameDataStr != "0") {
            JsonObject gameData = (JsonObject)SimpleJson.SimpleJson.DeserializeObject(gameDataStr);
            JsonArray jarr = (JsonArray)gameData[ "cardData" ];

            //卡牌列表复原
            for (var i = 0; i < 4; i = i + 1) {
                if (cardDataList.Count > i) {
                    cardDataList[ i ].Clear();
                }
                else {
                    cardDataList.Add(new List<int>());
                }
                if (cardUIList.Count > i) {
                    cardUIList[ i ].Clear();
                }
                else {
                    cardUIList.Add(new List<Transform>());
                }
                JsonArray jarr1 = (JsonArray)jarr[ i ];
                //其中单个列表的复原
                for (var j = 0; j < jarr1.Count; j = j + 1) {

                    if (int.Parse(jarr1[ j ].ToString()) == 100) {
                        cardDataList[ i ].Add(64);
                    }
                    else {
                        cardDataList[ i ].Add(int.Parse(jarr1[ j ].ToString()));
                    }
                    var card = ProduceCard(cardDataList[ i ][ j ]);
                    var cardScript = card.GetComponent<Card>();

                    //遮罩去掉
                    cardScript.mask.SetActive(false);

                    card.SetParent(cardSlotList[ i ]);

                    card.localPosition = new Vector2(0, 0 - (j * cardYSpace) - 10);
                    card.localScale = new Vector3(1f, 1f, 1f);
                    //设置状态为已设置
                    cardScript.Seted();

                    cardUIList[ i ].Add(card);

                    //碰撞体的tag设置
                    if (j == jarr1.Count - 1) {
                        card.gameObject.tag = "CanCollision";
                    }
                    else {
                        card.gameObject.tag = "Seted";
                    }
                    //将上级赋值
                    if (j == 0) {
                        cardSlotList[ i ].gameObject.tag = "Seted";
                        cardScript.pro = cardSlotList[ i ];
                    }
                    else {
                        cardScript.pro = cardUIList[ i ][ j - 1 ];
                    }
                    card.gameObject.SetActive(true);
                }
            }
            //卡排池的卡牌复原
            var poolCard = (JsonArray)gameData[ "pool" ];
            for (var i = 0; i < poolCard.Count; i = i + 1) {
                var card = ProduceCard(int.Parse(poolCard[ i ].ToString()));
                var cardScript = card.GetComponent<Card>();
                //遮罩去掉
                cardScript.mask.SetActive(false);
                cardPoolCards[ i ] = card;
                card.SetParent(cardPoolList[ i ]);
                //区分大小
                if (i == 0) {
                    card.localScale = new Vector3(0.8f, 0.8f, 0.8f);
                }
                else {
                    card.localScale = new Vector3(1f, 1f, 1f);
                    cardScript.isReady = true;
                }
                card.localPosition = new Vector2(0, 0);
                card.gameObject.SetActive(true);
            }
            //垃圾桶数量以及分数
            discardCount = int.Parse(gameData[ "discard" ].ToString());
            discardCountText.text = this.discardCount + "";
            curScore = int.Parse(gameData[ "curScore" ].ToString());
            curScoreText.text = GameApplication.GetInstance.CountUnit(curScore)[ 3 ];
        }
        else {
            //初始化各类储存
            cardUIList.Clear();
            for (var i = 0; i < 4; i = i + 1) {
                if (cardDataList.Count > i) {
                    cardDataList[ i ].Clear();
                }
                else {
                    cardDataList.Add(new List<int>());
                }
                if (cardUIList.Count > i) {
                    cardUIList[ i ].Clear();
                }
                else {
                    cardUIList.Add(new List<Transform>());
                }
            }
            //生成卡片
            PoolCard((card) => {
                PoolCard(null);
            });
            //垃圾桶重置
            discardCount = 2;
            discardCountText.text = discardCount + "";
            //分数重置
            curScore = 0;
            curScoreText.text = GameApplication.GetInstance.CountUnit(curScore)[ 3 ];
            PlayerPrefs.SetInt("LevelScore", 0);
        }
        //获取储存起来的等级以及分数
        levelNum[ 0 ] = PlayerPrefs.GetInt("Level", 1);
        levelNum[ 1 ] = PlayerPrefs.GetInt("LevelScore", 0);
        levelTextList[ 0 ].text = levelNum[ 0 ] + "";
        levelTextList[ 1 ].text = (levelNum[ 0 ] + 1) + "";

        var needScore = 3000;
        if (levelNum[ 0 ] < 25) {
            needScore = levelNum[ 0 ] * levelNeed[ levelNum[ 0 ] / 5 ];
        }
        else {
            needScore = 25 * 7000;
        }
        //计算当前进度条的位置
        var pros = levelNum[ 1 ] / needScore;
        levelPro.value = pros;
    }

    //获取不相同的随机数
    int[] getRandom(int have, int count, int length) {
        List<int> arr = new List<int>();
        for (var i = 0; i < length; i = i + 1) {
            arr.Add(i);
        }
        if (have < length) {
            //去掉已存在的那个
            var center = arr[ have ];
            arr[ have ] = arr[ arr.Count - 1 ];
            arr[ arr.Count - 1 ] = center;
            arr.RemoveAt(arr.Count - 1);
        }
        //抽取所需要的数量
        List<int> need = new List<int>();
        var needIdx = 0;
        for (int i = 0; i < count; i = i + 1) {
            //随机抽取一个
            var ran = Random.Range(0, arr.Count);
            var center = arr[ ran ];
            arr.RemoveAt(ran);
            need.Add(center);
            needIdx = needIdx + 1;
        }
        return need.ToArray(); ;
    }

    //初始化皮肤
    void initSkins() {
        var obj = PlayerPrefs.GetString("SkinNew", "0");
        int[] skinNew = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        if (obj == "0") {
            skinsNew = skinNew;
            var dataString = SimpleJson.SimpleJson.SerializeObject(skinNew);
            PlayerPrefs.SetString("SkinNew", dataString);
        }
        else {
            JsonArray arry = (JsonArray)SimpleJson.SimpleJson.DeserializeObject(obj);
            for (var i = 0; i < arry.Count; i = i + 1) {
                skinNew[ i ] = int.Parse(arry[ i ].ToString());
            }
        }
        skinsNew = skinNew;
    }


    void Awake() {
        //单例初始化
        GetInstance = this;
        initUI();
        initSkins();
        var isFirst = PlayerPrefs.GetInt("isFirst", 0);
        if (isFirst != 1) {
            PlayerPrefs.SetInt("isFirst", 1);
            //第一次的剪刀数量
            setProp(1, 1);
            isGuide = true;
            JsonObject gameData = new JsonObject();
            List<List<int>> firstData = new List<List<int>>();
            for (var i = 0; i < 4; i++) {
                List<int> newList = new List<int>();
                if (i > 2) {
                    newList.Add(16);
                }
                if (i>1) {
                    newList.Add(8);
                }
                if (i>0) {
                    newList.Add(4);
                }
                newList.Add(2);
                firstData.Add(newList);
            }
            gameData[ "cardData" ] = firstData;
            int[] pool = { 2, 2 };
            gameData[ "pool" ] = pool;
            gameData[ "discard" ] = 2;
            gameData[ "curScore" ] = 0;
            var dataString = SimpleJson.SimpleJson.SerializeObject(gameData);
            PlayerPrefs.SetString("gameData", dataString);
        }
        else {
            //获取剪刀数量
            var val = PlayerPrefs.GetInt("cut", 0);
            setProp(val, 1);
            //获取万能卡数量
            val = PlayerPrefs.GetInt("change", 0);
            setProp(val, 0);
            bestScore = PlayerPrefs.GetFloat("bestScore", 0);
            bestScoreText.text = GameApplication.GetInstance.CountUnit(bestScore)[ 3 ];
        }
        //显示底部
        UPLTV.GetInstance.showBannerAdAtBottom();
        SoundManager.GetInstance.LoadBg();
        cardDataList = new List<List<int>>();
        gameStart();
    }

    void OnEnable() {
        InvokeRepeating("updataVal", -1, 1);
    }


    void OnDisable() {
        CancelInvoke("updataVal");
    }

    //处理小视频图标的显示
    void updataVal() {
        if (changeCount > 0) {
            changeCountText.gameObject.SetActive(true);
            adSpriteList[ 0 ].SetActive(false);
        }
        else {
            changeCountText.gameObject.SetActive(false);
            adSpriteList[ 0 ].SetActive(true);
        }
        if (cutCount > 0) {
            cutCountText.gameObject.SetActive(true);
            adSpriteList[ 1 ].SetActive(false);
        }
        else {
            cutCountText.gameObject.SetActive(false);
            adSpriteList[ 1 ].SetActive(true);
        }
        if (discardCount > 0) {
            discardCountText.gameObject.SetActive(true);
            adSpriteList[ 2 ].SetActive(false);
        }
        else {
            discardCountText.gameObject.SetActive(false);
            adSpriteList[ 2 ].SetActive(true);
        }
    }

    // Use this for initialization
    void Start() {
    }

    //游戏开始
    public void gameStart() {
        clearData();
        initData();
        isGuide = true;
        checkOver();
        if (isGuide) {
            isGuide = false;
            var guide = transform.Find("Guide");
            //Transform hand = transform.Find("Guide/Hand");
            guide.gameObject.SetActive(true);
            //var poolPos = cardPoolList[ 1 ].position;
            //var cardSlotPos = cardSlotList[ 2 ].position;
            //var Canvas = GameObject.Find("Canvas").GetComponent<RectTransform>();
            //cardSlotPos = cardSlotPos + new Vector3(Canvas.sizeDelta.x, Canvas.sizeDelta.y, 0)*0.5f;
            //Debug.Log(cardSlotPos);
            //hand.position = poolPos;
            //var MoveSqc = DOTween.Sequence();
            //MoveSqc.Append(hand.DOMove(cardSlotPos, 2).SetEase(Ease.Linear));
            //MoveSqc.AppendCallback(()=> {
            //    hand.position = poolPos;
            //});
            //MoveSqc.SetLoops(-1);
            //MoveSqc.Play();
        }
        isOver = false;
    }


    //移动处理
    public void cardMoving(Transform card, Vector3 pos) {
        //清除剪切状态
        onCuting = false;
        cutTip.SetActive(false);

        var touchPos = pos;
        var realPos = touchPos;//card.parent.convertToNodeSpaceAR(touchPos);
        if (realPos.y > 0) {
            realPos.y = realPos.y + realPos.y * 0.5f;
        }
        card.position = realPos;
    }

    //移动结束处理
    public void MoveOver(Card card) {
        var cardTran = card.transform;
        if (card.isMoving) {
            //是否可以放置
            if (collisioning != null) {
                var collisionScript = collisioning.GetComponent<Card>();
                //正在融合
                if (collisionScript != null && collisionScript.merging) {
                    cardTran.localPosition = Vector3.zero;
                }
                var guide = transform.Find("Guide");
                guide.gameObject.SetActive(false);
                SoundManager.GetInstance.PlaySound("PutCard");
                var idx = cardSlotList.IndexOf(collisioning);
                if (idx >= 0) {
                    //父级变动
                    cardTran.SetParent(collisioning);
                    cardTran.localPosition = new Vector3(0, -10, 0);
                    card.pro = collisioning;
                    //状态变化
                    card.Seted();
                    collisioning.gameObject.tag = "Seted";
                    card.gameObject.tag = "CanCollision";
                    //牌放上去要动画
                    var setSqc = DOTween.Sequence();
                    setSqc.Append(cardTran.DOScale(cardTran.localScale - (Vector3.one * 0.1f), 0.1f));
                    setSqc.Append(cardTran.DOScale(Vector3.one, 0.1f));
                    setSqc.Play();
                    //UI存储
                    cardUIList[ idx ].Add(card.transform);
                    //数据压入
                    var proScript = card.pro.GetComponent<Card>();
                    if (proScript == null && card.myNum == 100) {
                        cardDataList[ idx ].Add(64);
                    }
                    else {
                        cardDataList[ idx ].Add(card.myNum);
                    }
                }
                else {
                    idx = cardSlotList.IndexOf(collisioning.parent);
                    //如果已经满了而且无法合并则返回原位
                    if (cardUIList[ idx ].Count == 8 && collisionScript.myNum != card.myNum && card.myNum != 100) {
                        //提示框提示满队列了
                        /* viewManager.popView("TipsView", true, function (view) {
                            var tipText = cc.find("Bg/Text", view).getComponent("LocalizedLabel");
                            tipText.dataID = "lang.overLenth";
                            this.scheduleOnce(function () {
                                viewManager.popView("TipsView", false);
                            }.bind(this), 1);
                        }.bind(this)) */
                        cardTran.localPosition = Vector3.zero;
                        return;
                    }
                    //父级变动
                    cardTran.SetParent(collisioning.parent);
                    cardTran.localPosition = new Vector3(collisioning.localPosition.x, collisioning.localPosition.y - cardYSpace);
                    card.pro = collisioning;
                    //状态变化
                    card.Seted();
                    //碰撞中物体可碰撞状态消失
                    collisioning.gameObject.tag = "Seted";
                    //放上去的牌状态为可碰撞
                    card.gameObject.tag = "CanCollision";
                    //牌放上去要动画
                    var sequence = DOTween.Sequence();
                    sequence.Append(cardTran.DOScale(cardTran.localScale - (Vector3.one * 0.1f), 0.1f));
                    sequence.Append(cardTran.DOScale(Vector3.one, 0.1f));
                    //UI存储
                    cardUIList[ idx ].Add(cardTran);
                    //数据压入
                    cardDataList[ idx ].Add(card.myNum);
                }
                //清除当前选择体
                collisioning = null;
                proline.SetActive(false);
                //判断合并
                mergeCard(card, 0);
                //生成卡片
                PoolCard(null);
            }
            else if (onDisCard && discardCount > 0) {
                discardCard(card);
            }
            else {
                cardTran.localPosition = Vector3.zero;
            }
        }
        StartCoroutine(GameApplication.GetInstance.DelayedAction(() => {
            checkOver();
        }, 1));
    }


    //合并纸牌
    void mergeCard(Card card, int times) {
        var proScript = card.pro.GetComponent<Card>();
        var cardTran = card.transform;
        if (proScript == null) {
            //第一张是万能牌则变成64
            if (card.myNum == 100) {
                card.myNum = 64;
                card.gameObject.tag = "CanCollision";
                //读取图片
                LoadCardSprite(card.myNum, card.mySprite);
                //ResManager.GetInstance.loadSprite("GameSp/card" + card.myNum, (spFrame) => {
                //    card.mySprite.sprite = spFrame as Sprite;
                //});
            }
            return;
        }
        if (card.myNum == proScript.myNum || card.myNum == 100) {
            card.merging = true;
            StartCoroutine(GameApplication.GetInstance.DelayedAction(() => {
                var idx = cardSlotList.IndexOf(cardTran.parent);
                var cIdx = cardUIList[ idx ].IndexOf(cardTran);
                //UI弹出
                cardUIList[ idx ].RemoveAt(cIdx - 1);
                cardUIList[ idx ].RemoveAt(cIdx - 1);
                //数据弹出
                cardDataList[ idx ].RemoveAt(cIdx - 1);
                cardDataList[ idx ].RemoveAt(cIdx - 1);

                var mergeSqc = DOTween.Sequence();
                mergeSqc.Append(cardTran.DOLocalMove(card.pro.localPosition, 0.2f));
                mergeSqc.Append(cardTran.DOScale(Vector3.one * 0.8f, 0.1f));
                mergeSqc.AppendCallback(() => {
                    //卡片面值翻倍
                    if (card.myNum == 100) {
                        card.myNum = proScript.myNum * 2;
                    }
                    else {
                        card.myNum = card.myNum * 2;
                    }
                    //读取图片
                    LoadCardSprite(card.myNum, card.mySprite);
                    //ResManager.GetInstance.loadSprite("GameSp/card" + card.myNum, (spFrame) => {
                    //    card.mySprite.sprite = spFrame as Sprite;
                    //});
                    //旧卡上级储存
                    var oldPro = proScript.pro;
                    if (cardSlotList.IndexOf(card.pro) < 0) {
                        //旧卡销毁
                        Destroy(card.pro.gameObject);
                    }
                    //上级转变
                    card.pro = oldPro;
                    //2048爆炸特效
                    if (card.myNum == 2048) {
                        EffectManager.GetInstance.ParticleShow(cardTran.position, "Boom");
                        SoundManager.GetInstance.PlaySound("2048Sound");
                    }
                });
                SoundManager.GetInstance.PlaySound("Merge" + times);
                mergeSqc.Append(cardTran.DOScale(Vector3.one, 0.1f));
                mergeSqc.AppendCallback(() => {
                    //特效
                    if (times > 0) {
                        flyCombo(cardTran, times);
                    }
                    times = times + 1;
                    //如果合并后不是2048
                    if (card.myNum != 2048) {
                        //UI储存
                        cardUIList[ idx ].Insert(cIdx - 1, cardTran);

                        //数据储存
                        cardDataList[ idx ].Insert(cIdx - 1, card.myNum);

                        //如果不是最后一个就不能进行碰撞
                        if (cardUIList[ idx ].IndexOf(cardTran) != cardDataList[ idx ].Count - 1) {
                            card.gameObject.tag = "Seted";
                        }
                        else {
                            card.gameObject.tag = "CanCollision";
                        }

                        card.merging = false;
                        mergeCard(card, times);
                    }
                    else {
                        discardCount = discardCount + 2;
                        discardCountText.text = discardCount + "";
                        card.pro.gameObject.tag = "CanCollision";
                        Destroy(card.gameObject);
                    }
                    var addScore = card.myNum * times;
                    setScore(addScore);
                });
            }, 0.2f));
        }
        else {
            var idx = cardSlotList.IndexOf(cardTran.parent);
            //尝试下面的牌
            var cIdx = cardUIList[ idx ].IndexOf(cardTran);
            if (cIdx < cardUIList[ idx ].Count - 1) {
                mergeCard(cardUIList[ idx ][ cIdx + 1 ].GetComponent<Card>(), times);
            }
            //this.checkRewardCard(idx);
        }
    }

    //检测游戏结束
    void checkOver() {
        if (this.isOver) {
            return;
        }
        var isOver = 0;
        for (var i = 0; i < 4; i = i + 1) {
            if (cardDataList[ i ].Count >= 8) {
                isOver = isOver + 1;
            }
            if (cardPoolCards.Count > 1 && cardPoolCards[ 1 ] != null && cardDataList[ i ].Count > 7) {
                //还能放置
                if (cardPoolCards[ 1 ].GetComponent<Card>().myNum == 100 || cardPoolCards[ 1 ].GetComponent<Card>().myNum == cardDataList[ i ][ 7 ]) {
                    isOver = isOver - 1;
                }
            }
        }
        if (isOver >= 4) {
            this.isOver = true;
            SoundManager.GetInstance.PlaySound("Fail");
            ViewManager.GetInstance.popView("ReviveView", true, null);
        }
    }

    //丢弃一张牌
    void discardCard(Card card) {
        //卡牌移动时不能被丢掉
        if (!onDisCard && card.isMoving) {
            return;
        }
        discardCount = discardCount - 1;
        discardCountText.text = discardCount + "";
        var pos = discardBtn.position;

        SoundManager.GetInstance.PlaySound("Discard");
        var cardTran = card.transform;
        var discardSqc = DOTween.Sequence();
        discardSqc.Append(cardTran.DOScale(Vector3.zero, 0.4f));
        discardSqc.AppendCallback(() => {
            //放入垃圾桶
            Destroy(card.gameObject);
            //生成卡片
            PoolCard(null);
            //垃圾桶状态重置
            onDisCard = false;

            //记录使用垃圾桶
            //gameApplication.DataAnalytics.doEvent("discard");
        });
        discardSqc.Play();
        cardTran.DOMove(pos, 0.3f);
    }

    //剪切函数
    public void cutOff(Card card) {
        var cardTran = card.transform;
        if (onCuting && card.isSeted) {
            lightList[ 1 ].SetActive(false);
            cutTip.SetActive(false);
            //状态重置，并且剪刀数量减一
            onCuting = false;
            setProp(-1, 1);
            SoundManager.GetInstance.PlaySound("Discard");
            var cutSqc = DOTween.Sequence();

            cutSqc.Append(cardTran.DOScale(new Vector3(0.6f, 1.5f, 1), 0.15f));
            cutSqc.Append(cardTran.DOScale(new Vector3(1.2f, 0.8f, 1), 0.15f));
            cutSqc.Append(cardTran.DOScale(Vector3.zero, 0.1f));
            cutSqc.AppendCallback(() => {
                EffectManager.GetInstance.ParticleShow(cardTran.position, "CutOff");
            });

            StartCoroutine(GameApplication.GetInstance.DelayedAction(
                () => {
                    //获取所在列并
                    var idx = cardSlotList.IndexOf(cardTran.parent);
                    var goOverWrite = false;
                    var nextOneIdx = -1;
                    for (var i = 0; i < cardDataList[ idx ].Count; i = i + 1) {
                        if (goOverWrite) {
                            //UI前移
                            cardUIList[ idx ][ i - 1 ] = cardUIList[ idx ][ i ];
                            //数据前移
                            cardDataList[ idx ][ i - 1 ] = cardDataList[ idx ][ i ];
                            //下面的卡往上移动
                            cardUIList[ idx ][ i - 1 ].DOLocalMove(new Vector3(0, ((i - 1) * -cardYSpace) - 10, 0), 0.2f);
                        }
                        //检索到对应剪切卡片
                        if (cardTran == cardUIList[ idx ][ i ]) {
                            goOverWrite = true;
                            //把父级移交给被切得下一个
                            if (i + 1 < cardDataList[ idx ].Count) {
                                nextOneIdx = i;
                                cardUIList[ idx ][ i + 1 ].GetComponent<Card>().pro = card.pro;
                            }
                        }
                    }
                    //移除最后一张牌
                    var last = cardUIList[ idx ][ cardUIList[ idx ].Count - 1 ];
                    cardUIList[ idx ].RemoveAt(cardUIList[ idx ].Count - 1);
                    cardDataList[ idx ].RemoveAt(cardDataList[ idx ].Count - 1);
                    if (last == cardTran) {
                        card.pro.gameObject.tag = "CanCollision";
                    }

                    //销毁被剪切的卡片
                    Destroy(card.gameObject);
                    //判断能否合并
                    if (nextOneIdx != -1) {
                        mergeCard(cardUIList[ idx ][ nextOneIdx ].GetComponent<Card>(), 0);
                    }
                }, 0.7f)
            );
            //记录剪刀
            //gameApplication.DataAnalytics.doEvent("cutCard");
        }
    }


    //炸弹卡
    public void BoomCard() {
        SoundManager.GetInstance.PlaySound("BoomSound");
        isOver = false;
        //记录炸弹
        //gameApplication.DataAnalytics.doEvent("boomCard");

        for (var i = 0; i < 4; i = i + 1) {
            //最后可以被碰撞的牌
            Transform lastCard;
            var beginIdx = 0;
            if (cardUIList[ i ].Count > 3) {
                lastCard = cardUIList[ i ][ cardUIList[ i ].Count - 4 ];
                beginIdx = cardUIList[ i ].Count - 3;
            }
            else {
                lastCard = cardSlotList[ i ];
            }
            lastCard.gameObject.tag = "CanCollision";
            for (var j = cardUIList[ i ].Count - 1; j >= beginIdx; j = j - 1) {
                //将后面几张牌弹出销毁
                var popCard = cardUIList[ i ][ cardUIList[ i ].Count - 1 ];
                cardUIList[ i ].RemoveAt(cardUIList[ i ].Count - 1);
                cardDataList[ i ].RemoveAt(cardDataList[ i ].Count - 1);
                //播放特效
                EffectManager.GetInstance.ParticleShow(popCard.position, "Boom");

                Destroy(popCard.gameObject);
            }
        }
    }

    public void CollisionEnter(GameObject other, GameObject self) {
        //检测是否有可以放置的位置
        if (other.tag == "CanCollision" && self.tag == "Normal") {
            var oScript = other.GetComponent<Card>();
            var sScript = self.GetComponent<Card>();
            if (sScript.isMoving) {
                if (oScript != null) {
                    if (oScript.merging) {
                        return;
                    }
                }
                collisioning = other.transform;
                proline.SetActive(true);
                proline.transform.position = collisioning.position;//viewManager.getUIPosition(this.collisioning, this.proline.parent);
            }
        }
        //是否离开触及垃圾桶
        else if (other.tag == "Discard") {
            onDisCard = true;
        }
    }
    public void CollisionStay(GameObject other, GameObject self) {

    }
    public void CollisionExit(GameObject other, GameObject self) {
        //解除可防止的位置
        if (other.transform == collisioning) {
            collisioning = null;
            proline.SetActive(false);
        }

        //是否离开垃圾桶
        if (other.tag == "Discard") {
            onDisCard = false;
        }
    }

    public void checkNewSkin() {
        //显示皮肤是否有新的
        var idx = 0;
        var isHave = false;
        while (idx < skinsNeed.Length - 1 && levelNum[ 0 ] >= skinsNeed[ idx ]) {
            if (skinsNew[ idx ] == 0) {
                isHave = true;
                break;
            }
            idx++;
        }
        skinsSprite.SetActive(isHave);
        lightList[ 2 ].SetActive(isHave);
    }


    //设置分数
    void setScore(int val) {
        if (levelNum.Length > 0) {
            //等级处理
            levelNum[ 1 ] = levelNum[ 1 ] + val;
            int needScore = 3000;
            if (levelNum[ 0 ] < 25) {
                needScore = levelNum[ 0 ] * levelNeed[ levelNum[ 0 ] / 5 ];
            }
            else {
                needScore = 25 * 7000;
            }
            if (levelNum[ 1 ] > needScore) {
                levelNum[ 1 ] = levelNum[ 1 ] - needScore;
                levelNum[ 0 ] = levelNum[ 0 ] + 1;
                SetItem("Level", levelNum[ 0 ], 0);
                levelColorNode[ 0 ].color = levelColorNode[ 1 ].color;
                levelColorNode[ 2 ].color = levelColorNode[ 1 ].color;
                var color = getRandom(curColor, 2, 7);
                levelColorNode[ 1 ].color = colors[ color[ 1 ] ];
                curColor = color[ 1 ];

                //检测是否有新的皮肤
                checkNewSkin();

                //弹出升级
                ViewManager.GetInstance.popView("PassView", true, null);
            }
            levelTextList[ 0 ].text = levelNum[ 0 ] + "";
            levelTextList[ 1 ].text = (levelNum[ 0 ] + 1) + "";
            SetItem("LevelScore", levelNum[ 1 ], 0);
            var pros = levelNum[ 1 ] / needScore;
            levelPro.value = pros;
        }

        curScore = curScore + val;
        curScoreText.text = GameApplication.GetInstance.CountUnit(curScore)[ 3 ];
        //超越历史记录处理
        if (curScore >= bestScore) {
            //最高分处理
            bestScore = curScore;
            bestScoreText.text = GameApplication.GetInstance.CountUnit(bestScore)[ 3 ];
            SetItem("bestScore", bestScore, 1);
            //榜单处理
            //SDK().setRankScore(2, this.curScore, "{}");
        }
    }


    //点击事件处理
    public void menuClick(int type) {
        SoundManager.GetInstance.PlaySound("btnClick");
        if (type == 0) {
            if (changeCount > 0) {
                lightList[ 0 ].SetActive(false);
                //道具数量减少一个
                setProp(-1, 0);

                cardPoolCards[ 1 ].GetComponent<Card>().myNum = 100;
                var changeSqc = DOTween.Sequence();
                changeSqc.Append(cardPoolCards[ 1 ].DOScale(new Vector3(0, 1, 1), 0.1f));
                changeSqc.AppendCallback(() => {
                    //读取图片
                    LoadCardSprite(100, cardPoolCards[ 1 ].GetComponent<Image>());
                    //ResManager.GetInstance.loadSprite("GameSp/cardX", (spFrame) => {
                    //    cardPoolCards[ 1 ].GetComponent<Image>().sprite = spFrame as Sprite;
                    //});
                });
                changeSqc.Append(cardPoolCards[ 1 ].DOScale(Vector3.one, 0.1f));
            }
            else {
                GameApplication.GetInstance.onVideoBtnClick((isOk) => {
                    if (isOk == 2) {
                        this.setProp(1, 0);
                        lightList[ 0 ].SetActive(true);
                    }
                }, 0);

            }
        }
        else if (type == 1) {
            if (this.onCuting) {
                return;
            }
            if (cutCount > 0 && !onCuting) {
                onCuting = true;
                cutTip.SetActive(true);
            }
            else {
                GameApplication.GetInstance.onVideoBtnClick((isOk) => {
                    if (isOk == 2) {
                        this.setProp(1, 1);
                        lightList[ 1 ].SetActive(true);
                    }
                }, 0);
            }
        }
        else if (type == 2) {
            if (discardCount > 0) {
                discardCard(cardPoolCards[ 1 ].GetComponent<Card>());
                return;
            }
            GameApplication.GetInstance.onVideoBtnClick((isOk) => {
                if (isOk == 2) {
                    this.setProp(1, 2);
                }
            }, 0);
        }
        else if (type == 3) {
            ViewManager.GetInstance.popView("PauseView", true, null);
        }
        else if (type == 4) {
            var guideView = transform.Find("Guide").gameObject;
            guideView.SetActive(false);
        }
        else if (type == 5) {
            ViewManager.GetInstance.popView("SkinsView", true, null);
        }
    }



    //连击特效
    void flyCombo(Transform card, int num) {
        num = num + 1;
        //if (this.scoreTime > 0) {
        //    num = num * 3;
        //}
        EffectManager.GetInstance.flyText("X " + num, card);
    }



    //设置道具数量
    void setProp(int val, int type) {
        if (type == 0) {
            changeCount = changeCount + val;
            changeCountText.text = changeCount + "";
            PlayerPrefs.SetInt("change", changeCount);
        }
        else if (type == 1) {
            cutCount = cutCount + val;
            cutCountText.text = cutCount + "";
            PlayerPrefs.SetInt("cut", cutCount);
        }
        else if (type == 2) {
            discardCount = discardCount + 1;
            discardCountText.text = discardCount + "";
        }
        /* if (val > 0) {
            //获取道具的提示
            viewManager.popView("TipsView", true, function (view) {
                var tipText = cc.find("Bg/Text", view).getComponent("LocalizedLabel");
                if (type == 0) {
                    tipText.dataID = "lang.getChange";
                } else {
                    tipText.dataID = "lang.getCut";
                }
                this.scheduleOnce(function () {
                    viewManager.popView("TipsView", false);
                }.bind(this), 1);
            }.bind(this))
        } */
    }


    //储存数据
    void SetItem(string name, object val, int type) {
        switch (type) {
            case 0: {
                PlayerPrefs.SetInt(name, (int)val);
            }
            break;
            case 1: {
                PlayerPrefs.SetFloat(name, (float)val);
            }
            break;
            case 2: {
                PlayerPrefs.SetString(name, (string)val);
            }
            break;
        }
    }

    //获取数据
    object GetItem(string name, int type) {
        switch (type) {
            case 0: {
                return PlayerPrefs.GetInt(name, 0);
            }
            case 1: {
                return PlayerPrefs.GetFloat(name, 0);
            }
            case 2: {
                return PlayerPrefs.GetString(name, "0");
            }
        }
        return 0;
    }

    // Update is called once per frame
    void Update() {

    }

}


//    //分数卡
//    scoreCard() {
//    soundManager.playSound("ScoreCard");
//    //记录分数卡
//    gameApplication.DataAnalytics.doEvent("scoreCard");

//    if (this.scoreTime < 0) {
//        this.scoreTime = 0;
//    }
//    this.scoreTime = this.scoreTime + 20;
//    //飞动
//    effectManager.flyReward(1, 0, this.score3.node, null, function() {
//        this.score3.node.active = true;
//    }.bind(this))
//    },

//    

//    //检查奖励卡
//    checkRewardCard(idx) {
//    if (this.isGetTime > 0) {
//        return;
//    }
//    if (this.cardDataList[ idx ].length >= 8) {
//        var random = Math.random();
//        viewManager.popView("UseCardView", true, function(view) {
//            this.isGetTime = 120;
//            var sprite = cc.find("Bg/Card", view).getComponent(cc.Sprite);
//            var useBtn = cc.find("Bg/Use", view);
//            var desc = cc.find("Bg/Desc", view).getComponent("LocalizedLabel");
//            var closeBtn = cc.find("Bg/Close", view);
//            closeBtn.off("click");
//            closeBtn.on("click", function() {
//                sprite.node.active = false;
//                desc.node.active = true;
//                useBtn.off("click");
//                viewManager.popView("UseCardView", false);
//            }.bind(this), this);
//            //判断奖励
//            var cb = null;
//            if (random >= 0.5) {
//                cb = function() {
//                    this.isFirstShare = false;
//                    viewManager.popView("UseCardView", false, function() {
//                        this.scheduleOnce(function() {
//                            gameViewScript.boomCard();
//                        }.bind(this), 0.5)
//                        }.bind(this));
//                }.bind(this)
//                    desc.dataID = "lang.boomCardDesc";
//                resManager.loadSprite("UIView.boomCard", function(spFrame) {
//                    sprite.spriteFrame = spFrame;
//                    sprite.node.active = true;
//                }.bind(this))
//                }
//            else {
//                cb = function() {
//                    this.isFirstShare = false;
//                    gameViewScript.scoreCard();
//                    viewManager.popView("UseCardView", false);
//                }.bind(this)
//                    desc.dataID = "lang.X3CardDesc";
//                resManager.loadSprite("UIView.x3Card", function(spFrame) {
//                    sprite.spriteFrame = spFrame;
//                    sprite.node.active = true;
//                }.bind(this))
//                }
//            desc.node.active = true;
//            useBtn.off("click");
//            if (this.isFirstShare) {
//                useBtn.on("click", function() {
//                    cb();
//                    gameApplication.onShareBtnClick(function(isOK) {
//                        if (isOK) {
//                        }
//                    }.bind(this))
//                    }.bind(this), this)
//                }
//            else {
//                useBtn.on("click", function() {
//                    gameApplication.onVideoBtnClick(function(isOk) {
//                        if (isOk) {
//                            cb();
//                        }
//                    }.bind(this), 0);
//                }.bind(this), this)
//                }
//        }.bind(this));
//    }
//},



