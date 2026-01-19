using Cysharp.Threading.Tasks;
using jam.CodeBase.Core;
using jam.CodeBase.Core.Interactors;
using UnityEngine;

namespace jam.CodeBase.Tasks.Interactors
{
    public class InitializeDonateProcess : BaseInteractor, IGameplayLoaded
    {
        public override int GetPriority()
        {
            return 1;
        }

        public UniTask OnLoaded()
        {
            Debug.LogError("!!!");
            G.Donate.DonateExecuteProcess().Forget();
            return UniTask.CompletedTask;
        }
    }
}