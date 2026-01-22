using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using jam.CodeBase.Core;
using jam.CodeBase.UI;
using Ostryzhnyi.EasyViewService.Impl.Service;
using Sirenix.OdinInspector;
using UnityEngine;

namespace jam.CodeBase
{
    public class M : MonoBehaviour
    {
        public ViewService ViewService;
        public HUD HUD;

        private void Awake()
        {
            G.Menu = this;
        }

        private void OnDestroy()
        {
            G.Menu = null;
            OnGamePlayUnloaded().Forget();
        }

        private static async UniTask OnGamePlayUnloaded()
        {
            foreach (var gameplayUnloaded in G.Interactors.GetAll<IGameplayUnloaded>())
            {
                await gameplayUnloaded.OnUnloaded();
            }
        }

        [Button]
        private void Win()
        {
            G.Win();
        }
        

        [Button]
        private void Lose()
        {
            G.Loose();
        }
    }
}