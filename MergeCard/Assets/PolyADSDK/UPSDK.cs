using System;

namespace Polymer
{
	public class UPSDK : PolyBaseApi
	{
		
		public static Action<bool, string> UPSDKInitFinishedCallback = null;

		//reward ad
		public static Action<string, string> UPRewardDidOpenCallback = null;
		public static Action<string, string> UPRewardDidClickCallback = null;
		public static Action<string, string> UPRewardDidCloseCallback = null; 
		public static Action<string, string> UPRewardDidGivenCallback = null;
		public static Action<string, string> UPRewardDidAbandonCallback = null;
		//Interstitial ad
		public static Action<string, string> UPInterstitialDidShowCallback = null;
		public static Action<string, string> UPInterstitialDidCloseCallback = null;
		public static Action<string, string> UPInterstitialDidClickCallback = null;
		//banner ad
		public static Action<string, string> UPBannerDidShowCallback = null;
		public static Action<string, string> UPBannerDidClickCallback = null;
		public static Action<string, string> UPBannerDidRemoveCallback = null;

		#if UNITY_ANDROID && !UNITY_EDITOR
		public static Action<string> UPExitAdDidShowCallback = null;
		public static Action<string> UPExitAdDidClickCallback = null;
		public static Action<string> UPExitAdDidClickMoreCallback = null;
		public static Action<string> UPExitAdOnExitCallback = null;
		public static Action<string> UPExitAdOnCancelCallback = null;

		#endif
	}
}

