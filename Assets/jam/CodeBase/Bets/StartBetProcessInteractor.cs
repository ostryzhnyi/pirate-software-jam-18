using Cysharp.Threading.Tasks;
using jam.CodeBase.Core;
using jam.CodeBase.Core.Interactors;

namespace jam.CodeBase.Bets
{
    public class StartBetProcessInteractor : BaseInteractor, IGameplayLoaded
    {
        public override int GetPriority()
        {
            return -1;
        }

        public async UniTask OnLoaded(RunSaveModel runSaveModel)
        {
            if(runSaveModel.Data.IsStarted)
                return;
            
            await G.BetController.BetProcess();
        }
    }
}