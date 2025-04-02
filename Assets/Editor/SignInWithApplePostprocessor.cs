#if UNITY_IOS
using AppleAuth.Editor;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;

public static class SignInWithApplePostprocessor
{
    [PostProcessBuild(1)]
    public static void OnPostProcessBuild(BuildTarget target, string path)
    {
        if (target != BuildTarget.iOS)
            return;

        var projectPath = PBXProject.GetPBXProjectPath(path);

#if UNITY_2019_3_OR_NEWER
        var project = new PBXProject();
        project.ReadFromString(System.IO.File.ReadAllText(projectPath));
        var manager = new ProjectCapabilityManager(projectPath, "Entitlements.entitlements", null,
            project.GetUnityMainTargetGuid());
        manager.AddSignInWithAppleWithCompatibility(project.GetUnityFrameworkTargetGuid());
#if DEVELOPMENT
        manager.AddPushNotifications(true);
#else
        manager.AddPushNotifications(false);
#endif
        manager.WriteToFile();
#else
        var manager = new ProjectCapabilityManager(projectPath, "Entitlements.entitlements", PBXProject.GetUnityTargetName());
        manager.AddSignInWithAppleWithCompatibility();
#if DEVELOPMENT
        manager.AddPushNotifications(true);
#else
        manager.AddPushNotifications(false);
#endif
        manager.WriteToFile();
#endif
    }
}
#endif