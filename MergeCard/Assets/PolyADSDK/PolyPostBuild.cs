#if UNITY_EDITOR && !UNITY_WEBPLAYER && UNITY_IOS
using UnityEngine;

using UnityEditor;
using UnityEditor.Callbacks;
using System.IO;
using UnityEditor.XCodeEditor;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public static class PolyPostBuild
{
	[PostProcessBuild]
	public static void OnPostProcessBuild(BuildTarget target2, string path)
	{
#if UNITY_5 || UNITY_5_3_OR_NEWER
		if (target2 == BuildTarget.iOS)
#else
if (target2 == BuildTarget.iPhone)
#endif
		{
			UnityEditor.XCodeEditor.XCProject proj = new UnityEditor.XCodeEditor.XCProject(path);

			string[] projmods = System.IO.Directory.GetFiles(
				System.IO.Path.Combine(System.IO.Path.Combine(Application.dataPath, "PolyADSDK"), "Plugins"), "PolyADSDK.projmods", System.IO.SearchOption.AllDirectories);
			if(projmods.Length == 0)
			{
				Debug.LogWarning("[PolyPostBuild]PolyADSDK.projmods not found!");
			}
			foreach (string p in projmods)
			{
				proj.ApplyMod(p);
			}

			proj.AddOtherLinkerFlags ("-ObjC");
			proj.AddOtherLinkerFlags ("-fobjc-arc");
			proj.Save();


			// delete android assets file of avidly

			string avidlypath = System.IO.Path.Combine(System.IO.Path.Combine(proj.projectRootPath, "Data"), "Raw");
			Debug.Log ("==> path1:" + avidlypath);
			if (System.IO.Directory.Exists (avidlypath)) {
				avidlypath = System.IO.Path.Combine (avidlypath, "avidly_android");
				Debug.Log ("==> path2:" + avidlypath);
				if (System.IO.Directory.Exists (avidlypath)) {
					Debug.Log ("==> exist avidly_android, will delete allfiles" );
					string [] avidlyfiles = System.IO.Directory.GetFiles (avidlypath);
					foreach (string p in avidlyfiles)
					{
						Debug.Log ("==> to del:" + p );
						if (System.IO.File.Exists (p)) {
							System.IO.File.Delete (p);
							Debug.Log ("==> to del finish");
						}
					}
				}
			}

			string filePath = Path.GetFullPath (path);
			Debug.Log ("==> filePath " + filePath);
			string infofilePath = Path.Combine(filePath, "info.plist" );
			Debug.Log ("==> infofilePath " + infofilePath);

			if( !System.IO.File.Exists( infofilePath ) ) {
				Debug.LogError( infofilePath +", 路径下文件不存在" );
				return;
			}

			Dictionary<string, object> dict = (Dictionary<string, object>)PlistCS.Plist.readPlist(infofilePath);
			string dkey = "AppLovinSdkKey";
			if (!dict.ContainsKey (dkey)) {
				dict.Add (dkey, "e-4s7LbXsuJb2oXtoW10amMsJ9scHJhwHmmP6LxzEEZH159qbBqBxA2FKvsbCXWUIHuPdqMs2w840HucShoOtq");
				PlistCS.Plist.writeXml (dict, infofilePath);
				Debug.Log ("==> add " + dkey+ " :" + dict [dkey]);
			} else {
				Debug.Log ("==> exist " + dkey+ " :" + dict[dkey]);
			}
			dkey = "NSAppTransportSecurity";
			if (!dict.ContainsKey (dkey)) {
				Dictionary<string, bool> dv = new Dictionary<string, bool>(); 
				dv.Add ("NSAllowsArbitraryLoads", true);
				dict.Add (dkey, dv);
				Debug.Log ("==> add " + dkey+ " :" + dict [dkey]);
			} else {
				Debug.Log ("==> exist " + dkey+ " :" + dict[dkey]);
			}

			string pluginspath = System.IO.Path.Combine (System.IO.Path.Combine (Application.dataPath, "PolyADSDK"), "Plugins");
			Debug.Log ("==> pluginspath " + pluginspath);
			if (System.IO.File.Exists(pluginspath)) {
				string iospath = System.IO.Path.Combine (pluginspath, "IOS");
				Debug.Log ("==> iospath " + iospath);
				if (System.IO.File.Exists (iospath)) {
					string fmpath = System.IO.Path.Combine (iospath, "frameworks");
					if (System.IO.File.Exists (fmpath)) {
						string adcolonypath = System.IO.Path.Combine (fmpath, "AdColony.framework");
						if (System.IO.File.Exists (adcolonypath)) {
							System.IO.File.Delete(adcolonypath);
							Debug.Log ("==> del exist " + adcolonypath);
						}

						string delfmpath = System.IO.Path.Combine (fmpath, "FBAudienceNetwork.framework");
						if (System.IO.File.Exists (delfmpath)) {
							System.IO.File.Delete(delfmpath);
							Debug.Log ("==> del exist " + delfmpath);
						}

						delfmpath = System.IO.Path.Combine (fmpath, "GoogleMobileAds.framework");
						if (System.IO.File.Exists (delfmpath)) {
							System.IO.File.Delete(delfmpath);
							Debug.Log ("==> del exist " + delfmpath);
						}

						delfmpath = System.IO.Path.Combine (fmpath, "MVSDK.framework");
						if (System.IO.File.Exists (delfmpath)) {
							System.IO.File.Delete(delfmpath);
							Debug.Log ("==> del exist " + delfmpath);
						}

						delfmpath = System.IO.Path.Combine (fmpath, "MVSDKReward.framework");
						if (System.IO.File.Exists (delfmpath)) {
							System.IO.File.Delete(delfmpath);
							Debug.Log ("==> del exist " + delfmpath);
						}

						delfmpath = System.IO.Path.Combine (fmpath, "UnityAds.framework");
						if (System.IO.File.Exists (delfmpath)) {
							System.IO.File.Delete(delfmpath);
							Debug.Log ("==> del exist " + delfmpath);
						}

						delfmpath = System.IO.Path.Combine (fmpath, "VungleSDK.framework");
						if (System.IO.File.Exists (delfmpath)) {
							System.IO.File.Delete(delfmpath);
							Debug.Log ("==> del exist " + delfmpath);
						}
					}
				}
			}

			//在plist里面增加一行

			//list.AddKey(PlistAdd);
			//在plist里面替换一行
			//list.ReplaceKey("<string>com.yusong.${PRODUCT_NAME}</string>","<string>"+bundle+"</string>");
			//保存
			//list.Save();


			//string projPath = PBXProject.GenerateGuid ();
			//PBXProject pbxProject = new PBXProject ();



//			proj.AddEmbedFramework ("WebKit.framework");
//			proj.AddEmbedFramework ("libsqlite3.tbd");
//			proj.AddEmbedFramework ("libz.tbd");
//			proj.AddOtherLinkerFlags("-ObjC");
//			proj.Save();

//			string projPath = PBXProject.GetPBXProjectPath (path);
//			PBXProject proj = new PBXProject ();
//
//
//			proj.ReadFromString (File.ReadAllText (projPath));
//			string target = proj.TargetGuidByName ("Unity-iPhone");
//
//			// add extra framework(s)
//			proj.AddFrameworkToProject (target, "WebKit.framework", true);
//			proj.AddFrameworkToProject (target, "libsqlite3.tbd", true);
//			proj.AddFrameworkToProject (target, "libz.tbd", true);
//			proj.AddBuildProperty(target, "OTHER_LDFLAGS", "-ObjC");
//
//			File.WriteAllText(projPath, proj.WriteToString());

			// project.overwriteBuildSetting("ENABLE_BITCODE", "NO", "Release");
			// project.overwriteBuildSetting("ENABLE_BITCODE", "NO", "Debug");
			//project.AddOtherLinkerFlags("-ObjC");
			//project.Save();

		}
	}
}
#endif