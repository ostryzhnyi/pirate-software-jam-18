using jam.CodeBase.Audio;
using jam.CodeBase.Core.Interactors;
using jam.CodeBase.Core.SavesGeneral;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace jam.CodeBase.Core
{
    public abstract class Boot
    {
        private const string LoadFromMenuKey = "LoadFromMenu";
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
        private static void StartGame()
        {
            //PlayerPrefs.DeleteAll();
            
#if UNITY_EDITOR
            if (EditorPrefs.GetBool(LoadFromMenuKey, false))
            {
              SceneManager.LoadScene("Menu");
            }
#endif
            SpawnG();
            SpawnAudioController();
            CMS.Unload();
            CMS.Init();
            G.Interactors = new Interactor();
            G.Saves = new Saves();
        }
        
        private static void SpawnG()
        {
            var gObject = new GameObject("G");
            gObject.AddComponent<G>();
            Object.DontDestroyOnLoad(gObject);
        }

        private static void SpawnAudioController()
        {
            var bootstrap = new GameObject("AudioController");
            bootstrap.AddComponent<AudioControllerBootstrap>();
            Object.DontDestroyOnLoad(bootstrap);
        }
    }
}