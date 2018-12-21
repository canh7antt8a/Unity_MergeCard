using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace Polymer
{

	public class PolyADSDK : PolyBaseApi
	{
		
		public static Action<bool, string> AvidlySDKInitFinishedCallback = null;
	 
		//reward ad
		public static Action<string, string> AvidlyRewardDidOpenCallback = null;
		public static Action<string, string> AvidlyRewardDidClickCallback = null;
		public static Action<string, string> AvidlyRewardDidCloseCallback = null; 
		public static Action<string, string> AvidlyRewardDidGivenCallback = null;
		public static Action<string, string> AvidlyRewardDidAbandonCallback = null;
		//Interstitial ad
		public static Action<string, string> AvidlyInterstitialDidShowCallback = null;
		public static Action<string, string> AvidlyInterstitialDidCloseCallback = null;
		public static Action<string, string> AvidlyInterstitialDidClickCallback = null;
		//banner ad
		public static Action<string, string> AvidlyBannerDidShowCallback = null;
		public static Action<string, string> AvidlyBannerDidClickCallback = null;
		public static Action<string, string> AvidlyBannerDidRemoveCallback = null;

		//public static Action<AvidlyAccessPrivacyInfoStatusEnum, string> AvidlyAccessPrivacyInfoCallback = null;

		#if UNITY_ANDROID && !UNITY_EDITOR
		public static Action<string> AvidlyExitAdDidShowCallback = null;
		public static Action<string> AvidlyExitAdDidClickCallback = null;
		public static Action<string> AvidlyExitAdDidClickMoreCallback = null;
		public static Action<string> AvidlyExitAdOnExitCallback = null;
		public static Action<string> AvidlyExitAdOnCancelCallback = null;

		#endif

	}

}
