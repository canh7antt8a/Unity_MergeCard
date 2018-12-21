
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Polymer
{
	public class PolyADCall {



		#if UNITY_IOS && !UNITY_EDITOR
			[DllImport("__Internal")]
			private static extern string initIosSDKByZone(string gameName, string funName, int zone);
			[DllImport("__Internal")]
			private static extern void showInterstitial(string cpPlaceId);
			[DllImport("__Internal")]
			private static extern void showReward(string cpCustomId);
			[DllImport("__Internal")]
			private static extern void showBannerTop(string placementid);
			[DllImport("__Internal")]
			private static extern void showBannerBottom(string placementid);
			[DllImport("__Internal")]
			private static extern void removeBannerAd(string placementid);
			[DllImport("__Internal")]
			private static extern bool isInterstitialReady(string placementid);
			[DllImport("__Internal")]
			private static extern bool isRewardReady();

			[DllImport("__Internal")]
			private static extern void showRewardDebugController();  

			[DllImport("__Internal")]
			private static extern void showInterstitialDebugController(); 

			[DllImport("__Internal")]
			private static extern string getAbtConfigForIos(string placementid);

			[DllImport("__Internal")]
			private static extern void initAbtConfigJsonForIos(string gameAccountId, bool completeTask,int isPaid, string promotionChannelName, string gender, int age, string tags);	

			[DllImport("__Internal")]
			private static extern void setInterstitialLoadCallbackAt(string placementid);

			[DllImport("__Internal")]
			private static extern void setRewardloadCallback();

			[DllImport("__Internal")]
			private static extern void hideTopBanner();

			[DllImport("__Internal")]
			private static extern void hideBottomBanner();

			[DllImport("__Internal")]
			private static extern void setTopBannerPadingForIphonex(int padding);

			[DllImport("__Internal")]
			private static extern void loadAdsByManual();

			[DllImport("__Internal")]
			private static extern void updateAccessPrivacyInfoStatus(int value);

			[DllImport("__Internal")]
			private static extern void requestAuthorizationWithAlert(string gameName, string funName);

			[DllImport("__Internal")]
			private static extern int getCurrentAccessPrivacyInfoStatus();

			[DllImport("__Internal")]
			private static extern void checkIsEuropeanUnionUser(string gameName, string funName);

			[DllImport("__Internal")]
			private static extern void reportRDShowDid(string msg);

			[DllImport("__Internal")]
			private static extern void reportRDRewardGiven(string msg);

			[DllImport("__Internal")]
			private static extern void reportRDRewardCancel(string msg);

			[DllImport("__Internal")]
			private static extern void reportRDRewardClick(string msg);

			[DllImport("__Internal")]
			private static extern void reportRDRewardClose(string msg);

			[DllImport("__Internal")]
			private static extern void reportILShowDidCall(string placeid, string msg);

			[DllImport("__Internal")]
			private static extern void reportILClickCall(string placeid, string msg);

			[DllImport("__Internal")]
			private static extern void reportILCloseCall(string placeid, string msg);

			[DllImport("__Internal")]
			private static extern void reportInvokeMethodReceive(string msg);

			[DllImport("__Internal")]
			private static extern bool IsIosReportOnlineEnable();


		#elif UNITY_ANDROID && !UNITY_EDITOR
			private static AndroidJavaClass jc = null;
			private readonly static string JavaClassName = "com.up.ads.unity.PolyProxy";
			private readonly static string JavaClassStaticMethod_IniSDKByZone = "iniSDKByZone";
			private readonly static string JavaClassStaticMethod_ShowTopBanner = "showTopBanner";
			private readonly static string JavaClassStaticMethod_ShowBottomBanner = "showBottomBanner";
			private readonly static string JavaClassStaticMethod_RemoveBanner = "removeBanner";
			private readonly static string JavaClassStaticMethod_SetManifestPackageName = "setManifestPackageName";
			private readonly static string JavaClassStaticMethod_ShowInterstitial = "showInterstitial";
			private readonly static string JavaClassStaticMethod_ShowRewardVideo = "showRewardVideo";
			private readonly static string JavaClassStaticMethod_OnBackPressed = "onBackPressed";
			private readonly static string JavaClassStaticMethod_IsInterstitialReady = "isInterstitialReady";
			private readonly static string JavaClassStaticMethod_IsRewardReady = "isRewardReady";
			private readonly static string JavaClassStaticMethod_OnApplicationFocus = "onApplicationFocus";
			private readonly static string JavaClassStaticMethod_getAbtConfig = "getAbtConfig";
			private readonly static string JavaClassStaticMethod_initAbtConfigJson = "initAbtConfigJson";
			private readonly static string JavaClassStaticMethod_ShowVideoDebugActivity = "showVideoDebugActivity";
			private readonly static string JavaClassStaticMethod_ShowInterstitialDebugActivity = "showInterstitialDebugActivity";
			private readonly static string JavaClassStaticMethod_SetInterstitialCallbackAt = "setInterstitialCallbackAt";
			private readonly static string JavaClassStaticMethod_SetRewardVideoLoadCallback = "setRewardVideoLoadCallback";
			private readonly static string JavaClassStaticMethod_HideTopBanner = "hideTopBanner";
			private readonly static string JavaClassStaticMethod_HideBottomBanner = "hideBottomBanner";
			private readonly static string JavaClassStaticMethod_PrintToken = "printToken";
			private readonly static string JavaClassStaticMethod_LoadAnroidAdsByManual = "loadAnroidAdsByManual";
			private readonly static string JavaClassStaticMethod_updateAccessPrivacyInfoStatus = "updateAccessPrivacyInfoStatus";
			private readonly static string JavaClassStaticMethod_getAccessPrivacyInfoStatus = "getAccessPrivacyInfoStatus";
			private readonly static string JavaClassStaticMethod_notifyAccessPrivacyInfoStatus = "notifyAccessPrivacyInfoStatus";
			private readonly static string JavaClassStaticMethod_IsEuropeanUnionUser = "isEuropeanUnionUser";
			private readonly static string JavaClassStaticMethod_SetTopBannerTopPadding = "setTopBannerTopPadding";
			private readonly static string JavaClassStaticMethod_SetCustomerId = "setCustomerId";
			
			private readonly static string JavaClassStaticMethod_ReportRDShowDid = "reportRDShowDid";
			private readonly static string JavaClassStaticMethod_ReportRDRewardGiven = "reportRDRewardGiven";
			private readonly static string JavaClassStaticMethod_ReportRDRewardCancel = "reportRDRewardCancel";
			private readonly static string JavaClassStaticMethod_ReportRDRewardClick = "reportRDRewardClick";
			private readonly static string JavaClassStaticMethod_ReportRDRewardClose = "reportRDRewardClose";

			private readonly static string JavaClassStaticMethod_ReportILShowDid = "reportILShowDid";
			private readonly static string JavaClassStaticMethod_ReportILClick = "reportILClick";
			private readonly static string JavaClassStaticMethod_ReportILClose = "reportILClose";

			private readonly static string JavaClassStaticMethod_IsReportOnlineEnable = "isReportOnlineEnable";

			private readonly static string JavaClassStaticMethod_ReportIvokePluginMethodReceive = "reportIvokePluginMethodReceive";

		#else
		// "do nothing";
		#endif

		public string getPlatformName() {
			#if UNITY_IOS && !UNITY_EDITOR
			return "UNITY_IOS";
			#elif UNITY_ANDROID && !UNITY_EDITOR
			return "UNITY_ANDROID";
			#else
			return "unkown";
			#endif
		}

		public void loadUpAdsByManual() {
			#if UNITY_IOS && !UNITY_EDITOR
			loadAdsByManual();
			#elif UNITY_ANDROID && !UNITY_EDITOR
			if (jc != null) 
			{
			jc.CallStatic (JavaClassStaticMethod_LoadAnroidAdsByManual);
			}
			#endif
		}

		public void setTopBannerForAndroid(int padding) {
			#if UNITY_IOS && !UNITY_EDITOR

			#elif UNITY_ANDROID && !UNITY_EDITOR
			if (jc != null) 
			{	
			jc.CallStatic (JavaClassStaticMethod_SetTopBannerTopPadding, padding);
			}
			#endif
		}

		public void setTopBannerForIphonex(int padding) {
			#if UNITY_IOS && !UNITY_EDITOR
				setTopBannerPadingForIphonex(padding);
			#elif UNITY_ANDROID && !UNITY_EDITOR

			#endif
		}

		public void printInfo() {
			
			#if UNITY_IOS && !UNITY_EDITOR

			#elif UNITY_ANDROID && !UNITY_EDITOR
			if (jc != null) 
			{
			jc.CallStatic (JavaClassStaticMethod_PrintToken);
			}
			#endif
		}

		public PolyADCall() {
			PolyADSDKGameObject.getInstance ().setPolyADCall (this);
			#if UNITY_IOS && !UNITY_EDITOR
			#elif UNITY_ANDROID && !UNITY_EDITOR
			if (jc == null) {
				Debug.Log ("===> PolyADCall instanced");
				jc = new AndroidJavaClass (JavaClassName);
			}
			#endif
		}


		public bool IsReportOnlineEnable()
		{
			
			#if UNITY_IOS && !UNITY_EDITOR
			return IsIosReportOnlineEnable();
			#elif UNITY_ANDROID && !UNITY_EDITOR
			if (jc != null) 
			{
				return jc.CallStatic<bool> (JavaClassStaticMethod_IsReportOnlineEnable);
			}
			return false;
			#else
			return false;
			#endif

		}

		public void reportAdVideoRewardGiven(string msg) {
			#if UNITY_IOS && !UNITY_EDITOR
				reportRDRewardGiven(msg);
			#elif UNITY_ANDROID && !UNITY_EDITOR
			if (jc != null) 
			{
				jc.CallStatic (JavaClassStaticMethod_ReportRDRewardGiven, msg);
			}
			#endif
		}

		public void reportAdVideoRewardCancel(string msg) {
			#if UNITY_IOS && !UNITY_EDITOR
				reportRDRewardCancel(msg);
			#elif UNITY_ANDROID && !UNITY_EDITOR
			if (jc != null) 
			{
				jc.CallStatic (JavaClassStaticMethod_ReportRDRewardCancel, msg);
			}
			#endif
		}

		public void reportAdVideoClick(string msg) {
			#if UNITY_IOS && !UNITY_EDITOR
				reportRDRewardClick(msg);
			#elif UNITY_ANDROID && !UNITY_EDITOR
			if (jc != null) 
			{
				jc.CallStatic (JavaClassStaticMethod_ReportRDRewardClick, msg);
			}
			#endif
		}
			
		public void reportAdVideoClose(string msg) {
			#if UNITY_IOS && !UNITY_EDITOR
				reportRDRewardClose(msg);
			#elif UNITY_ANDROID && !UNITY_EDITOR
			if (jc != null) 
			{
				jc.CallStatic (JavaClassStaticMethod_ReportRDRewardClose, msg);
			}
			#endif
		}

		public void reportAdVideoShowDid(string msg) {
			
			#if UNITY_IOS && !UNITY_EDITOR
				reportRDShowDid(msg);
			#elif UNITY_ANDROID && !UNITY_EDITOR
			if (jc != null) 
			{
				jc.CallStatic (JavaClassStaticMethod_ReportRDShowDid, msg);
			}
			#endif
		}

		public void reportILDidShow(String cpPlaceId, String msg) {
			#if UNITY_IOS && !UNITY_EDITOR
				reportILShowDidCall(cpPlaceId, msg);
			#elif UNITY_ANDROID && !UNITY_EDITOR
			if (jc != null) 
			{
				jc.CallStatic (JavaClassStaticMethod_ReportILShowDid, cpPlaceId, msg);
			}
			#endif
		}

		public void reportILClick(String cpPlaceId, String msg) {
			#if UNITY_IOS && !UNITY_EDITOR
				reportILClickCall(cpPlaceId, msg);
			#elif UNITY_ANDROID && !UNITY_EDITOR
			if (jc != null) 
			{
				jc.CallStatic (JavaClassStaticMethod_ReportILClick, cpPlaceId, msg);
			}
			#endif
		}

		public void reportILClose(String cpPlaceId, String msg) {
			#if UNITY_IOS && !UNITY_EDITOR
				reportILCloseCall(cpPlaceId, msg);
			#elif UNITY_ANDROID && !UNITY_EDITOR
			if (jc != null) 
			{
				jc.CallStatic (JavaClassStaticMethod_ReportILClose, cpPlaceId, msg);
			}
			#endif
		}

		public void reportPluginInvokeMethodCall(string msg) {
			#if UNITY_IOS && !UNITY_EDITOR
				reportInvokeMethodReceive(msg);
			#elif UNITY_ANDROID && !UNITY_EDITOR
			if (jc != null) 
			{
				jc.CallStatic (JavaClassStaticMethod_ReportIvokePluginMethodReceive, msg);
			}
			#endif

		}
			
		public void RunCallbackAfterAppFocus(bool enable) {
			PolyADSDKGameObject.getInstance ().RunCallbackAfterAppFocus (enable);
		}

		// Use this for initialization
		public string initSDK (int azone) {



			#if UNITY_IOS && !UNITY_EDITOR
				string result = initIosSDKByZone(PolyADSDKGameObject.GameObject_Callback_Name,PolyADSDKGameObject.Java_Callback_Function, azone);
				if (UPSDK.UPSDKInitFinishedCallback != null) {
					UPSDK.UPSDKInitFinishedCallback (true, "UPSDK Init Ios Sdk Finish");
				}
				else if (PolyADSDK.AvidlySDKInitFinishedCallback != null) {
					PolyADSDK.AvidlySDKInitFinishedCallback (true, "UPSDK Init Ios Sdk Finish");
				}
				return result;

			#elif UNITY_ANDROID && !UNITY_EDITOR
			if (jc == null) {
				//Debug.Log (JavaClassName);
				jc = new AndroidJavaClass (JavaClassName);
			}
			string resule = jc.CallStatic<string> (JavaClassStaticMethod_IniSDKByZone, 
													PolyADSDKGameObject.GameObject_Callback_Name, 
													PolyADSDKGameObject.Java_Callback_Function,
													azone);
			if (UPSDK.UPSDKInitFinishedCallback != null) {
				UPSDK.UPSDKInitFinishedCallback (true, "UPSDK Init Android Sdk Finish");
			}
			else if (PolyADSDK.AvidlySDKInitFinishedCallback != null) {
				PolyADSDK.AvidlySDKInitFinishedCallback (true, "UPSDK Init Android Sdk Finish");
			}
			return resule;

			#else
			// "do nothing";
			if (PolyADSDK.AvidlySDKInitFinishedCallback != null) {
				PolyADSDK.AvidlySDKInitFinishedCallback (false, "UPSDK can't ini unkown platform");
			}
			return "initSDK ()";
			#endif



			//return "initSDK ()";
		}

		private string stringAryToString(string [] tags) {
			if (tags == null || tags.Length == 0) {
				return "";
			}

			string str = "{ \"array\":[";
			int len = tags.Length;
			for (int i = 0; i < len; i++) {
				str += "\"" + tags [i];
				if (i < len - 1) {
					str += "\",";
				} else {
					str += "\"]}";
				}
			}
			//Debug.Log ("stringAryToString:" + str);
			return str;
		}

		public void initAbtConfigJson(string gameAccountId, bool completeTask,
			int isPaid, string promotionChannelName,  string gender,
			int age, string[] tags){
			 
			#if UNITY_IOS && !UNITY_EDITOR
				initAbtConfigJsonForIos(gameAccountId, completeTask, isPaid, promotionChannelName, gender, age, stringAryToString(tags));
			#elif UNITY_ANDROID && !UNITY_EDITOR
			if (jc != null) 
			{
				jc.CallStatic (JavaClassStaticMethod_initAbtConfigJson, gameAccountId, completeTask, isPaid, promotionChannelName, gender, age, stringAryToString(tags));
			}
			#endif
		}

		public string getAbtConfig(string cpPlaceId){
			if (cpPlaceId == null) {
				Debug.Log ("===> call getAbtConfig(), the param cpPlaceId can't be null. ");
				return "";
			}
			#if UNITY_IOS && !UNITY_EDITOR
			return getAbtConfigForIos(cpPlaceId);
			#elif UNITY_ANDROID && !UNITY_EDITOR
			if (jc != null) 
			{
			return jc.CallStatic<string> (JavaClassStaticMethod_getAbtConfig, cpPlaceId);
			}
			return "";
			#else
				return "";
			#endif
		}

		public void setManifestPackageName(string packagename)
		{
			#if UNITY_IOS && !UNITY_EDITOR
			 
			#elif UNITY_ANDROID && !UNITY_EDITOR
			if (jc != null) 
			{
				jc.CallStatic (JavaClassStaticMethod_SetManifestPackageName, packagename);
			}
			#endif
		}

		public void setCustomerId(string curstomerId) {

			if (curstomerId == null) {
				Debug.Log ("===> fail to call setCustomerId(), curstomerId can't be null." );
				return;
			}

			#if UNITY_IOS && !UNITY_EDITOR
			Debug.Log ("===> setCustomerId() is not supported by IOS." );
			#elif UNITY_ANDROID && !UNITY_EDITOR
			if (jc != null) 
			{
				jc.CallStatic (JavaClassStaticMethod_SetCustomerId, curstomerId);
			}
			#endif
		}

		public void removeBanner(string cpPlaceId)
		{
			if (cpPlaceId == null) {
				Debug.Log ("===> call removeBanner(), the param cpPlaceId can't be null. ");
				return;
			}

			#if UNITY_IOS && !UNITY_EDITOR
				removeBannerAd(cpPlaceId);
			#elif UNITY_ANDROID && !UNITY_EDITOR
			if (jc != null) 
			{
				jc.CallStatic (JavaClassStaticMethod_RemoveBanner, cpPlaceId);
			}
			#endif
			 
		}

		public void setRewardVideoLoadFailCallback(Action<string, string> call) {
			PolyADSDKGameObject.getInstance ().setRewardVideoLoadFailCallback (call);
		}

		public void setRewardVideoLoadSuccessCallback(Action<string, string> call) {
			PolyADSDKGameObject.getInstance ().setRewardVideoLoadSuccessCallback (call);
		}

		public void addIntsLoadFailCallback(string cpPlaceId, Action<string, string> call) {
			PolyADSDKGameObject.getInstance ().addIntsLoadFailCallback (cpPlaceId, call);

		}

		public void addIntsLoadSuccessCallback(string cpPlaceId, Action<string, string> call) {
			PolyADSDKGameObject.getInstance ().addIntsLoadSuccessCallback (cpPlaceId, call);

		}

		public void isEuropeanUnionUser(Action<bool, string> callback) {
			PolyADSDKGameObject.getInstance ().setCheckEuropeanUserCallback (callback);
			#if UNITY_IOS && !UNITY_EDITOR
				checkIsEuropeanUnionUser(PolyADSDKGameObject.GameObject_Callback_Name,PolyADSDKGameObject.Java_Callback_Function);
			#elif UNITY_ANDROID && !UNITY_EDITOR
			if (jc != null) 
			{
			jc.CallStatic (JavaClassStaticMethod_IsEuropeanUnionUser, PolyADSDKGameObject.GameObject_Callback_Name,PolyADSDKGameObject.Java_Callback_Function);
			}
			#endif
		}

		public void notifyAccessPrivacyInfoStatus(Action<UPConstant.UPAccessPrivacyInfoStatusEnum, string> callback) {
			PolyADSDKGameObject.getInstance ().setAccessPrivacyInformationCallback (callback);

			#if UNITY_IOS && !UNITY_EDITOR
				requestAuthorizationWithAlert(PolyADSDKGameObject.GameObject_Callback_Name,PolyADSDKGameObject.Java_Callback_Function);
			#elif UNITY_ANDROID && !UNITY_EDITOR
			if (jc != null) 
			{
				jc.CallStatic (JavaClassStaticMethod_notifyAccessPrivacyInfoStatus, PolyADSDKGameObject.GameObject_Callback_Name, PolyADSDKGameObject.Java_Callback_Function);
			}
			#endif

		}

		public void setAccessPrivacyInfoStatus(UPConstant.UPAccessPrivacyInfoStatusEnum value) {
			int result = 0;
			switch (value) {
				case UPConstant.UPAccessPrivacyInfoStatusEnum.UPAccessPrivacyInfoStatusAccepted:
				{
					result = 1;
					break;
				}
				case UPConstant.UPAccessPrivacyInfoStatusEnum.UPAccessPrivacyInfoStatusDefined:
				{
					result = 2;
					break;
				}
				default:
				{
					result = 0;
					break;
				}
			}
			#if UNITY_IOS && !UNITY_EDITOR
				updateAccessPrivacyInfoStatus(result);
			#elif UNITY_ANDROID && !UNITY_EDITOR
			if (jc != null) 
			{
				jc.CallStatic (JavaClassStaticMethod_updateAccessPrivacyInfoStatus, result);
			}
			#endif
		}

		public UPConstant.UPAccessPrivacyInfoStatusEnum getAccessPrivacyInfoStatus() {


			#if UNITY_IOS && !UNITY_EDITOR
				int result = getCurrentAccessPrivacyInfoStatus();
				if (result == 1) {
					return UPConstant.UPAccessPrivacyInfoStatusEnum.UPAccessPrivacyInfoStatusAccepted;
				}
				else if (result == 2) {
					return UPConstant.UPAccessPrivacyInfoStatusEnum.UPAccessPrivacyInfoStatusDefined;
				}
			#elif UNITY_ANDROID && !UNITY_EDITOR
			if (jc != null) 
			{
				int result = jc.CallStatic<int> (JavaClassStaticMethod_getAccessPrivacyInfoStatus);
				if (result == 1) {
					return UPConstant.UPAccessPrivacyInfoStatusEnum.UPAccessPrivacyInfoStatusAccepted;
				}
				else if (result == 2) {
					return UPConstant.UPAccessPrivacyInfoStatusEnum.UPAccessPrivacyInfoStatusDefined;
				}
			}
			#endif

			return UPConstant.UPAccessPrivacyInfoStatusEnum.UPAccessPrivacyInfoStatusUnkown;
		}



		public void callRewardVideoLoadCallback() {
			#if UNITY_IOS && !UNITY_EDITOR
				setRewardloadCallback();
			#elif UNITY_ANDROID && !UNITY_EDITOR
			if (jc != null) 
			{
			jc.CallStatic (JavaClassStaticMethod_SetRewardVideoLoadCallback);
			Debug.Log ("callRewardVideoLoadCallback function 运行完成:" );
			}
			#endif
		}

		public void callInterstitialCallbackAt(string cpPlaceId) {
			if (cpPlaceId == null) {
				Debug.Log ("===> call setInterstitialCallbackAt(), the param cpPlaceId can't be null. ");
				return;
			}
			#if UNITY_IOS && !UNITY_EDITOR
			setInterstitialLoadCallbackAt(cpPlaceId);
			Debug.Log ("callInterstitialCallbackAt function cpPlaceId:" + cpPlaceId);
			#elif UNITY_ANDROID && !UNITY_EDITOR
			if (jc != null) 
			{
			jc.CallStatic (JavaClassStaticMethod_SetInterstitialCallbackAt, cpPlaceId);
			//Debug.Log ("callInterstitialCallbackAt function cpPlaceId:" + cpPlaceId);
			}
			#endif
		}

		public bool isInterstitialAdReady(string cpPlaceId)
		{
			if (cpPlaceId == null) {
				Debug.Log ("===> call isInterstitialAdReady(), the param cpPlaceId can't be null. ");
				return false;
			}
			#if UNITY_IOS && !UNITY_EDITOR
				return isInterstitialReady(cpPlaceId);
			#elif UNITY_ANDROID && !UNITY_EDITOR
			if (jc != null) 
			{
				return jc.CallStatic<bool> (JavaClassStaticMethod_IsInterstitialReady, cpPlaceId);
			}
			return false;
			#else
			return false;
			#endif

		}

		public bool isRewardAdReady()
		{
			#if UNITY_IOS && !UNITY_EDITOR
				return isRewardReady();
			#elif UNITY_ANDROID && !UNITY_EDITOR
			if (jc != null) 
			{
				return jc.CallStatic<bool> (JavaClassStaticMethod_IsRewardReady);
			}
			return false;
			#else
			return false;
			#endif

		}
	
		public void showInterstitialAd(string cpPlaceId)
		{
			if (cpPlaceId == null) {
				Debug.Log ("===> call isInterstitialAdReady(), the param cpPlaceId can't be null. ");
				return ;
			}
			#if UNITY_IOS && !UNITY_EDITOR
				showInterstitial(cpPlaceId);
			#elif UNITY_ANDROID && !UNITY_EDITOR

			if (jc != null) 
			{
				jc.CallStatic (JavaClassStaticMethod_ShowInterstitial, cpPlaceId);
			}
			#endif
		}

		public void hideBannerAtTop() {
			#if UNITY_IOS && !UNITY_EDITOR
				hideTopBanner();
			#elif UNITY_ANDROID && !UNITY_EDITOR

			if (jc != null) 
			{
			jc.CallStatic (JavaClassStaticMethod_HideTopBanner);
			}
			#endif
		}

		public void hideBannerAtBottom() {
			#if UNITY_IOS && !UNITY_EDITOR
				hideBottomBanner();
			#elif UNITY_ANDROID && !UNITY_EDITOR

			if (jc != null) 
			{
			jc.CallStatic (JavaClassStaticMethod_HideBottomBanner);
			}
			#endif
		}

		public void showRewardAd(string cpCustomId)
		{
			if (cpCustomId == null) {
				Debug.Log ("===> call showRewardAd(), the param cpCustomId be null. ");
				cpCustomId = "reward_vedio";
			}
			#if UNITY_IOS && !UNITY_EDITOR
			showReward(cpCustomId);
			#elif UNITY_ANDROID && !UNITY_EDITOR
			if (jc != null) 
			{
				jc.CallStatic (JavaClassStaticMethod_ShowRewardVideo, cpCustomId);
			}
			#endif
		}

		public void showBannerAdAtTop(string cpPlaceId) {
			if (cpPlaceId == null) {
				Debug.Log ("===> call showBannerAdAtTop(), the param cpPlaceId can't be null. ");
				return ;
			}
			#if UNITY_IOS && !UNITY_EDITOR
			showBannerTop(cpPlaceId);
			#elif UNITY_ANDROID && !UNITY_EDITOR
			if (jc != null) 
			{
				jc.CallStatic (JavaClassStaticMethod_ShowTopBanner, cpPlaceId);
			}
			#endif
			 
		}

		public void showBannerAdAtBottom(string cpPlaceId) {
			if (cpPlaceId == null) {
				Debug.Log ("===> call showBannerAdAtBottom(), the param cpPlaceId can't be null. ");
				return ;
			}
			#if UNITY_IOS && !UNITY_EDITOR
			showBannerBottom (cpPlaceId);
			#elif UNITY_ANDROID && !UNITY_EDITOR
			if (jc != null) 
			{
				jc.CallStatic (JavaClassStaticMethod_ShowBottomBanner, cpPlaceId);
			}
			#endif
		}

		public void onBackPressed() {
			#if UNITY_ANDROID && !UNITY_EDITOR
			if (jc != null) 
			{
			jc.CallStatic (JavaClassStaticMethod_OnBackPressed);
			}
			#endif
		}

		public void OnApplicationFocus(bool hasfoucus) {
			#if UNITY_ANDROID && !UNITY_EDITOR
			if (jc != null) 
			{
			jc.CallStatic (JavaClassStaticMethod_OnApplicationFocus, hasfoucus);
			}
			#endif
		}
 
		public void showRewardDebugView() {
			#if UNITY_IOS && !UNITY_EDITOR
				showRewardDebugController ();
			#elif   UNITY_ANDROID && !UNITY_EDITOR
			if (jc != null) 
			{
				jc.CallStatic (JavaClassStaticMethod_ShowVideoDebugActivity);
			}
			#endif
		}

		public void showInterstitialDebugView() {
			//
			#if UNITY_IOS && !UNITY_EDITOR
				showInterstitialDebugController();
				//Debug.Log ("===>sorry, this function is not supported. ");
			#elif  UNITY_ANDROID && !UNITY_EDITOR
			if (jc != null) 
			{
			jc.CallStatic (JavaClassStaticMethod_ShowInterstitialDebugActivity);
			}
			#endif
		}

//		private void showBannerAd(string cpPlaceId, int type) {
//			#if UNITY_IOS //&& !UNITY_EDITOR
//			showBanner(cpPlaceId, type);
//			#elif UNITY_ANDROID && !UNITY_EDITOR
//
//			#endif
//		}


	}
}
