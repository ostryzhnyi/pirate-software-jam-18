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
                "Assets/jam/Scenes/Gameplay.unity",
            };
            
            PlayerSettings.defaultWebScreenWidth = 1340;
            PlayerSettings.defaultWebScreenHeight = 710;

            BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
            buildPlayerOptions.scenes = scenes;
            buildPlayerOptions.locationPathName = "webgl_build";
            buildPlayerOptions.target = BuildTarget.WebGL;
            buildPlayerOptions.options = BuildOptions.None;
            buildPlayerOptions.extraScriptingDefines = new []{"DOTWEEN", "UNITASK_DOTWEEN_SUPPORT", "ODIN_INSPECTOR", "ODIN_INSPECTOR_3", "ODIN_INSPECTOR_3_1", "ODIN_INSPECTOR_3_2", "ODIN_INSPECTOR_3_3"};

            BuildPipeline.BuildPlayer(scenes, "webgl_build", BuildTarget.WebGL, BuildOptions.None);
        }
    }
}