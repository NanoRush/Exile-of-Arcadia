
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.IO;

public class ReplaceBarrierTile : EditorWindow
{
    private TileBase oldTile;
    private TileBase newTile;

    private readonly string[] targetScenes =
    {
        "Level1",
        "Level2",
        "Level3"
    };

    [MenuItem("Tools/Replace Barrier Tiles")]
    public static void ShowWindow()
    {
        GetWindow<ReplaceBarrierTile>("Replace Barrier Tiles");
    }

    private void OnGUI()
    {
        GUILayout.Label("Replace Barrier Tiles", EditorStyles.boldLabel);

        EditorGUILayout.HelpBox(
            "Only Level1, Level2, and Level3 will be modified.",
            MessageType.Info
        );

        oldTile = (TileBase)EditorGUILayout.ObjectField(
            "Old Barrier Tile", oldTile, typeof(TileBase), false);

        newTile = (TileBase)EditorGUILayout.ObjectField(
            "New Animated Tile", newTile, typeof(TileBase), false);

        EditorGUILayout.Space(10);

        GUI.enabled = oldTile != null &&
                      newTile != null &&
                      oldTile != newTile;

        if (GUILayout.Button("Scan Target Levels"))
            ScanScenes();

        if (GUILayout.Button("Replace All Instances"))
            ReplaceAll();

        GUI.enabled = true;
    }

    private List<string> GetScenePaths()
    {
        List<string> paths = new List<string>();

        foreach (string guid in AssetDatabase.FindAssets(
                     "t:Scene", new[] { "Assets" }))
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            string sceneName = Path.GetFileNameWithoutExtension(path);

            foreach (string target in targetScenes)
            {
                if (sceneName == target)
                {
                    paths.Add(path);
                    break;
                }
            }
        }

        return paths;
    }

    private int ProcessScene(Scene scene, bool replace)
    {
        int count = 0;

        foreach (GameObject root in scene.GetRootGameObjects())
        {
            Tilemap[] tilemaps =
                root.GetComponentsInChildren<Tilemap>(true);

            foreach (Tilemap tilemap in tilemaps)
            {
                BoundsInt bounds = tilemap.cellBounds;

                foreach (Vector3Int position in bounds.allPositionsWithin)
                {
                    if (tilemap.GetTile(position) == oldTile)
                    {
                        count++;

                        if (replace)
                        {
                            Undo.RegisterCompleteObjectUndo(
                                tilemap, "Replace Barrier Tiles");

                            tilemap.SetTile(position, newTile);
                        }
                    }
                }
            }
        }

        return count;
    }

    private Scene FindLoadedScene(string path)
    {
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);

            if (scene.path == path)
                return scene;
        }

        return default;
    }

    private void ScanScenes()
    {
        List<string> paths = GetScenePaths();

        if (paths.Count != 3)
        {
            EditorUtility.DisplayDialog(
                "Scene Warning",
                "Expected 3 scenes but found " + paths.Count +
                ". Check your scene names and paths before proceeding.",
                "OK"
            );
            return;
        }

        string report = "";
        int total = 0;

        foreach (string path in paths)
        {
            Scene scene = FindLoadedScene(path);
            bool openedHere = !scene.IsValid();

            if (openedHere)
                scene = EditorSceneManager.OpenScene(
                    path, OpenSceneMode.Additive);

            int count = ProcessScene(scene, false);

            report += Path.GetFileNameWithoutExtension(path) +
                      ": " + count + " tiles\n";

            total += count;

            if (openedHere)
                EditorSceneManager.CloseScene(scene, true);
        }

        EditorUtility.DisplayDialog(
            "Scan Complete",
            report + "\nTotal: " + total + " tiles",
            "OK"
        );
    }

    private void ReplaceAll()
    {
        List<string> paths = GetScenePaths();

        if (paths.Count != 3)
        {
            EditorUtility.DisplayDialog(
                "Scene Warning",
                "Expected exactly 3 scenes. Found " + paths.Count +
                ". Replacement cancelled.",
                "OK"
            );
            return;
        }

        if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            return;

        if (!EditorUtility.DisplayDialog(
            "Confirm Replacement",
            "Replace all matching tiles in Level1, Level2, and Level3 only?",
            "Replace All",
            "Cancel"))
            return;

        int total = 0;

        try
        {
            for (int i = 0; i < paths.Count; i++)
            {
                string path = paths[i];

                EditorUtility.DisplayProgressBar(
                    "Replacing Barrier Tiles",
                    path,
                    (float)i / paths.Count
                );

                Scene scene = FindLoadedScene(path);
                bool openedHere = !scene.IsValid();

                if (openedHere)
                    scene = EditorSceneManager.OpenScene(
                        path, OpenSceneMode.Additive);

                int count = ProcessScene(scene, true);
                total += count;

                if (count > 0)
                {
                    EditorSceneManager.MarkSceneDirty(scene);
                    EditorSceneManager.SaveScene(scene);
                }

                if (openedHere)
                    EditorSceneManager.CloseScene(scene, true);
            }
        }
        finally
        {
            EditorUtility.ClearProgressBar();
        }

        AssetDatabase.SaveAssets();

        EditorUtility.DisplayDialog(
            "Replacement Complete",
            "Replaced " + total +
            " tiles across Level1, Level2, and Level3.",
            "OK"
        );
    }
}
