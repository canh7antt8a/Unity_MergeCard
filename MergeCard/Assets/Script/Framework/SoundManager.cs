using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SoundManager : MonoBehaviour {
    public bool isOpen = true;
    public bool isBgOpen = true;
    private AudioSource myAudio;
    //單例模式
    private static SoundManager _instance;
    public static SoundManager GetInstance {
        get {
            if (_instance == null) {
                _instance = new SoundManager();
            }

            return _instance;
        }
    }
    void Awake() {
        _instance = this;
        myAudio = gameObject.AddComponent<AudioSource>();
    }


    public void LoadBg() {
        ResManager.GetInstance.loadClip("Bg",
            (clip) => {
                if (clip != null) {
                    myAudio.clip = (AudioClip)clip;
                    myAudio.volume = 0.2f;
                    myAudio.Play();
                    myAudio.loop = true;
                }
                else {
                    Debug.Log("bg soundClip not exist!");
                }
            }
        );
    }

    //播放音效
    public void PlaySound(string soundtype) {
        if (isOpen) {
            if (ResManager.GetInstance != null) {
                ResManager.GetInstance.loadClip(soundtype,
                    (clip) => {
                        if (clip != null) {
                            myAudio.PlayOneShot((AudioClip)clip, 1.0f);
                        }
                        else {
                            Debug.Log(soundtype + " soundClip not exist!");
                        }
                    }
                );
            }
            else {
                Debug.Log("resManeger not exist!(资源管理器不存在！)");
            }
        }
    }

    //播放背景音效
    public void playBg() {
        if (isBgOpen) {
            myAudio.Play();
        }
        else {
            myAudio.Pause();
        }
    }

    //设置背景音效开关
    public void setBgOpen(bool isOpen) {
        isBgOpen = isOpen;
        playBg();
    }

    //设置音效开关
    public void setIsOpen(bool isOpen) {
        this.isOpen = isOpen;
    }

}
