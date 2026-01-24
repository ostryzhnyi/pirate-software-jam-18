using Cysharp.Threading.Tasks;
using jam.CodeBase.Core;
using jam.CodeBase.Core.Interactors;

namespace jam.CodeBase.Tasks.Interactors
{
    public class InitializeDonateProcess : BaseInteractor, IGameplayLoaded
    {
        public override int GetPriority()
        {
            return 1;
        }

        public UniTask<bool> OnLoaded(RunSaveModel runSaveModel)
        {
            G.Donate.DonateExecuteProcess().Forget();
            return UniTask.FromResult(true);
        }
    }
}