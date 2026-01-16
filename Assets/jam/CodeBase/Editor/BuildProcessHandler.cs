using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace jam.CodeBase.Editor
{
    public class BuildProcessHandler : IProcessSceneWithReport
    {
        public int callbackOrder => 0;

        public void OnProcessScene(Scene scene, BuildReport report)
        {
            var timeStamp = Object.FindAnyObjectByType<TimeStampBuild>();
            
            if (timeStamp != null)
            {
                timeStamp.Set();
                Debug.Log($"[Build] TimeStamp updated in scene: {scene.name}");
            }
        }
    }
}