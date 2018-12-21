using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteAnimation : MonoBehaviour {
    Image image = null;
    float wait = 0;
    float waitDelta = 0;
    float delta = 0;
    //是否播放
    bool isPlay = false;
    //是否循环
    bool isLoop = true;
    //是否旋转
    bool isRotat = true;
    //播放一次后是否重置
    bool isReset = false;
    //是否已经初始化
    bool isInited = false;
    //是否销毁
    bool isDestory = false;
    //缓存目标的id，如果发生变化，重新init
    string animName = "";
    float fps = 0;
    int index = 0;
    float scale = 1;
    System.Action callback = null;
    //当前播放的序列帧
    List<Sprite> sprites;
    //储存不同阶段的序列帧
    List<List<Sprite>> spritesArray;

    void Awake() {
        image = transform.GetComponent<Image>();
    }

    // Use this for initialization
    void Start() {
    }
    /**
     * 播放动画
     * id:boom_1 则boom_为id
     * num：帧动画的数量
     * offset：从几开始读图 例如为3，则从boom_3开始读图
     * stage：表示一个阶段，一个id可以有几个阶段，一个阶段是offset 到 offset + num的帧动画
     * fps：每秒的帧速度
     * isReset:是否重置
     * isLoop：是否循环播放
     * scale：是否定义大小
     * cb：回调函数
    */
    public void playSprites(string animName, int num, int stage, float fps, bool isReset, bool isLoop, bool isDestory = true, int offset = 0, float scale = 1, System.Action cb = null) {
        wait = 0;
        waitDelta = 0;
        isRotat = false;
        transform.localScale = new Vector3(scale, scale, scale);

        this.isLoop = isLoop;
        this.fps = fps;
        this.isReset = isReset;
        this.isDestory = isDestory;
        this.scale = scale;

        //已经初始化了
        if (isInited && this.animName == animName && spritesArray[ stage ] != null) {
            sprites = spritesArray[ stage ];
            play(cb);
        }
        else {
            this.animName = animName;
            spritesArray = new List<List<Sprite>>();
            spritesArray.Add(new List<Sprite>());
            sprites = spritesArray[ stage ];
            for (var i = offset; i < offset + num; i = i + 1) {
                loadSprite(this.animName + i, spritesArray[ stage ]);
            }
            isInited = true;
            play(cb);
        }
    }

    //加载sprite
    void loadSprite(string spritesName, List<Sprite> sprites) {
        ResManager.GetInstance.loadSprite(spritesName, (spriteRes) => {
            //Sprite sp = Sprite.Create(
            //    spriteRes as Texture2D,
            //    new Rect(0, 0, (spriteRes as Texture2D).width,
            //    (spriteRes as Texture2D).height),
            //    new Vector2(0.5f, 0.5f));
            sprites.Add(spriteRes as Sprite);
        });
    }

    /**
    * 根据idx播放单张图
    */
    public void playSpriteByIdx(int stage, int num) {
        image.sprite = spritesArray[ stage ][ num ];
        isPlay = false;
    }

    /**
    * 设置图
    */
    public void SetSprite(Sprite sp) {
        image.sprite = sp;
    }

    //开始播放
    void play(System.Action cb) {
        index = 0;
        isPlay = true;
        transform.localScale = new Vector3(scale, scale, scale);
        image.color = new Color(1, 1, 1, 1);

        if (cb != null) {
            callback = cb;
        }
    }

    //重置图片
    void reSet() {
        index = 0;
        if (image != null) {
            image.sprite = sprites[ index ];
        }
        isPlay = false;

        image.color = new Color(1, 1, 1, 1);
        transform.rotation = Quaternion.identity;

        if (callback != null) {
            callback();
        }
    }

    // Update is called once per frame
    void Update() {

        if (wait > 0 && waitDelta < wait) {
            waitDelta += Time.deltaTime;
            return;
        }
        if (isPlay && fps > 0 && sprites.Count > 0) {

            delta += Time.deltaTime;

            var rate = 1 / fps;
            if (rate < delta) {
                delta = rate > 0 ? delta - rate : 0;
                if (index < sprites.Count) {
                    image.sprite = sprites[ index ];
                    image.SetNativeSize();
                    if (index + 1 == sprites.Count) {
                        waitDelta = 0;

                        //isLoop
                        if (!isLoop) {
                            isPlay = false;
                        }

                        if (isReset) {
                            reSet();
                        }

                        if (isDestory) {
                            Destroy(gameObject);
                        }
                    }
                }
                index = index + 1 >= sprites.Count ? 0 : index + 1;
            }
        }
    }
}
