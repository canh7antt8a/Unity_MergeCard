using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EffectManager : MonoBehaviour {
    public static EffectManager GetInstance { get; private set; }
    private Transform effectView;
    //特效预制件储存
    private List<GameObject> effectList = new List<GameObject>();
    //礼包预制件储存
    private List<Transform> giftList = new List<Transform>();
    //粒子预制件储存
    private List<GameObject> particleList = new List<GameObject>();
    //图片资源列表
    private List<Image> picList = new List<Image>();

    private Dictionary<string, int> prefabMap = new Dictionary<string, int>();

    public GameObject TextObj;

    private Vector3 canvas;

    private void Awake() {
        GetInstance = this;
        canvas = GameObject.Find("Canvas").GetComponent<RectTransform>().sizeDelta;
        ResManager.GetInstance.loadPrefab("FlyText",
               (obj) => {
                   TextObj = ((GameObject)obj);
               }
        );
        //        scheduleOnce(function () {
        //            loadFont();
        //        }.bind(this), 40);
    }

    private void Start() {
    }

    public void SetEffect() {
        //设置特效界面
        if (effectView == null) {
            effectView = new GameObject("EffectView").transform;
            effectView.parent = GameObject.Find("Canvas").GetComponent<RectTransform>();
        }
    }

    //飞字
    public void flyText(string str, Transform target) {
        Text textObj = Instantiate(TextObj).GetComponent<Text>();
        textObj.fontStyle = FontStyle.Bold;
        textObj.text = str;
        textObj.color = new Color(1, 0.5f, 0, 1);
        textObj.fontSize = 50;
        textObj.rectTransform.sizeDelta = new Vector2(300, 60);
        textObj.transform.parent = effectView;
        //定位飞字
        Vector3 begin = target.position;
        textObj.transform.position = begin;
        //往上飞并消失最后销毁
        var flySqc = DOTween.Sequence();
        textObj.DOFade(0, 0.7f).SetEase(Ease.InFlash);
        flySqc.Append(textObj.transform.DOLocalMoveY(textObj.transform.position.y + 80, 0.5f));
        var scalSqc = DOTween.Sequence();
        scalSqc.Append(textObj.transform.DOScale(Vector3.one * 1.2f, 0.25f));
        scalSqc.Append(textObj.transform.DOScale(Vector3.one, 0.25f));
        flySqc.Play();
        scalSqc.Play();
        StartCoroutine(GameApplication.GetInstance.DelayedAction(
            () => {
                if (textObj != null) {
                    Destroy(textObj.gameObject);
                }
            }, 1f)
        );
        
        effectView.transform.SetAsLastSibling();
    }


    //显示粒子效果
    public void ParticleShow(Vector3 target, string name) {
        if (!prefabMap.ContainsKey(name)) {
            ResManager.GetInstance.loadPrefab(name,
                (obj) => {
                    particleList.Add((GameObject)obj);
                    var type = particleList.Count - 1;
                    prefabMap.Add(name, type);
                    ParticleShow(target, name);
                }
            );
            return;
        }
        var tpye = prefabMap[ name ];
        var effect = Instantiate(particleList[ tpye ]).transform;
        effect.parent = effectView;
        effect.position = target;
        effect.gameObject.SetActive(true);

        effectView.transform.SetAsLastSibling();
    }
}

//    loadFont(){
//        cc.loader.loadRes("fnt/Normal", function (err, font) {
//            if (err == null) {
//                this.fontType = font;
//            } else {
//                console.warn(action.name);
//                console.warn(err);
//            }
//        }.bind(this));
//    },

//    //奖励动画效果
//    flyReward(num, type, target, start, cb, isBoom = false) {
//        if (this.picList[type] == null) {
//            if (type == 0) {
//                //金币图用于获得金币的特效
//                resManager.loadSprite("SpParticle.0", function (spriteFrame) {
//                    this.picList[type] = spriteFrame;
//                    this.flyReward(num, type, target, start, cb, isBoom);
//                }.bind(this))
//            } else if (type == 1) {
//                //钻石图用于获得钻石的特效
//                resManager.loadSprite("SpGoods.sDiamond", function (spriteFrame) {
//                    this.picList[type] = spriteFrame;
//                    this.flyReward(num, type, target, start, cb, isBoom);
//                }.bind(this))
//            } else if (type == 2) {
//                //钻石图用于获得能量水的特效
//                resManager.loadSprite("SpProp.sEnergyDrinks", function (spriteFrame) {
//                    this.picList[type] = spriteFrame;
//                    this.flyReward(num, type, target, start, cb, isBoom);
//                }.bind(this))
//            }
//        } else {
//            //起点
//            let begin = cc.v2(0, 0);
//            if (start != null) {
//                begin = start.parent.convertToWorldSpaceAR(start.position);
//                begin = this.effectView.convertToNodeSpaceAR(begin);
//            }

//            //终点
//            let dis = cc.v2(0, 500);
//            if (target != null) {
//                dis = target.parent.convertToWorldSpaceAR(target.position);
//                dis = this.effectView.convertToNodeSpaceAR(dis);
//            }

//            for (var i = 0; i < num; i++) {
//                var reward = this.effectList.pop();
//                if (reward == null) {
//                    reward = new cc.Node(i);
//                    reward.addComponent(cc.Sprite);
//                }
//                var sp = reward.getComponent(cc.Sprite);
//                sp.spriteFrame = this.picList[type];
//                reward.active = false;
//                reward.parent = this.effectView;
//                reward.position = begin;
//                reward.active = true;
//                if (isBoom) {
//                    reward.x = reward.x + (Math.random() * 50 * (Math.random() < 0.5 ? -1 : 1));
//                    reward.y = reward.y + (Math.random() * 50 * (Math.random() < 0.5 ? -1 : 1));
//                }
//                this.flyAnim(reward, dis, cb, i);
//            }
//        }
//    },


//    //飞行移动动作
//    flyAnim(reward, dis, cb, i) {
//        reward.active = true;
//        reward.scale = 0;
//        this.scheduleOnce(function () {
//            reward.runAction(
//                cc.spawn(
//                    cc.moveTo(0.8, cc.v2(dis.x, dis.y)),
//                    cc.sequence(
//                        cc.scaleTo(0.4, 1.1),
//                        cc.scaleTo(0.4, 0),
//                        //用于只执行一次的
//                        cc.callFunc(function () {
//                            if (cb != null) {
//                                cb()
//                            }
//                            this.effectList.push(reward);
//                        }.bind(this), this)
//                    )
//                )
//            );
//        }.bind(this), i * 0.05);
//    },

//    //左右摇摆晃动
//    shake(node) {
//        node.runAction(cc.repeatForever(cc.sequence(
//            cc.rotateTo(0.1, 5).easing(cc.easeIn(2)),
//            cc.rotateTo(0.2, -5).easing(cc.easeIn(2)),
//            cc.rotateTo(0.2, 5).easing(cc.easeIn(2)),
//            cc.rotateTo(0.1, 0).easing(cc.easeIn(2)),
//            cc.delayTime(0.5)
//        )));
//    },

//    //模拟心跳
//    scaleUpAndDowm(node) {
//        node.runAction(
//            cc.repeatForever(
//                cc.sequence(
//                    cc.scaleTo(0.3, 1.1).easing(cc.easeIn(2)),
//                    cc.scaleTo(0.6, 0.9).easing(cc.easeIn(2)),
//                    cc.scaleTo(0.6, 1.1).easing(cc.easeIn(2)),
//                    cc.scaleTo(0.6, 0.9).easing(cc.easeIn(2)),
//                )
//            )
//        );
//    },

//    //飞行礼包
//    flyGift(type, cb) {
//        //飞行礼包的预制件
//        if (this.giftList[type] == null) {
//            resManager.loadPrefab("FlyGift" + type, function (prefab) {
//                this.giftList[type] = cc.instantiate(prefab);
//                this.giftList[type].parent = this.effectView;
//                this.flyGift(type, cb);
//            }.bind(this))
//        } else {
//            var gift = cc.instantiate(this.giftList[type]);
//            gift.parent = this.effectView;
//            gift.x = cc.winSize.width * -0.8;
//            gift.y = Math.random() * cc.winSize.height * 0.35 * (Math.random() < 0.5 ? -1 : 1);
//            gift.off("click");
//            gift.on("click", function () {
//                if (cb != null) {
//                    cb({ parent: gift.parent, position: gift.position });
//                    gift.stopAllActions();
//                    gift.destroy();
//                }
//            }.bind(this), this);
//            gift.active = true;
//            gift.runAction(
//                cc.sequence(
//                    cc.moveTo(10, cc.v2(cc.winSize.width * 0.6, gift.y)),
//                    cc.callFunc(function () {
//                        gift.destroy();
//                    }.bind(this), this)
//                )
//            )
//        }
//    },
//});
