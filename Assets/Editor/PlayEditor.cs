using Cysharp.Threading.Tasks;
using UnityEditor;
using UnityEditor.SceneManagement;

#if UNITY_EDITOR
public class PlayEditor : Editor
{
    private static string ACTIVE_SCENE_KEY = "ActiveScene";

    [MenuItem("Play/PlayGame")]
    public static void PlayGame()
    {
        if (!EditorPrefs.HasKey(ACTIVE_SCENE_KEY))
        {
            EditorPrefs.SetString(ACTIVE_SCENE_KEY, string.Empty);
        }

        string activeSceneName = EditorSceneManager.GetActiveScene().name;
        EditorPrefs.SetString(ACTIVE_SCENE_KEY, activeSceneName);

        if (EditorApplication.isPlaying)
        {
            return;
        }

        if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
        {
            return;
        }

        string splashScene = "Splash";
        EditorSceneManager.OpenScene($"Assets/Scenes/{splashScene}.unity");
        EditorApplication.EnterPlaymode();
    }

    [MenuItem("Play/Stop")]
    public static async void StopGame()
    {
        if (!EditorApplication.isPlaying)
        {
            return;
        }

        EditorApplication.ExitPlaymode();

        await UniTask.WaitUntil(() => EditorApplication.isPlaying == false);

        string activeSceneName = EditorPrefs.GetString(ACTIVE_SCENE_KEY);
        EditorSceneManager.OpenScene($"Assets/Scenes/{activeSceneName}.unity");
    }
}

#endif