using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEditor.U2D;
using UnityEngine;

public static class CustomBuilder
{
    private const string GoogleCredentialsId = "google_publisher_account_unimob_global";
    private const string FirebaseAndroidAppId = "1:117794470811:android:da43380c002d84441bff3e";
    private const string FirebaseIosAppId = "1:117794470811:ios:eefae854ca67bd551bff3e";
    private const string FirebaseCliToken = "1//0ep18apY1O0HXCgYIARAAGA4SNwF-L9IrDo1iznfZNIK-9ZKX-lE9P0ftsvik2XQzGoghBz5lxBLP0aV2IqwNcZuEFjqzwIulsBg";
    private const string IosAppId = "6447154983";
    private const string UserKeystore = "foodfever.keystore";
    private const string KeyaliasName = "foodfever";
    private const string KeyaliasPass = "ffsmobile";
    private const string KeystorePass = "ffsmobile";

    #region Build

    public static void Build()
    {
        BeforeBuild();
        var pipeline = Configs["PIPELINE"];
        // This was set during CI pipeline
        Console.WriteLine($"LOG::: Build Target {EditorUserBuildSettings.activeBuildTarget}");
        switch (EditorUserBuildSettings.activeBuildTarget)
        {
            case BuildTarget.iOS:
                BuildiOS();
                break;
            case BuildTarget.Android:
                BuildAndroid();
                break;
            default: throw new Exception("Not support this build target " + EditorUserBuildSettings.activeBuildTarget);
        }

        void BuildAndroid()
        {
            Console.WriteLine($"LOG::: Start building android {pipeline}");
            switch (pipeline)
            {
                case "production":
                    BuildAndroidProduction();
                    break;
                case "staging":
                    BuildAndroidStaging();
                    break;
                default:
                    BuildAndroidDevelopment();
                    break;
            }
        }

        void BuildiOS()
        {
            Console.WriteLine($"LOG::: Start building iOS {pipeline}");
            switch (pipeline)
            {
                case "production":
                    BuildXcodeProject();
                    break;
                default:
                    BuildXcodeProjectDevelopment();
                    break;
            }
        }
    }

    private static void AfterBuild()
    {
        var projectPath = new DirectoryInfo(Application.dataPath).Parent;
        if (projectPath != null)
        {
            var report = BuildReportTool.ReportGenerator.CreateReport(Path.Combine(projectPath.FullName, "buildlog.txt"));
            Debug.Log($"LOG::: {report}");
            if (!string.IsNullOrEmpty(report))
            {
                var reportExportFile = GetAndroidBuildPath(".xml");
                File.Copy(report, reportExportFile, true);
            }
        }
    }

    public static void BuildAndroidProduction()
    {
        AddScriptingDefineSymbol("PRODUCTION");
        RemoveScriptingDefineSymbol("DEVELOPMENT"); // Someone might accidentally add this
        EditorUserBuildSettings.development = false;
        EditorUserBuildSettings.allowDebugging = false;
        EditorUserBuildSettings.symlinkSources = false;
        EditorUserBuildSettings.androidCreateSymbolsZip = true;
        EditorUserBuildSettings.buildAppBundle = true;
        EditorUserBuildSettings.exportAsGoogleAndroidProject = false;
#if UNITY_2020_1_OR_NEWER
        PlayerSettings.Android.minifyDebug = false;
        PlayerSettings.Android.minifyRelease = false;
#endif
        SwitchScriptingImplement(ScriptingImplementation.IL2CPP);
        var buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.scenes = GetEnabledScenes();
        buildPlayerOptions.target = BuildTarget.Android;
        buildPlayerOptions.locationPathName = GetAndroidBuildPath(".aab");
        buildPlayerOptions.options = BuildOptions.CompressWithLz4HC;
        var report = BuildPipeline.BuildPlayer(buildPlayerOptions);
        var code = report.summary.result == BuildResult.Succeeded ? 0 : 1;
        if (code == 0) AfterBuild();
        EditorApplication.Exit(code);
    }

    public static void BuildAndroidStaging()
    {
        AddScriptingDefineSymbol("PRODUCTION");
        RemoveScriptingDefineSymbol("DEVELOPMENT"); // Someone might accidentally add this
		RemoveScriptingDefineSymbol("STAGING");
        EditorUserBuildSettings.development = false;
        EditorUserBuildSettings.allowDebugging = false;
        EditorUserBuildSettings.symlinkSources = false;
        EditorUserBuildSettings.androidCreateSymbolsZip = false;
        EditorUserBuildSettings.buildAppBundle = false;
        EditorUserBuildSettings.exportAsGoogleAndroidProject = false;
#if UNITY_2020_1_OR_NEWER
        PlayerSettings.Android.minifyDebug = false;
        PlayerSettings.Android.minifyRelease = false;
#endif
        SwitchScriptingImplement(ScriptingImplementation.IL2CPP);
        var report = BuildPipeline.BuildPlayer(
            GetEnabledScenes(), GetAndroidBuildPath(), BuildTarget.Android, BuildOptions.CompressWithLz4HC);
        var code = report.summary.result == BuildResult.Succeeded ? 0 : 1;
        if (code == 0) AfterBuild();
        EditorApplication.Exit(code);
    }

    public static void BuildAndroidDevelopment()
    {
        AddScriptingDefineSymbol("DEVELOPMENT");
        RemoveScriptingDefineSymbol("PRODUCTION");
        PlayerSettings.Android.useCustomKeystore = false;
        EditorUserBuildSettings.development = false;
        EditorUserBuildSettings.allowDebugging = false;
        EditorUserBuildSettings.symlinkSources = false;
        EditorUserBuildSettings.androidCreateSymbolsZip = false;
        EditorUserBuildSettings.buildAppBundle = false;
        EditorUserBuildSettings.exportAsGoogleAndroidProject = false;
        SwitchScriptingImplement(ScriptingImplementation.IL2CPP);
        var buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.scenes = GetEnabledScenes();
        buildPlayerOptions.target = BuildTarget.Android;
        buildPlayerOptions.locationPathName = GetAndroidBuildPath();
        buildPlayerOptions.options = BuildOptions.CompressWithLz4;
        var report = BuildPipeline.BuildPlayer(buildPlayerOptions);
        var code = report.summary.result == BuildResult.Succeeded ? 0 : 1;
        if (code == 0) AfterBuild();
        EditorApplication.Exit(code);
    }

    public static void BuildXcodeProject()
    {
        AddScriptingDefineSymbol("PRODUCTION");
        RemoveScriptingDefineSymbol("DEVELOPMENT"); // Someone might accidentally add this
        EditorUserBuildSettings.development = false;
        EditorUserBuildSettings.allowDebugging = false;
        EditorUserBuildSettings.symlinkSources = true;
        SwitchScriptingImplement(ScriptingImplementation.IL2CPP);
        var report =
            BuildPipeline.BuildPlayer(GetEnabledScenes(), GetXcodeFolder(), BuildTarget.iOS, BuildOptions.CompressWithLz4HC);
        var code = report.summary.result == BuildResult.Succeeded ? 0 : 1;
        EditorApplication.Exit(code);
    }

    public static void BuildXcodeProjectDevelopment()
    {
        //AddScriptingDefineSymbol("DEVELOPMENT");
        //RemoveScriptingDefineSymbol("PRODUCTION");
        EditorUserBuildSettings.development = false;
        EditorUserBuildSettings.allowDebugging = false;
        EditorUserBuildSettings.symlinkSources = false;
        SwitchScriptingImplement(ScriptingImplementation.IL2CPP);
        var report =
            BuildPipeline.BuildPlayer(GetEnabledScenes(), GetXcodeFolder(), BuildTarget.iOS, BuildOptions.CompressWithLz4HC);
        var code = report.summary.result == BuildResult.Succeeded ? 0 : 1;
        EditorApplication.Exit(code);
    }

    private static void SwitchScriptingImplement(ScriptingImplementation target)
    {
        if (PlayerSettings.GetScriptingBackend(BuildTargetGroup.Android) != target)
            PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, target);
    }

    public static void AddScriptingDefineSymbol(params string[] defines)
    {
        var definesString =
            PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
        var allDefines = definesString.Split(';').ToList();
        allDefines.AddRange(defines);
        PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup,
            string.Join(";", allDefines.ToArray()));
    }

    public static void RemoveScriptingDefineSymbol(params string[] defines)
    {
        var definesString =
            PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
        var allDefines = definesString.Split(';').ToList();
        foreach (var define in defines)
            if (allDefines.Contains(define))
            {
                Debug.Log($"LOG::: remove {define} from Define Symbols");
                allDefines.Remove(define);
            }

        PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup,
            string.Join(";", allDefines.ToArray()));
    }

    #endregion

    #region Environment

    public static void CleanUpDeletedScenes()
    {
        var currentScenes = EditorBuildSettings.scenes;
        EditorBuildSettings.scenes = currentScenes
            .Where(ebss => AssetDatabase.LoadAssetAtPath(ebss.path, typeof(SceneAsset)) != null).ToArray();
    }

    private static void BeforeBuild()
    {
        CleanUpDeletedScenes();

        AppendConfig("GOOGLE_CREDENTIALS_ID", GoogleCredentialsId);
        AppendConfig("FIREBASE_ANDROID_APP_ID", FirebaseAndroidAppId);
        AppendConfig("FIREBASE_IOS_APP_ID", FirebaseIosAppId);
        AppendConfig("FIREBASE_TOKEN", FirebaseCliToken);
        AppendConfig("IOS_APP_ID", IosAppId);

        KeyStore();

        SpriteAtlasUtility.PackAllAtlases(EditorUserBuildSettings.activeBuildTarget, false);

        AssetDatabase.SaveAssets();

        PrintAllEnviromentVariables();
        WriteBuildInfoFile();

        SetupBuildVersion();
        SetupBuildBundleVersion();

        AssetDatabase.SaveAssets();
        BuildAddressable();

		try
        {
            var projectPath = new DirectoryInfo(Application.dataPath).Parent;
            if (projectPath != null)
            {
                var buildPath = Path.Combine(projectPath.FullName, "build", "Android");
                if (!Directory.Exists(buildPath))
                {
                    Directory.CreateDirectory(buildPath);
                }
            }
        }
        catch (IOException ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private static void KeyStore()
    {
        // PlayerSettings.Android.useCustomKeystore = false;
        PlayerSettings.Android.useCustomKeystore = true;
        PlayerSettings.Android.keystoreName = UserKeystore;
        PlayerSettings.Android.keyaliasName = KeyaliasName;
        PlayerSettings.Android.keyaliasPass = KeyaliasPass;
        PlayerSettings.Android.keystorePass = KeystorePass;
    }

    private static void BuildAddressable()
    {
        // AddressableAssetSettings.CleanPlayerContent(); // Clean everything
        // Console.WriteLine("LOG::: Start Build addressable");
        //
        // // There is issue where Addressable in batch mode. Ask for confirm scene modified and stop build.
        // // You can check log and see that addressable build time = 0
        // AssetDatabase.SaveAssets();
        // AddressableAssetSettings.BuildPlayerContent();
        // Console.WriteLine("LOG::: Build addressable Done");
    }

    private static void SetupBuildBundleVersion()
    {
        try
        {
            var buildNumber = GetEnvironmentVariable("BUILD_NUMBER");
            var buildCount = int.Parse(buildNumber);
            if (Configs.ContainsKey("BUILD_BASE_BUNDLE_VERSION"))
            {
                var baseBuildCount = int.Parse(Configs["BUILD_BASE_BUNDLE_VERSION"]);
                PlayerSettings.Android.bundleVersionCode = baseBuildCount + buildCount;
                PlayerSettings.iOS.buildNumber = (baseBuildCount + buildCount).ToString();
            }
            else
            {
                PlayerSettings.Android.bundleVersionCode = buildCount;
                PlayerSettings.iOS.buildNumber = buildCount.ToString();
            }

#if UNITY_ANDROID
            AppendConfig("BUNDLE_VERSION_CODE", $"{PlayerSettings.Android.bundleVersionCode}");
#elif UNITY_IOS
            AppendConfig("BUNDLE_VERSION_CODE", $"{PlayerSettings.iOS.buildNumber}");
#endif
        }
        catch (Exception e)
        {
            Console.WriteLine("ERROR::: MISSING Environment for Build versioning");
            Console.WriteLine(e);
        }
    }

    private static void SetupBuildVersion()
    {
        // Change the game version and bundle version if config have it
        if (Configs.ContainsKey("RELEASE_VERSION"))
        {
            // version set up by semantic versioning
            Console.WriteLine("LOG::: Found release version in config");
            PlayerSettings.bundleVersion = Configs["RELEASE_VERSION"];
        }
        else
        {
            AppendConfig("RELEASE_VERSION", PlayerSettings.bundleVersion);
        }
    }

    private static string GetEnvironmentVariable(string name)
    {
        var envs = Environment.GetEnvironmentVariables();
        foreach (DictionaryEntry entry in envs)
            if (name == entry.Key.ToString())
                return entry.Value.ToString();
        return null;
    }

    private static void PrintAllEnviromentVariables()
    {
        Console.WriteLine("----------START ENVIRONMENT VARIABLES---------");
        var envs = Environment.GetEnvironmentVariables();
        foreach (DictionaryEntry entry in envs)
            Console.WriteLine(entry.Key + " " + entry.Value);

        var args = Environment.GetCommandLineArgs();
        var length = args.Length;
        for (var i = 0; i < length; i++)
            if (args[i].Contains("-") && i + 1 != length)
                Console.WriteLine(args[i] + " " + args[++i]);
        Console.WriteLine("----------END ENVIRONMENT VARIABLES---------");
    }

    // This is build information. Show this in game to identify build version
    private static void WriteBuildInfoFile()
    {
        AppendConfig("BUNDLE_VERSION", PlayerSettings.bundleVersion);
        var path = "Assets/Resources/BuildInfo.txt";
        var branch = Configs["GIT_BRANCH"];
        var commiter = Configs["GIT_COMMITER_NAME"];
        var hash = Configs["GIT_COMMIT_SHORT_HASH"];
        var buildNumber = GetEnvironmentVariable("BUILD_NUMBER");
        var time = Configs["GIT_COMMIT_DATE"];
        DateTime.TryParse(time, out var commitDate);
        var buildName =
            $"[{branch}][{commiter}] version:{PlayerSettings.bundleVersion} build:{buildNumber} time:{time} hash:{hash} unity:{Application.unityVersion}";
        var versionText = $"Version: {PlayerSettings.bundleVersion}.{buildNumber}.{commitDate:s}";
        Directory.CreateDirectory("Assets/Resources");
        var writer = new StreamWriter(path, false);
        writer.WriteLine(buildName);
        writer.WriteLine(versionText);
        writer.Close();
    }

    #endregion

    #region Naming build folder

    private static string[] GetEnabledScenes()
    {
        return (from scene in EditorBuildSettings.scenes where scene.enabled select scene.path).ToArray();
    }

    // File name is simple. Only when archive/moving file. It change name using shell
    public static string GetAndroidBuildPath(string extension = ".apk")
    {
        var projectDir = new DirectoryInfo(Application.dataPath).Parent;
        var time = DateTime.Now; // Use build time as name not git Commit date time. Due to sometime parse failed
        var branch = Configs["GIT_BRANCH"];
        branch = ReplaceInvalidChars(branch);
        var buildName = $"{PlayerSettings.productName.Replace(" ", "")}--{time:yyyy-MM-dd--HH-mm}--{branch}";

        AppendConfig("BUILD_FILE_NAME", buildName);
        AppendConfig("BUILD_FILE_SYMBOLS", $"{buildName}-{PlayerSettings.bundleVersion}-v{PlayerSettings.Android.bundleVersionCode}.symbols");
        AppendConfig("BUILD_FILE_SHRINK_SYMBOLS", $"{buildName}-{PlayerSettings.bundleVersion}-v{PlayerSettings.Android.bundleVersionCode}.symbols.shrink");
        AppendConfig("BUILD_FILE_MAPPING", $"{buildName}_mapping");

        buildName = $"{buildName}" + extension;
        var path = Path.Combine(projectDir.FullName, "build", "Android", buildName);
        Console.WriteLine("BUILD PATH: " + path);
        return path;
    }

    public static string ReplaceInvalidChars(string filename)
    {
        return string.Join("_", filename.Split(Path.GetInvalidFileNameChars()));
    }

    public static string GetXcodeFolder()
    {
        var projectDir = new DirectoryInfo(Application.dataPath).Parent;
        var path = Path.Combine(projectDir.FullName, "build", "iOS");
        Debug.Log(path);
        Console.WriteLine("BUILD PATH: " + path);
        return path;
    }

    #endregion

    #region Configs

    private static Dictionary<string, string> _configs;

    public static Dictionary<string, string> Configs
    {
        get
        {
            if (_configs != null)
                return _configs;
            InitConfig();
            return _configs;
        }
    }

    private static void InitConfig()
    {
        var projectDir = new DirectoryInfo(Application.dataPath).Parent;
        var configFile = Path.Combine(projectDir.FullName, "config.cfg");
        var lines = File.ReadAllLines(configFile);
        _configs = new Dictionary<string, string>();
        try
        {
            foreach (var line in lines)
            {
                var splits = line.Split('=');
                var key = splits[0];
                var value = splits.Length > 1 ? splits[1] : null;
                _configs.Add(key, value?.Replace("\"", ""));
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("ERROR::: Broken config files");
            Console.WriteLine(e);
        }
    }

    private static void AppendConfig(string key, string value)
    {
        var projectDir = new DirectoryInfo(Application.dataPath).Parent;
        var configFile = Path.Combine(projectDir.FullName, "config.cfg");
        try
        {
            var configs = new Dictionary<string, string>();
            var lines = File.ReadAllLines(configFile);
            foreach (var line in lines)
            {
                var splits = line.Split('=');
                var k = splits[0];
                var v = splits.Length > 1 ? splits[1] : null;
                configs.Add(k, v?.Replace("\"", ""));
            }

            if (!configs.ContainsKey(key))
            {
                File.AppendAllLines(configFile, new[] { $"{key}={value}" });
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("ERROR::: Broken config files");
            Console.WriteLine(e);
        }
    }

    #endregion
}