using Cysharp.Threading.Tasks;
using jam.CodeBase.Core;
using jam.CodeBase.Core.Interactors;

namespace jam.CodeBase.Bets
{
    public class StartBetProcessInteractor : BaseInteractor, IGameplayLoaded
    {
        public override int GetPriority()
        {
            return -10;
        }

        public async UniTask OnLoaded(RunSaveModel runSaveModel)
        {
            if(runSaveModel.Data.IsStarted)
                return;
            
            await G.BetController.BetProcess();

            var betSaveModel = G.Saves.Get<BetSaveModel>();
            betSaveModel.Data.IsFirst = false;
            betSaveModel.ForceSave();
        }
    }
}