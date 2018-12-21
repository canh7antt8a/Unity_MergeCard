using System;

namespace Polymer
{
	public class PolyBaseApi
	{
		// 插件版本号
		private readonly static string Version_Of_Ios_In_Plugin = "3005";

		private readonly static string Version_Of_Android_In_Plugin = "3005";
		private readonly static string Version_Of_Plugin = "3005";
	

		private static bool _sInited; 
		private static PolyADCall polyCall = null;

		public static string getVersionOfPlugin() {
			return Version_Of_Plugin;
		}

		/*
		 * 获取插件的版本号
		 * 此版本号一般取android与ios两者最大版本号
		 * 
		 */
		public static string getVersionOfPlatform() {
			#if UNITY_IOS && !UNITY_EDITOR
			return Version_Of_Ios_In_Plugin;
			#elif UNITY_ANDROID && !UNITY_EDITOR
			return Version_Of_Android_In_Plugin;
			#else
			return "undfined";
			#endif
		}

		/*
		 * 初始化avidly的聚合广告
		 * 即使多次调用，此方法也仅会初始化一次
		 * @param zone:0，海外；1，中国大陆；2，根据IP自动定位区域
		 * 具体引用：SDKZONE_FOREIGN，SDKZONE_CHINA，SDKZONE_AUTO
		 * from 2030,initPolyAdSDK()增加zone参数
		 */
		public static string initPolyAdSDK(int zone)
		{
			if (_sInited) {
				return "initPolyAdSDK finished";
			}
			if (polyCall == null) {
				polyCall = new PolyADCall ();
			}
			_sInited = true;
			return polyCall.initSDK (zone);
		}

		/*
		 * 初始化avidly的聚合广告abtest配置
		 * 
		 */
		public static void initAbtConfigJson(string gameAccountId, bool completeTask,
			int isPaid, string promotionChannelName,  string gender,
			int age, string[] tags) {
			if (polyCall != null) {
				polyCall.initAbtConfigJson (gameAccountId, completeTask, isPaid, promotionChannelName, gender, age, tags);
			}
		}

		/*
		 * 获取avidly的聚合广告abtest配置
		 * 返回结果为Json字符串，可能为null
		 * 
		 */
		public static string getAbtConfig(string placementId) {
			if (polyCall != null) {
				string r = polyCall.getAbtConfig (placementId);
				if (r == null || r.Length == 0 || r.Equals("{}")) {
					return null;
				} else {
					return r;
				}
			} else {
				return null;
			}
		}

		/*
		 * 判断avidly的插屏广告是否填充成功，此方法可用于检查广告是否可以展示
		 * @param cpPlaceId: 插屏广告位标识符
		 * 返回结果为bool值
		 * 
		 */
		public static bool isInterstitialReady(string cpPlaceId)
		{
			if (polyCall != null) {
				return polyCall.isInterstitialAdReady (cpPlaceId);
			}
			return false;
		}

		/*
		 * 判断avidly的激励视屏广告是否填充成功，此方法可用于检查广告是否可以展示
		 * 返回结果为bool值
		 * 
		 */
		public static bool isRewardReady()
		{
			if (polyCall != null) {
				return polyCall.isRewardAdReady ();
			}
			return false;
		}

		/*
		 * 用于展示avidly的插屏广告
		 * @param cpPlaceId: 插屏广告位标识符
		 * @deprecated 方法名拼写错误
		 * 建议使用showInterstitialAd()
		 */
		public static void showIntersitialAd(string cpPlaceId)
		{
			if (polyCall != null) {
				polyCall.showInterstitialAd (cpPlaceId);
			}
		}

		/*
		 * 用于展示avidly的插屏广告
		 * @param cpPlaceId: 插屏广告位标识符
		 * 用于替换showIntersitialAd()
		 */
		public static void showInterstitialAd(string cpPlaceId) {
			if (polyCall != null) {
				polyCall.showInterstitialAd (cpPlaceId);
			}
		}

		/*
		 * 用于展示avidly的激励视屏广告
		 * @param cpCustomId，用户自定义广告位，区分收益来源，不能为空，否则广告无法显示
		 */
		public static void showRewardAd(string cpCustomId)
		{
			if (polyCall != null) {
				polyCall.showRewardAd (cpCustomId);
			}
		}

		/*
		 * 增加对avidly的激励视屏广告加载的回调接口
		 * @param cpPlaceId: 第一个参数，插屏广告位标识符，不能为空或null
		 * @param success 第二个参数，加载成功后的回调
		 * @param fail 第三个参数，加载失败后的回调
		 * 
		 * 回调参数类型：Action<string,string>，第一个为cpPlaceId，广告位，可能为空或null；第二个参数为描述信息，可能为空或null
		 * supported from 2028
		 */
		public static void setIntersitialLoadCallback(string cpPlaceId, Action<string,string> success, Action<string, string> fail) {
			if (polyCall != null && cpPlaceId != null && cpPlaceId.Length > 0) {
				polyCall.addIntsLoadFailCallback (cpPlaceId, fail);
				polyCall.addIntsLoadSuccessCallback (cpPlaceId, success);
				polyCall.callInterstitialCallbackAt (cpPlaceId);
			}
		}

		/*
		 * 增加对avidly的激励视屏广告加载的回调接口
		 * @param success 第一个参数，加载成功后的回调
		 * @param fail 第二个参数，加载失败后的回调
		 * 
		 * 回调参数类型：Action<string,string>，第一个为cpPlaceId，广告位，可能为空或null；第二个参数为描述信息，可能为空或null
		 * supported from 2028
		 */
		public static void setRewardVideoLoadCallback(Action<string,string> success, Action<string, string> fail) {
			if (polyCall != null) {
				polyCall.setRewardVideoLoadFailCallback (fail);
				polyCall.setRewardVideoLoadSuccessCallback (success);
				polyCall.callRewardVideoLoadCallback ();
			}
		}

		/*
		 * 用于展示avidly的Banner广告
		 * 此类广告将自动展现在当前应用界面的顶部
		 * @param cpPlaceId: 插屏广告位标识符
		 * 
		 */
		public static void showBannerAdAtTop(string cpPlaceId)
		{
			if (polyCall != null) {
				polyCall.showBannerAdAtTop (cpPlaceId);
			}
		}

		/*
		 * 用于展示avidly的Banner广告
		 * 此类广告将自动展现在当前应用界面的底部
		 * @param cpPlaceId: 插屏广告位标识符
		 * 
		 */
		public static void showBannerAdAtBottom(string cpPlaceId)
		{
			if (polyCall != null) {
				polyCall.showBannerAdAtBottom (cpPlaceId);
			}
		}

		/*
		 * 隐藏当前顶部的Banner广告
		 * 再次展示时，需要调用showBannerAdAtTop()
		 * from 2037开始支持
		 */
		public static void hideBannerAdAtTop() {
			if (polyCall != null) {
				polyCall.hideBannerAtTop ();
			}
		}

		/*
		 * 隐藏当前底部的Banner广告
		 * 再次展示时，需要调用showBannerAdAtBottom()
		 * from 2037开始支持
		 */
		public static void hideBannerAdAtBottom() {
			if (polyCall != null) {
				polyCall.hideBannerAtBottom ();
			}
		}

		/*
		 * 根据广告位，删除avidly的Banner广告
		 * @param cpPlaceId: 插屏广告位标识符
		 */
		public static void removeBannerAdAt(string cpPlaceId)
		{
			if (polyCall != null) {
				polyCall.removeBanner (cpPlaceId);
			}
		}

		/*
		 * 用于向avidly设置安卓平台Manifest中定义的PackageName
		 * Manifest中定义的PackageName与最终包名是不一样，为了避免admob的广告无法显示
		 * 在此情况下，请向avidly正确配置Manifest中定义的PackageName
		 * 自己2031版本，adomb不再要求调用此方法
		 * 对于ios平台，请忽略此方法
		 */
		public static void setManifestPackageName(string packagename) {
			if (polyCall != null && int.Parse(Version_Of_Android_In_Plugin) < 2031) {
				polyCall.setManifestPackageName (packagename);
			}
		}

		/*
		 * 用于展示avidly的在安卓平台的退出广告
		 * 
		 */
		public static void onBackPressed()
		{
			if (polyCall != null) {
				polyCall.onBackPressed ();
			}
		}

		public static void OnApplicationFocus(bool hasfoucus)
		{
			if (polyCall != null) {
				polyCall.OnApplicationFocus (hasfoucus);
			}
		}

		/*
		 * 用于展示avidly的激励视屏广告调试界面
		 * supported from 2028
		 */
		public static void showRewardDebugView() {
			if (polyCall != null) {
				polyCall.showRewardDebugView ();
			}
		}

		/*
		 * 用于展示avidly的插屏广告调试界面
		 * supported from 2028
		 */
		public static void showInterstitialDebugView() {
			if (polyCall != null) {
				polyCall.showInterstitialDebugView ();
			}
		}

		public static void printDebugInfo() {
			if (polyCall != null) {
				polyCall.printInfo ();
			}
		}

		/**
     	* 满足需求：不希望在初始化自动加载广告，且要求根据游戏自主选择合适的时机进行广告加载
     	* 运行条件：当sdk默认禁用广告自动加载的功能，且upltv后台云配也关闭此功能时
     	* 如果以上条件不成立，即使调用以下方法，SDK也会自动忽略
     	* supported from 3002
     	*/
		public static void loadAvidlyAdsByManual() {
			if (polyCall != null) {
				polyCall.loadUpAdsByManual ();
			}
		}

		/**
     	* 对Iphonex手机，顶部Banner被状态栏遮挡时，可以通过调节顶部Banner的偏移值，解决此问题
     	* @param padding: 顶部Banner的偏移值，如32，则状态样会向下偏移32像素
     	* supported from 3002
     	*/
		public static void setTopBannerForIphonex(int padding) {
			if (polyCall != null) {
				polyCall.setTopBannerForIphonex (padding);
			}
		}

		/**
     	* 对Android 华为P20手机，顶部Banner被状态栏遮挡时，可以通过调节顶部Banner的偏移值，解决此问题
     	* @param padding: 顶部Banner的偏移值，如32，则状态样会向下偏移75像素
     	* supported from 3004
     	*/
		public static void setTopBannerForHuaWeiP20(int padding) {
			if (polyCall != null) {
				polyCall.setTopBannerForAndroid (padding);
			}
		}

		/**
     	* 弹出授权窗口，向用户说明重要数据收集的情况并询问用户是否同意授权
     	* 如果用户拒绝授权将放弃相关数据的收集
     	* @param callback
     	* Version 3003 and above support this method
     	*/
		public static void notifyAccessPrivacyInfoStatus(Action<UPConstant.UPAccessPrivacyInfoStatusEnum, string> callback) {
			if (polyCall == null) {
				polyCall = new PolyADCall ();
			}
			polyCall.notifyAccessPrivacyInfoStatus (callback);
		}

		/**
     	* 外部进行GDPR授权时，将用户授权结果同步到UPSDK时，调用此方法
     	* @param enumValue
     	* Version 3003 and above support this method
     	*/
		public static void updateAccessPrivacyInfoStatus(UPConstant.UPAccessPrivacyInfoStatusEnum enumValue) {
			if (polyCall == null) {
				polyCall = new PolyADCall ();
			}
			polyCall.setAccessPrivacyInfoStatus (enumValue);
		}

		/**
     	* 获取用户授权结果
     	* return UPConstant.UPAccessPrivacyInfoStatusEnum
     	* Version 3003 and above support this method
     	*/
		public static UPConstant.UPAccessPrivacyInfoStatusEnum getAccessPrivacyInfoStatus() {
			if (polyCall == null) {
				polyCall = new PolyADCall ();
			}
			return polyCall.getAccessPrivacyInfoStatus ();
		}

		/**
     	* 判断用户是否属于欧盟地区
     	* 异步回调
     	* Version 3003 and above support this method
     	*/
		public static void isEuropeanUnionUser(Action<bool, string> callback) {
			if (polyCall == null) {
				polyCall = new PolyADCall ();
			}
			polyCall.isEuropeanUnionUser (callback);
		}


		/**
     	* 设置所有的回调均在游戏活跃时发生
     	* 对于unity 17.4的版本，在测试中发现苹果手机无法正确调用did_open的回调而且也无法监听OnApplicationPause(bool pauseStatus)
     	* 有监于此，增加此方法解决回调问题
     	* Version 3004 and above support this method
     	*/
		public static void runCallbackAfterAppFocus(bool enable) {
			if (polyCall == null) {
				polyCall = new PolyADCall ();
			}
			polyCall.RunCallbackAfterAppFocus (enable);
		}

		/**
		 * 向统计包传递CustomerId(仅Android支持)
		 * 请在初始化SDK之前调用
		 * 对于非GP的包，可以传androidid
		 * Version 3004 and above support this method
		 */
		public static void setCustomerIdForAndroid(string customerId) {
			if (null == polyCall) {
				polyCall = new PolyADCall ();
			}
			polyCall.setCustomerId (customerId);
		}
	}
}

