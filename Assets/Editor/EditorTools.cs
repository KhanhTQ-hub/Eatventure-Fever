using System;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using I2.Loc;
using TMPro;
using UnityEditor.SceneManagement;
using UnityEngine.U2D;
using com.unimob.save.core;

#if UNITY_EDITOR
public class EditorTools : EditorWindow
{
    public class AssetDatabaseUtils
    {
        public static string GetSelectionObjectPath()
        {
            var path = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (path == "")
            {
                path = "Assets";
            }
            else if (Path.GetExtension(path) != "")
            {
                path = path.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
            }

            return path;
        }

        public static T GetAssetOfType<T>(string name, System.Type mainType = null) where T : class
        {
            if (mainType == null)
            {
                mainType = typeof(T);
            }

            var guids = AssetDatabase.FindAssets(name + " t:" + mainType.Name);
            if (guids.Length == 0)
                return null;
            string guid = guids[0];
            string path = AssetDatabase.GUIDToAssetPath(guid);
            foreach (var o in AssetDatabase.LoadAllAssetsAtPath(path))
            {
                var res = o as T;
                if (res != null)
                {
                    return res;
                }
            }

            return default(T);
        }

        public static string GetAssetPathOfType<T>(string name, System.Type mainType = null) where T : class
        {
            if (mainType == null)
            {
                mainType = typeof(T);
            }

            var guids = AssetDatabase.FindAssets(name + " t:" + mainType.Name);
            if (guids.Length == 0)
                return null;
            string guid = guids[0];
            string path = AssetDatabase.GUIDToAssetPath(guid);
            return path;
        }

        public static T GetAssetOfType<T>(bool unique = false) where T : class
        {
            var guids = AssetDatabase.FindAssets("t:" + typeof(T).FullName);
            if (guids.Length == 0)
                return null;
            if (guids.Length > 1 && unique)
            {
                var pathes = "";
                foreach (var g in guids)
                {
                    var assetPath = AssetDatabase.GUIDToAssetPath(g);
                    pathes += assetPath + "\n";
                }

                throw new System.ArgumentException("Has multiple objects with this type: \n" + pathes);
            }

            var guid = guids[0];
            var path = AssetDatabase.GUIDToAssetPath(guid);
            return AssetDatabase.LoadAssetAtPath(path, typeof(T)) as T;
        }

        public static T[] GetAssetsOfType<T>() where T : class
        {
            if (typeof(UnityEngine.Component).IsAssignableFrom(typeof(T)))
            {
                var guidsGO = AssetDatabase.FindAssets("t:Prefab");
                var l = new List<T>();
                foreach (var g in guidsGO)
                {
                    var path = AssetDatabase.GUIDToAssetPath(g);
                    var t = (AssetDatabase.LoadAssetAtPath(path, typeof(GameObject)) as GameObject).GetComponent<T>();
                    if (t != null)
                    {
                        l.Add(t);
                    }
                }

                return l.ToArray();
            }

            var guids = AssetDatabase.FindAssets("t:" + typeof(T).FullName);
            if (guids.Length == 0)
                return null;

            var i = 0;
            var res = new T[guids.Length];
            foreach (var g in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(g);
                var t = AssetDatabase.LoadAssetAtPath(path, typeof(T)) as T;
                res[i] = t;
                i++;
            }

            return res;
        }
    }

    private void DistinctAllCharacterInSheet()
    {
        var path = "Assets/Localization/Characters/CharacterSheet_{0}.txt";
        var pathCharId = "Assets/Localization/Characters/CharacterId_{0}.txt";
        var languageSourceAsset = AssetDatabaseUtils.GetAssetOfType<LanguageSourceAsset>("I2Languages");
        var sourceData = languageSourceAsset.SourceData;
        var languages = sourceData.mLanguages;
        var terms = sourceData.mTerms;

        var languageBuilderArray = new StringBuilder[languages.Count];
        for (var i = 0; i < languageBuilderArray.Length; i++)
        {
            languageBuilderArray[i] = new StringBuilder();
        }

        var charListArray = new HashSet<char>[languages.Count];
        for (var i = 0; i < charListArray.Length; i++)
        {
            charListArray[i] = new HashSet<char>();
        }

        foreach (var termData in terms)
        {
            for (var i = 0; i < languages.Count; ++i)
            {
                AppendToCharSet(charListArray[i], termData.Languages[i], LocalizationManager.IsRTL(languages[i].Code));
            }
        }

        for (var i = 0; i < charListArray.Length; i++)
        {
            var bytes = Encoding.UTF8.GetBytes(charListArray[i].ToArray().OrderBy(c => c).ToArray());
            var charSet = Encoding.UTF8.GetString(bytes);
            if (charSet.Length > 0)
            {
                var charBuilder = new StringBuilder();
                var missingCharIdBuilder = new StringBuilder();

                foreach (var character in charSet)
                {
                    if (!HasCharacter(TMP_Settings.defaultFontAsset, character))
                    {
                        missingCharIdBuilder.Append(character);
                    }

                    charBuilder.Append(character);
                }

                var fileName = string.Format(path, languages[i].Name);
                using (var writer = File.CreateText(fileName))
                {
                    writer.Write(charBuilder.ToString());
                }

                var charIdBuilder = new StringBuilder();
                foreach (var character in charSet)
                {
                    charIdBuilder.Append((int)character);
                    charIdBuilder.Append(',');
                }

                if (charIdBuilder.Length > 0)
                {
                    charIdBuilder.Remove(charIdBuilder.Length - 1, 1);
                }

                var idFileName = string.Format(pathCharId, languages[i].Name);
                using (var writer = File.CreateText(idFileName))
                {
                    writer.Write(charIdBuilder);
                }

                Debug.Log($"{languages[i].Name}: {missingCharIdBuilder.Length}");
                AddMissingCharacters(languages[i].Name, missingCharIdBuilder.ToString());
            }
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("List Unique Char");
    }

    private static void AddMissingCharacters(string language, string characters)
    {
        if (characters.Length <= 0) return;
        if (language.Equals(TMP_Settings.defaultFontAsset.name)) return;
        var fallbackFontAsset = Resources.Load<TMP_FontAsset>($"{TMP_Settings.defaultFontAssetPath}{language}");
        if (fallbackFontAsset != null)
        {
            fallbackFontAsset.atlasPopulationMode = AtlasPopulationMode.Dynamic;
            fallbackFontAsset.TryAddCharacters(characters);
            fallbackFontAsset.atlasPopulationMode = AtlasPopulationMode.Static;
        }
    }

    private static bool HasCharacter(TMP_FontAsset font, char character)
    {
        if (font.atlasPopulationMode == AtlasPopulationMode.Dynamic)
        {
            return false;
        }

        if (font.characterLookupTable.ContainsKey(character))
        {
            return true;
        }

        return false;
    }

    static void AppendToCharSet(HashSet<char> sb, string text, bool isRTL)
    {
        if (string.IsNullOrEmpty(text)) return;

        text = RemoveTagsPrefix(text, "[i2p_");
        text = RemoveTagsPrefix(text, "[i2s_");
        text = RemoveTagsPrefix(text, "{");

        if (isRTL) text = RTLFixer.Fix(text);

        foreach (char c in text)
        {
            sb.Add(char.ToLowerInvariant(c));
            sb.Add(char.ToUpperInvariant(c));
        }
    }

    static string RemoveTagsPrefix(string text, string tagPrefix)
    {
        var idx = 0;
        while (idx < text.Length)
        {
            idx = text.IndexOf(tagPrefix);
            if (idx < 0) break;

            int idx2 = text.IndexOf(']', idx);
            if (idx2 < 0) break;

            text = text.Remove(idx, idx2 - idx + 1);
        }

        idx = 0;
        while (idx < text.Length)
        {
            idx = text.IndexOf(tagPrefix);
            if (idx < 0) break;

            int idx2 = text.IndexOf('}', idx);
            if (idx2 < 0) break;

            text = text.Remove(idx, idx2 - idx + 1);
        }

        return text;
    }

    [MenuItem("Tools/Show Tool")]
    public static void ShowWindow()
    {
        GetWindow(typeof(EditorTools)).titleContent = new GUIContent("Editor Tool");
    }

    private Vector2 scroll;

    private void OnGUI()
    {
        GUILayout.Label("Tools", EditorStyles.boldLabel);

        if (GUILayout.Button("List Unique Char")) DistinctAllCharacterInSheet();
        if (GUILayout.Button("Clear Save Data")) ClearSaveData();
        if (GUILayout.Button("Clamp Map")) ClampMap();
        if (GUILayout.Button("Compress Image")) CompressImage();

        GUILayout.Label("Scenes", EditorStyles.boldLabel);
        var scenes = EditorBuildSettings.scenes;
        if (scenes.Length <= 0)
            return;

        int col = 3;
        int part = (int)(scenes.Length * 1.0f / col + 0.5f);

        int start = 0;
        int end = 0;

        scroll = GUILayout.BeginScrollView(scroll);
        {
            GUILayout.BeginHorizontal("Box");

            for (int i = 0; i < col; i++)
            {
                end = i == col - 1 ? scenes.Length : start + part;

                GUILayout.BeginVertical("Box");

                for (int j = start; j < end; j++)
                {
                    var scene = scenes[j];
                    string[] names = scene.path.Split('/');

                    if (!GUILayout.Button(names[names.Length - 1]))
                        continue;
                    EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
                    EditorSceneManager.OpenScene(scene.path);
                }

                GUILayout.EndVertical();

                start = end;
            }

            GUILayout.EndHorizontal();
        }
        GUILayout.EndScrollView();
    }

    private void ClearSaveData()
    {
        IOSaver.DeleteAll(Application.persistentDataPath);
        PlayerPrefs.DeleteAll();
        Debug.Log("Clear Save Data");
    }

    private void ClampMap()
    {
        var lst = Resources.FindObjectsOfTypeAll<ConfigCamera>();
        ConfigCamera camera = null;

        if (lst.Length > 1)
            camera = lst[0];

        float value = 0;
        if (camera != null)
            value = (camera.clamp.y + camera.clamp.x) / 2;

        var mains = Resources.FindObjectsOfTypeAll<MainGameManager>();
        MainGameManager main = null;

        if (mains.Length > 1)
            main = mains[0];

        if (value != 0 && main && camera)
        {
            {
                var pos = main.transform.position;
                pos.y -= value;
                main.transform.position = pos;
            }
            {
                var pos = camera.transform.position;
                pos.y -= value;
                camera.transform.position = pos;
            }
            {
                camera.clamp -= new Vector2(value, value);
            }

            EditorUtility.SetDirty(camera);
            EditorUtility.SetDirty(main);

            EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
        }

        Debug.Log("Clamp Map");
    }

    private void CompressImage()
    {
        string[] paths = AssetDatabase.FindAssets("t:SpriteAtlas");
        var numChanges = 0;

        var destFmtIphone = TextureImporterFormat.ASTC_6x6;
        var destFmtAndroid = TextureImporterFormat.ETC2_RGBA8;

        foreach (var path in paths)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(path);
            SpriteAtlas spriteAtlas = AssetDatabase.LoadAssetAtPath<SpriteAtlas>(assetPath);
            Sprite[] sprites = new Sprite[spriteAtlas.spriteCount];
            spriteAtlas.GetSprites(sprites);

            foreach (var sprite in sprites)
            {
                string spriteTexturePath = AssetDatabase.GetAssetPath(sprite.texture);
                var importer = AssetImporter.GetAtPath(spriteTexturePath) as TextureImporter;
                if (importer == null) continue;
                var def = importer.GetDefaultPlatformTextureSettings();
                var changed = false;

                Action<TextureImporterPlatformSettings, TextureImporterFormat> maybeChange = (platSettings, destFmt) =>
                {
                    if (platSettings.format != destFmt ||
                        platSettings.compressionQuality != def.compressionQuality ||
                        platSettings.maxTextureSize != def.maxTextureSize ||
                        !platSettings.overridden)
                    {
                        platSettings.format = destFmt;
                        platSettings.compressionQuality = def.compressionQuality;
                        platSettings.maxTextureSize = def.maxTextureSize;
                        platSettings.overridden = true;

                        changed = true;
                        importer.SetPlatformTextureSettings(platSettings);
                    }
                };

                maybeChange(importer.GetPlatformTextureSettings("iPhone"), destFmtIphone);
                maybeChange(importer.GetPlatformTextureSettings("Android"), destFmtAndroid);

                if (!importer.ignorePngGamma)
                {
                    importer.ignorePngGamma = true;
                    changed = true;
                }

                TextureImporterSettings importerSettings = new TextureImporterSettings();
                importer.ReadTextureSettings(importerSettings);
                {
                    if (importerSettings.spriteGenerateFallbackPhysicsShape)
                    {
                        importerSettings.spriteGenerateFallbackPhysicsShape = false;
                        importer.SetTextureSettings(importerSettings);

                        changed = true;
                    }
                }

                if (changed)
                {
                    importer.SaveAndReimport();
                    ++numChanges;
                }
            }
        }

        Debug.Log($"Compress Image: {numChanges}");
    }
}
#endif