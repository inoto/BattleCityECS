using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor.SceneManagement;

public static class MultiplayersBuildAndRun {

	[MenuItem("File/Run Multiplayer")]
	static void PerformWin64Build2 (){
		PerformWin64Build (2);
	}

    [MenuItem("File/Check Build")]
    static void CheckBuild()
    {
        PerformWin64Build(1);
    }

	static void PerformWin64Build (int playerCount)
	{
		EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTarget.StandaloneWindows);
		for (int i = 1; i <= playerCount; i++) {
			BuildPipeline.BuildPlayer (GetScenePaths (), "Builds/Win64/" + GetProjectName () + i.ToString() + ".exe", BuildTarget.StandaloneWindows64, BuildOptions.AutoRunPlayer);
		}
	}

    static string GetProjectName()
	{
		string[] s = Application.dataPath.Split('/');
		return s[s.Length - 2];
	}

    [MenuItem("File/Get Scenes paths")]
	static string[] GetScenePaths()
	{
		string[] scenes = new string[EditorBuildSettings.scenes.Length];

        for (int i = 0; i < scenes.Length; i++)
        {
            scenes[i] = EditorBuildSettings.scenes[i].path;
        }

        return scenes;
	}

}