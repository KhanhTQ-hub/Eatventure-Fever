using UnityEditor;
using UnityEditor.Callbacks;
using System.IO;
using System.Text.RegularExpressions;

public class FixFBDeepLinks
{
	/// <summary>
	/// This regex will find the function that the FB function is mistakenly editing
	/// and fix the return value back to return NO.
	/// </summary>
	private const string IsBackgroundLaunchOptionsRegex =
		@"(?x)                                                  # Verbose mode
       (isBackgroundLaunchOptions:\(NSDictionary\*\).      # Find this function...
       (?:.*\n)+?                                          # Match as few lines as possible until...
       \s*return\ )YES(\;\n                                #   find the last YES;
   \})                                                     # }";

	[PostProcessBuild(101)] // Facebook runs at PostProcessBuild(100)
	public static void FixFacebookSDKError(BuildTarget buildTarget, string buildPath)
	{
		if (buildTarget != BuildTarget.iOS)
		{
			return;
		}

		var fullPath = Path.Combine(buildPath, Path.Combine("Classes", "UnityAppController.mm"));
		var data = Load(fullPath);
		data = Regex.Replace(data, IsBackgroundLaunchOptionsRegex, "$1NO$2");
		Save(fullPath, data);
	}

	private static string Load(string fullPath)
	{
		var projectFileInfo = new FileInfo(fullPath);
		var fs = projectFileInfo.OpenText();
		var data = fs.ReadToEnd();
		fs.Close();
		return data;
	}

	private static void Save(string fullPath, string data)
	{
		var writer = new StreamWriter(fullPath, false);
		writer.Write(data);
		writer.Close();
	}
}
