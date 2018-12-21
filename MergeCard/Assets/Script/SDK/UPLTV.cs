using Polymer;
using System;
using System.Collections.Generic;
using UnityEngine;

public class UPLTV : MonoBehaviour {
    public static UPLTV GetInstance { get; private set; }
    //插屏和横幅广告ID
    private string[] cpPlaceId = { "002", "003" };
    //激励视频ID
    private string cpCustomId = "001";

    private void Awake() {
        GetInstance = this;
        UPSDK.initPolyAdSDK(2);
    }

    //激励视频广告
    public void showRewardAd(Action<int> cb) {
        if (UPSDK.isRewardReady()) {
            UPSDK.UPRewardDidGivenCallback = (str0, str2) => {
                if (cb != null) {
                    cb(2);
                }
            };
            UPSDK.UPRewardDidAbandonCallback = (str0, str2) => {
                if (cb != null) {
                    cb(1);
                }
            };
            UPSDK.setRewardVideoLoadCallback(
                (str0, str2) => {
                },
                (str0, str2) => {
                    if (cb != null) {
                        cb(1);
                    }
                });
            UPSDK.showRewardAd(cpCustomId);
        }
        else {
            if (cb != null) {
                cb(0);
            }
        }
    }
    //插屏广告
    public void showInterstitialAd(Action<int> cb) {
        if (UPSDK.isInterstitialReady(cpPlaceId[ 0 ])) {
            UPSDK.setIntersitialLoadCallback(cpPlaceId[ 0 ],
                (str0, str2) => {
                    if (cb != null) {
                        cb(2);
                    }
                },
                (str0, str2) => {
                    if (cb != null) {
                        cb(1);
                    }
                });
            UPSDK.showInterstitialAd(cpPlaceId[ 0 ]);
        }
        else {
            if (cb != null) {
                cb(0);
            }
        }
    }
    //底部横幅
    public void showBannerAdAtBottom() {
        UPSDK.showBannerAdAtBottom(cpPlaceId[ 1 ]);
    }

    public void ExitGame() {
        UPSDK.onBackPressed();
    }

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }
}
