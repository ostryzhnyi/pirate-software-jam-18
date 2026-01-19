using Cysharp.Threading.Tasks;
using jam.CodeBase.Audio;
using jam.CodeBase.Character;
using jam.CodeBase.Core.Interactors;
using jam.CodeBase.Core.SavesGeneral;
using Ostryzhnyi.EasyViewService.Impl.Service;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace jam.CodeBase.Core
{
    public abstract class Boot
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
        private static void StartGame()
        {
            //PlayerPrefs.DeleteAll();
            CMS.Unload();
            CMS.Init();
            
            G.Interactors = new Interactor();
            G.Saves = new Saves();
            G.Economy = new Economy.Economy();
            G.Characters = new Characters();
            
            SpawnG();
            SpawnAudioController();
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void LoadGamePlay()
        { 
            //LoadGamePlayAsync().Forget();
        }

        private static async UniTask LoadGamePlayAsync()
        {
            await SceneManager.LoadSceneAsync("Gameplay");
            
            foreach (var levelLoaded in G.Interactors.GetAll<IGameplayLoaded>())
            {
                await levelLoaded.OnLoaded();
            }
        }
        
        private static void SpawnG()
        {
            var gObject = new GameObject("G");
            gObject.AddComponent<G>();
            var canvas = GameObject.Instantiate(GameResources.Canvas, gObject.transform).GetComponentInChildren<ViewServiceCanvas>();
            var viewService = canvas.gameObject.AddComponent<ViewService>();
            viewService._canvas = canvas;
            G.GlobalViewService = viewService;
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