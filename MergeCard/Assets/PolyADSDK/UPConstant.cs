using System;

namespace Polymer
{
	public class UPConstant
	{
		public enum UPAccessPrivacyInfoStatusEnum {
			UPAccessPrivacyInfoStatusUnkown   = 0,
			UPAccessPrivacyInfoStatusAccepted = 1,
			UPAccessPrivacyInfoStatusDefined  = 2,
			UPAccessPrivacyInfoStatusFailed  = 3
		}

		public const int SDKZONE_FOREIGN = 0; //海外
		public const int SDKZONE_CHINA = 1;   //中国大陆
		public const int SDKZONE_AUTO = 2;    //根据ip自动判断
	}
}

