using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using jam.CodeBase.Audio;
using jam.CodeBase.Bets;
using jam.CodeBase.Character;
using jam.CodeBase.Core.Interactors;
using jam.CodeBase.Core.SavesGeneral;
using jam.CodeBase.Stream;
using jam.CodeBase.Tasks;
using Ostryzhnyi.EasyViewService.Api.Service;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace jam.CodeBase.Core
{
    public class G : MonoBehaviour
    {
        public static Interactor Interactors;
        public static Saves Saves;
        public static Donate Donate = new Donate();
        public static M Menu;
        public static Economy.Economy Economy;
        public static Characters Characters;
        public static BetController BetController;
        public static IViewService GlobalViewService;
     
        public static AudioController Audio;
        
        public static CancellationToken GameAliveCancellationToken;

        private static CancellationTokenSource _cancellationTokenSourceStop;

        public static bool IsPaused = false;
        public static GameObject GameObject;
        public static StreamController StreamController;

        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void OnApplicationQuit() {
            Application.quitting += Quit;
        }


        private void Awake()
        {
            GameAliveCancellationToken = gameObject.GetCancellationTokenOnDestroy();
            GameObject = gameObject;
        }

        private void Start()
        {
            
            StartAsync().Forget();
        }

        private async UniTask StartAsync()
        {
            await StartGameLoop();
        }

        public static void StopInteractions()
        {
            _cancellationTokenSourceStop.Cancel();
        }

        public async UniTask StartGameLoop()
        {
            _cancellationTokenSourceStop = new CancellationTokenSource();
            InitControllers();
            await GameLoop();
        }

        private void InitControllers()
        {
            StreamController = new StreamController();
        }

        private async UniTask GameLoop()
        {
            await UniTask.DelayFrame(1);

            foreach (var awake in Interactors.GetAll<IOnAwake>())
            {
                await awake.OnAwake();
            }
            
            foreach (var start in Interactors.GetAll<IOnStart>())
            {
                await start.OnStart();
            }

            while (!_cancellationTokenSourceStop.IsCancellationRequested)
            {
                if (IsPaused)
                    await UniTask.WaitForEndOfFrame(cancellationToken: _cancellationTokenSourceStop.Token);
                
                var updates = Interactors.GetAll<IOnUpdate>();
                if(updates.Length > 0)
                {
                    foreach (var update in updates)
                    {
                        await update.OnUpdate();
                    }
                }
                else
                {
                    await UniTask.WaitForEndOfFrame(cancellationToken: _cancellationTokenSourceStop.Token);
                }
            }
            
            foreach (var destroy in Interactors.GetAll<IOnDestroy>())
            {
                await destroy.OnDestroy();
            }
        }
        
        
        static async void Quit()
        {
            try
            {
                DestroyAllObjects();

                await Awaitable.NextFrameAsync(); // Let GC clean up
                Debug.Log("Quitting application.");
            }
            catch (Exception e)
            {
                Debug.LogError($"Exception during quit process: {e.Message}\n{e.StackTrace}");
            }
            
#if !UNITY_EDITOR
            HardQuit();
#else
            UnityEditor.EditorApplication.ExitPlaymode();
#endif
        }
        
        static void DestroyAllObjects() {
            for (int i = 0; i < SceneManager.sceneCount; i++) {
                Scene scene = SceneManager.GetSceneAt(i);
                if (scene.isLoaded is false) continue;

                foreach (GameObject rootGameObject in scene.GetRootGameObjects()) {
                    Debug.Log($"Destroying root object: {rootGameObject.name} in scene: {scene.name}");
                    Object.Destroy(rootGameObject);
                }
            }
        }

        static void HardQuit() {
            Debug.Log("Hard quitting to avoid input handler freezing the game on quit issue.");

            [System.Runtime.InteropServices.DllImport("kernel32.dll", SetLastError = true)]
            static extern int TerminateProcess(System.IntPtr hProcess, uint exitCode);

            IntPtr id = System.Diagnostics.Process.GetCurrentProcess().Handle;
            TerminateProcess(id, 0);
        }
    }
}
