#if UNITY_IOS
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.iOS.Xcode;
using UnityEngine;

public class IosPostprocessor : IPostprocessBuildWithReport
{
    public int callbackOrder => 100;
    const string skadnetworksKey = "SKAdNetworkItems";
    const string skadnetworkKey = "SKAdNetworkIdentifier";
    const string CFBundleIcons = "CFBundleIcons";
    const string CFBundlePrimaryIcon = "CFBundlePrimaryIcon";
    const string CFBundleAlternateIcons = "CFBundleAlternateIcons";
    const string CFBundleIconFiles = "CFBundleIconFiles";
    const string UIPrerenderedIcon = "UIPrerenderedIcon";
    const string k_TrackingDescription = "Your data will be used to provide you a better and personalized ad experience.";
    string[] resourceExts = { ".png", ".jpg", ".jpeg" };

    public void OnPostprocessBuild(BuildReport report)
    {
        var pathToBuiltProject = report.summary.outputPath;
        var target = report.summary.platform;
        Debug.LogFormat("Postprocessing build at \"{0}\" for target {1}", pathToBuiltProject, target);
        if (target != BuildTarget.iOS)
            return;

        var projPath = PBXProject.GetPBXProjectPath(pathToBuiltProject);
        var pbxProject = new PBXProject();
        pbxProject.ReadFromString(File.ReadAllText(projPath));

        var plistFileName = Path.Combine(pathToBuiltProject, "Info.plist");
        var plist = new PlistDocument();
        plist.ReadFromString(File.ReadAllText(plistFileName));

#if UNITY_2019_3_OR_NEWER
        var targetGuid = pbxProject.GetUnityMainTargetGuid();
#else
        var targetName = PBXProject.GetUnityTargetName();
        var targetGuid = pbxProject.TargetGuidByName(targetName);
#endif
        var targetGuid2 = pbxProject.TargetGuidByName("UnityFramework");

        var token = pbxProject.GetBuildPropertyForAnyConfig(targetGuid, "USYM_UPLOAD_AUTH_TOKEN");
        pbxProject.AddBuildProperty(targetGuid, "OTHER_LDFLAGS", "-ObjC -all_load -licucore");
        pbxProject.SetBuildProperty(targetGuid, "USYM_UPLOAD_AUTH_TOKEN", string.IsNullOrEmpty(token) ? "FakeToken" : token);
        pbxProject.SetBuildProperty(targetGuid, "CLANG_ENABLE_MODULES", "YES");
        pbxProject.SetBuildProperty(targetGuid, "GCC_ENABLE_OBJC_EXCEPTIONS", "YES");
        pbxProject.SetBuildProperty(targetGuid, "ENABLE_BITCODE", "NO");
        pbxProject.SetBuildProperty(targetGuid, "ALWAYS_EMBED_SWIFT_STANDARD_LIBRARIES", "YES");
        pbxProject.SetBuildProperty(targetGuid2, "ENABLE_BITCODE", "NO");
        pbxProject.SetBuildProperty(targetGuid2, "USYM_UPLOAD_AUTH_TOKEN", string.IsNullOrEmpty(token) ? "FakeToken" : token);
        pbxProject.SetBuildProperty(targetGuid2, "ALWAYS_EMBED_SWIFT_STANDARD_LIBRARIES", "NO");

        var rootDict = plist.root;
        rootDict.SetBoolean("ITSAppUsesNonExemptEncryption", false);
        rootDict.SetString("FacebookClientToken", "b0a5cb8fb0c9154943d9a945f500da9a");
        rootDict.SetString("NSUserTrackingUsageDescription", k_TrackingDescription);
        rootDict.SetString("NSAdvertisingAttributionReportEndpoint", "https://appsflyer-skadnetwork.com/");

        iOSIconOptimization(pathToBuiltProject, pbxProject);
        WriteSkadNetworksIds(rootDict);

        File.WriteAllText(projPath, pbxProject.WriteToString());
        File.WriteAllText(plistFileName, plist.WriteToString());
    }

    private void WriteSkadNetworksIds(PlistElementDict rootDict)
    {
        string[] skadnetworksIds =
        {
            // iron source
            "su67r6k2v3.skadnetwork",
            "f7s53z58qe.skadnetwork",
            "2u9pt9hc89.skadnetwork",
            "hs6bdukanm.skadnetwork",
            "8s468mfl3y.skadnetwork",
            "c6k4g5qg8m.skadnetwork",
            "v72qych5uu.skadnetwork",
            "44jx6755aq.skadnetwork",
            "prcb7njmu6.skadnetwork",
            "m8dbw4sv7c.skadnetwork",
            "3rd42ekr43.skadnetwork",
            "4fzdc2evr5.skadnetwork",
            "t38b2kh725.skadnetwork",
            "f38h382jlk.skadnetwork",
            "424m5254lk.skadnetwork",
            "ppxm28t8ap.skadnetwork",
            "av6w8kgt66.skadnetwork",
            "cp8zw746q7.skadnetwork",
            "4468km3ulz.skadnetwork",
            "e5fvkxwrpn.skadnetwork",
            "22mmun2rn5.skadnetwork",
            "s39g8k73mm.skadnetwork",
            "yclnxrl5pm.skadnetwork",
            "3qy4746246.skadnetwork",
            // max applovin
            "22mmun2rn5.skadnetwork",
            "24t9a8vw3c.skadnetwork",
            "275upjj5gd.skadnetwork",
            "294l99pt4k.skadnetwork",
            "2fnua5tdw4.skadnetwork",
            "2u9pt9hc89.skadnetwork",
            "32z4fx6l9h.skadnetwork",
            "3l6bd9hu43.skadnetwork",
            "3qcr597p9d.skadnetwork",
            "3rd42ekr43.skadnetwork",
            "3sh42y64q3.skadnetwork",
            "424m5254lk.skadnetwork",
            "4468km3ulz.skadnetwork",
            "4fzdc2evr5.skadnetwork",
            "4pfyvq9l8r.skadnetwork",
            "523jb4fst2.skadnetwork",
            "52fl2v3hgk.skadnetwork",
            "54nzkqm89y.skadnetwork",
            "578prtvx9j.skadnetwork",
            "5a6flpkh64.skadnetwork",
            "5l3tpt7t6e.skadnetwork",
            "5lm9lj6jb7.skadnetwork",
            "5tjdwbrq8w.skadnetwork",
            "6g9af3uyq4.skadnetwork",
            "6xzpu9s2p8.skadnetwork",
            "79pbpufp6p.skadnetwork",
            "7rz58n8ntl.skadnetwork",
            "7ug5zh24hu.skadnetwork",
            "8s468mfl3y.skadnetwork",
            "9b89h5y424.skadnetwork",
            "9nlqeag3gk.skadnetwork",
            "9rd848q2bz.skadnetwork",
            "9t245vhmpl.skadnetwork",
            "9yg77x724h.skadnetwork",
            "a8cz6cu7e5.skadnetwork",
            "av6w8kgt66.skadnetwork",
            "c3frkrj4fj.skadnetwork",
            "c6k4g5qg8m.skadnetwork",
            "cg4yq2srnc.skadnetwork",
            "cj5566h2ga.skadnetwork",
            "cstr6suwn9.skadnetwork",
            "dkc879ngq3.skadnetwork",
            "e5fvkxwrpn.skadnetwork",
            "ejvt5qm6ak.skadnetwork",
            "f38h382jlk.skadnetwork",
            "feyaarzu9v.skadnetwork",
            "g28c52eehv.skadnetwork",
            "ggvn48r87g.skadnetwork",
            "glqzh8vgby.skadnetwork",
            "gta9lk7p23.skadnetwork",
            "hs6bdukanm.skadnetwork",
            "k674qkevps.skadnetwork",
            "kbd757ywx3.skadnetwork",
            "kbmxgpxpgc.skadnetwork",
            "klf5c3l5u5.skadnetwork",
            "ludvb6z3bs.skadnetwork",
            "m5mvw97r93.skadnetwork",
            "m8dbw4sv7c.skadnetwork",
            "mlmmfzh3r3.skadnetwork",
            "mtkv5xtk9e.skadnetwork",
            "n66cz3y3bx.skadnetwork",
            "n6fk4nfna4.skadnetwork",
            "n9x2a789qt.skadnetwork",
            "nzq8sh4pbs.skadnetwork",
            "p78axxw29g.skadnetwork",
            "ppxm28t8ap.skadnetwork",
            "prcb7njmu6.skadnetwork",
            "pwa73g5rt2.skadnetwork",
            "qqp299437r.skadnetwork",
            "r45fhb6rf7.skadnetwork",
            "rvh3l7un93.skadnetwork",
            "t38b2kh725.skadnetwork",
            "tl55sbb4fm.skadnetwork",
            "uw77j35x4d.skadnetwork",
            "v72qych5uu.skadnetwork",
            "vcra2ehyfk.skadnetwork",
            "wg4vff78zm.skadnetwork",
            "wzmmz9fp6w.skadnetwork",
            "x44k69ngh6.skadnetwork",
            "x5l83yy675.skadnetwork",
            "x8jxxk4ff5.skadnetwork",
            "x8uqf25wch.skadnetwork",
            "xy9t38ct57.skadnetwork",
            "yclnxrl5pm.skadnetwork",
            "ydx93a7ass.skadnetwork",
            "zmvfpc5aq8.skadnetwork",
            // facebook
            "v9wttpbfk9.skadnetwork",
            "n38lu8286q.skadnetwork",
            // unity
            "w9q455wk68.skadnetwork",
            "4w7y6s5ca2.skadnetwork",
            "v79kvwwj4g.skadnetwork",
            "238da6jt44.skadnetwork",
            "488r3q3dtq.skadnetwork",
            "f73kdq92p3.skadnetwork",
            "mp6xlyr22a.skadnetwork",
            "lr83yxwka7.skadnetwork",
            "a2p9lx4jpn.skadnetwork",
            "4dzt52r2t5.skadnetwork",
            "zq492l623r.skadnetwork",
        };

        var skadnetworks = rootDict[skadnetworksKey] != null ? rootDict[skadnetworksKey].AsArray() : null;
        if (skadnetworks == null)
        {
            skadnetworks = rootDict.CreateArray(skadnetworksKey);
        }

        var skadnetworksCopy = new List<string>();
        foreach (var skadnetwork in skadnetworks.values)
        {
            skadnetworksCopy.Add(skadnetwork[skadnetworkKey].AsString());
        }

        foreach (var skadnetwork in skadnetworksIds)
        {
            if (skadnetworksCopy.Find(x => x == skadnetwork) == null)
            {
                var dict = skadnetworks.AddDict();
                dict.SetString(skadnetworkKey, skadnetwork);
            }
        }
    }

    private void iOSIconOptimization(string pathProject, PBXProject pbxProject)
    {
        var imagesXcassetsDirectoryPath = Path.Combine(pathProject, "Unity-iPhone", "Images.xcassets");
        var iconNames = new List<string>();
        var resources = new List<string>();
        GetDirFileList("Assets/Plugins/iOS/resources", ref resources, resourceExts);
        foreach (var resource in resources)
        {
            var iconName = Path.GetFileNameWithoutExtension(resource);
            iconNames.Add(iconName);

            var iconDirectoryPath = Path.Combine(imagesXcassetsDirectoryPath, $"{iconName}.appiconset");
            Directory.CreateDirectory(iconDirectoryPath);

            var contentsJsonPath = Path.Combine(iconDirectoryPath, "Contents.json");
            var contentsJson = ContentsJsonText;
            contentsJson = contentsJson.Replace("iPhoneContents",
                PlayerSettings.iOS.targetDevice == iOSTargetDevice.iPhoneOnly || PlayerSettings.iOS.targetDevice == iOSTargetDevice.iPhoneAndiPad
                    ? ContentsiPhoneJsonText
                    : string.Empty);
            contentsJson = contentsJson.Replace("iPadContents",
                PlayerSettings.iOS.targetDevice == iOSTargetDevice.iPadOnly || PlayerSettings.iOS.targetDevice == iOSTargetDevice.iPhoneAndiPad
                    ? ContentsiPadJsonText
                    : string.Empty);
            File.WriteAllText(contentsJsonPath, contentsJson, Encoding.UTF8);

            if (PlayerSettings.iOS.targetDevice == iOSTargetDevice.iPhoneOnly || PlayerSettings.iOS.targetDevice == iOSTargetDevice.iPhoneAndiPad)
            {
                SaveIcon(resource, 40, Path.Combine(iconDirectoryPath, "iPhoneNotification40px.png"));
                SaveIcon(resource, 60, Path.Combine(iconDirectoryPath, "iPhoneNotification60px.png"));
                SaveIcon(resource, 58, Path.Combine(iconDirectoryPath, "iPhoneSettings58px.png"));
                SaveIcon(resource, 87, Path.Combine(iconDirectoryPath, "iPhoneSettings87px.png"));
                SaveIcon(resource, 80, Path.Combine(iconDirectoryPath, "iPhoneSpotlight80px.png"));
                SaveIcon(resource, 120, Path.Combine(iconDirectoryPath, "iPhoneSpotlight120px.png"));
                SaveIcon(resource, 120, Path.Combine(iconDirectoryPath, "iPhoneApp120px.png"));
                SaveIcon(resource, 180, Path.Combine(iconDirectoryPath, "iPhoneApp180px.png"));
            }

            if (PlayerSettings.iOS.targetDevice == iOSTargetDevice.iPadOnly || PlayerSettings.iOS.targetDevice == iOSTargetDevice.iPhoneAndiPad)
            {
                SaveIcon(resource, 20, Path.Combine(iconDirectoryPath, "iPadNotifications20px.png"));
                SaveIcon(resource, 40, Path.Combine(iconDirectoryPath, "iPadNotifications40px.png"));
                SaveIcon(resource, 29, Path.Combine(iconDirectoryPath, "iPadSettings29px.png"));
                SaveIcon(resource, 58, Path.Combine(iconDirectoryPath, "iPadSettings58px.png"));
                SaveIcon(resource, 40, Path.Combine(iconDirectoryPath, "iPadSpotlight40px.png"));
                SaveIcon(resource, 80, Path.Combine(iconDirectoryPath, "iPadSpotlight80px.png"));
                SaveIcon(resource, 76, Path.Combine(iconDirectoryPath, "iPadApp76px.png"));
                SaveIcon(resource, 152, Path.Combine(iconDirectoryPath, "iPadApp152px.png"));
                SaveIcon(resource, 167, Path.Combine(iconDirectoryPath, "iPadProApp167px.png"));
            }

            SaveIcon(resource, 1024, Path.Combine(iconDirectoryPath, "appStore1024px.png"));
        }

        var targetGuid = pbxProject.GetUnityMainTargetGuid();
        pbxProject.SetBuildProperty(targetGuid, "ASSETCATALOG_COMPILER_INCLUDE_ALL_APPICON_ASSETS", "YES");

        var joinedIconNames = string.Join(" ", iconNames);
        pbxProject.SetBuildProperty(targetGuid, "ASSETCATALOG_COMPILER_ALTERNATE_APPICON_NAMES", joinedIconNames);
    }

    private void CopyIconOptimization(PBXProject pbxProject, string targetGuid, string path)
    {
        var resources = new List<string>();
        CopyAndReplaceDirectory("Assets/Plugins/iOS/resources", Path.Combine(path, "resources"), resourceExts);
        GetDirFileList("Assets/Plugins/iOS/resources", ref resources, resourceExts, "resources");
        foreach (string resource in resources)
        {
            var resourcesBuildPhase = pbxProject.GetResourcesBuildPhaseByTarget(targetGuid);
            var resourcesFilesGuid = pbxProject.AddFile(resource, resource, UnityEditor.iOS.Xcode.PBXSourceTree.Source);
            pbxProject.AddFileToBuildSection(targetGuid, resourcesBuildPhase, resourcesFilesGuid);
        }
    }

    private void WriteIconOptimization(PlistElementDict root)
    {
        var dict = root[CFBundleIcons] != null ? root[CFBundleIcons].AsDict() : null;
        if (dict == null)
        {
            dict = root.CreateDict(CFBundleIcons);
        }

        foreach (var file in Directory.GetFiles("Assets/Plugins/iOS/resources"))
        {
            if (resourceExts.Contains(Path.GetExtension(file)))
            {
                if (file.Contains("@")) continue;
                var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file);
                if (fileNameWithoutExtension.Equals("PrimaryIcon"))
                {
                    WritePrimaryIconItem(dict, fileNameWithoutExtension);
                }
                else
                {
                    WriteAlternateIconItem(dict, fileNameWithoutExtension);
                }
            }
        }
    }

    private void WriteAlternateIconItem(PlistElementDict root, string iconName)
    {
        var dict = root[CFBundleAlternateIcons] != null ? root[CFBundleAlternateIcons].AsDict() : null;
        if (dict == null)
        {
            dict = root.CreateDict(CFBundleAlternateIcons);
        }

        var alternate = dict[iconName] != null ? dict[iconName].AsDict() : null;
        if (alternate == null)
        {
            alternate = dict.CreateDict(iconName);
        }

        var icon = alternate[CFBundleIconFiles] != null ? alternate[CFBundleIconFiles].AsArray() : null;
        if (icon == null)
        {
            icon = alternate.CreateArray(CFBundleIconFiles);
        }

        var icons = icon.values.Select(i => i.AsString()).ToList();
        if (icons.Find(x => x == iconName) == null)
        {
            icon.AddString(iconName);
        }

        alternate.SetBoolean(UIPrerenderedIcon, false);
    }

    private void WritePrimaryIconItem(PlistElementDict root, string iconName)
    {
        var dict = root[CFBundlePrimaryIcon] != null ? root[CFBundlePrimaryIcon].AsDict() : null;
        if (dict == null)
        {
            dict = root.CreateDict(CFBundlePrimaryIcon);
        }

        var icon = dict[CFBundleIconFiles] != null ? dict[CFBundleIconFiles].AsArray() : null;
        if (icon == null)
        {
            icon = dict.CreateArray(CFBundleIconFiles);
        }

        var icons = icon.values.Select(i => i.AsString()).ToList();
        if (icons.Find(x => x == iconName) == null)
        {
            icon.AddString(iconName);
        }

        dict.SetBoolean(UIPrerenderedIcon, false);
    }

    private void CopyAndReplaceDirectory(string srcPath, string dstPath, string[] enableExts)
    {
        if (Directory.Exists(dstPath))
        {
            Directory.Delete(dstPath);
        }

        if (File.Exists(dstPath))
        {
            File.Delete(dstPath);
        }

        Directory.CreateDirectory(dstPath);

        foreach (var file in Directory.GetFiles(srcPath))
        {
            if (enableExts.Contains(Path.GetExtension(file)))
            {
                File.Copy(file, Path.Combine(dstPath, Path.GetFileName(file)));
            }
        }

        foreach (var dir in Directory.GetDirectories(srcPath))
        {
            CopyAndReplaceDirectory(dir, Path.Combine(dstPath, Path.GetFileName(dir)), enableExts);
        }
    }

    private void GetDirFileList(string dirPath, ref List<string> dirs, string[] enableExts, string subPathFrom = "")
    {
        foreach (string path in Directory.GetFiles(dirPath))
        {
            if (enableExts.Contains(Path.GetExtension(path)))
            {
                if (subPathFrom != "")
                {
                    dirs.Add(path.Substring(path.IndexOf(subPathFrom, StringComparison.Ordinal)));
                }
                else
                {
                    dirs.Add(path);
                }
            }
        }

        if (Directory.GetDirectories(dirPath).Length > 0)
        {
            foreach (string path in Directory.GetDirectories(dirPath))
            {
                GetDirFileList(path, ref dirs, enableExts, subPathFrom);
            }
        }
    }

    private static void SaveIcon(string sourcePath, int size, string savePath)
    {
        var iconTexture = new Texture2D(0, 0);
        iconTexture.LoadImage(File.ReadAllBytes(sourcePath));

        if (iconTexture.width != size || iconTexture.height != size)
        {
            var renderTexture = new RenderTexture(size, size, 24);
            var tmpRenderTexture = RenderTexture.active;
            RenderTexture.active = renderTexture;
            Graphics.Blit(iconTexture, renderTexture);
            var resizedTexture = new Texture2D(size, size);
            resizedTexture.ReadPixels(new Rect(0, 0, size, size), 0, 0);
            resizedTexture.Apply();
            RenderTexture.active = tmpRenderTexture;
            renderTexture.Release();
            iconTexture = resizedTexture;
        }

        var pngBytes = iconTexture.EncodeToPNG();
        File.WriteAllBytes(savePath, pngBytes);
    }

    private const string ContentsJsonText = @"{
  ""images"" : [
	iPhoneContents
	iPadContents
	{
	  ""filename"" : ""appStore1024px.png"",
	  ""idiom"" : ""ios-marketing"",
	  ""scale"" : ""1x"",
	  ""size"" : ""1024x1024""
	}
  ],
  ""info"" : {
    ""author"" : ""xcode"",
    ""version"" : 1
  }
}
";

    private const string ContentsiPhoneJsonText = @"{
	  ""filename"" : ""iPhoneNotification40px.png"",
	  ""idiom"" : ""iphone"",
	  ""scale"" : ""2x"",
	  ""size"" : ""20x20""
	},
	{
	  ""filename"" : ""iPhoneNotification60px.png"",
	  ""idiom"" : ""iphone"",
	  ""scale"" : ""3x"",
	  ""size"" : ""20x20""
	},
	{
	  ""filename"" : ""iPhoneSettings58px.png"",
	  ""idiom"" : ""iphone"",
	  ""scale"" : ""2x"",
	  ""size"" : ""29x29""
	},
	{
	  ""filename"" : ""iPhoneSettings87px.png"",
	  ""idiom"" : ""iphone"",
	  ""scale"" : ""3x"",
	  ""size"" : ""29x29""
	},
	{
	  ""filename"" : ""iPhoneSpotlight80px.png"",
	  ""idiom"" : ""iphone"",
	  ""scale"" : ""2x"",
	  ""size"" : ""40x40""
	},
	{
	  ""filename"" : ""iPhoneSpotlight120px.png"",
	  ""idiom"" : ""iphone"",
	  ""scale"" : ""3x"",
	  ""size"" : ""40x40""
	},
	{
	  ""filename"" : ""iPhoneApp120px.png"",
	  ""idiom"" : ""iphone"",
	  ""scale"" : ""2x"",
	  ""size"" : ""60x60""
	},
	{
	  ""filename"" : ""iPhoneApp180px.png"",
	  ""idiom"" : ""iphone"",
	  ""scale"" : ""3x"",
	  ""size"" : ""60x60""
	},
";

    private const string ContentsiPadJsonText = @"{
	  ""filename"" : ""iPadNotifications20px.png"",
	  ""idiom"" : ""ipad"",
	  ""scale"" : ""1x"",
	  ""size"" : ""20x20""
	},
	{
	  ""filename"" : ""iPadNotifications40px.png"",
	  ""idiom"" : ""ipad"",
	  ""scale"" : ""2x"",
	  ""size"" : ""20x20""
	},
	{
	  ""filename"" : ""iPadSettings29px.png"",
	  ""idiom"" : ""ipad"",
	  ""scale"" : ""1x"",
	  ""size"" : ""29x29""
	},
	{
	  ""filename"" : ""iPadSettings58px.png"",
	  ""idiom"" : ""ipad"",
	  ""scale"" : ""2x"",
	  ""size"" : ""29x29""
	},
	{
	  ""filename"" : ""iPadSpotlight40px.png"",
	  ""idiom"" : ""ipad"",
	  ""scale"" : ""1x"",
	  ""size"" : ""40x40""
	},
	{
	  ""filename"" : ""iPadSpotlight80px.png"",
	  ""idiom"" : ""ipad"",
	  ""scale"" : ""2x"",
	  ""size"" : ""40x40""
	},
	{
	  ""filename"" : ""iPadApp76px.png"",
	  ""idiom"" : ""ipad"",
	  ""scale"" : ""1x"",
	  ""size"" : ""76x76""
	},
	{
	  ""filename"" : ""iPadApp152px.png"",
	  ""idiom"" : ""ipad"",
	  ""scale"" : ""2x"",
	  ""size"" : ""76x76""
	},
	{
	  ""filename"" : ""iPadProApp167px.png"",
	  ""idiom"" : ""ipad"",
	  ""scale"" : ""2x"",
	  ""size"" : ""83.5x83.5""
	},
";
}
#endif