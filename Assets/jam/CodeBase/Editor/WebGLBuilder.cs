using UnityEditor;
using UnityEngine;

namespace jam.CodeBase
{
    public class WebGLBuilder
    {
        [MenuItem("Build/Build WebGL")]
        public static void Build()
        {
            string[] scenes =
            {
                "Assets/Scenes/SampleScene.unity",
            };
            
            PlayerSettings.defaultWebScreenWidth = 1340;
            PlayerSettings.defaultWebScreenHeight = 710;

            BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
            buildPlayerOptions.scenes = scenes;
            buildPlayerOptions.locationPathName = "webgl_build";
            buildPlayerOptions.target = BuildTarget.WebGL;
            buildPlayerOptions.options = BuildOptions.None;
            Object.FindAnyObjectByType<TimeStampBuild>().Set();

            BuildPipeline.BuildPlayer(scenes, "webgl_build", BuildTarget.WebGL, BuildOptions.None);
        }
    }
}