#if UNITY_IOS
using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;

public class AppleDeeplinkPostprocessor
{
	[PostProcessBuild]
	public static void OnPostProcessBuild(BuildTarget buildTarget, string path)
	{
		if (buildTarget != BuildTarget.iOS)
		{
			return;
		}

		var projectPath = PBXProject.GetPBXProjectPath(path);
		var project = new PBXProject();
		project.ReadFromString(File.ReadAllText(projectPath));
		var manager = new ProjectCapabilityManager(projectPath, "Entitlements.entitlements", null, project.GetUnityMainTargetGuid());
		manager.AddAssociatedDomains(new string[] { "applinks:g73j.adj.st" });
		manager.WriteToFile();
	}
}
#endif
