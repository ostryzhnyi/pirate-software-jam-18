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

        public async UniTask<bool> OnLoaded(RunSaveModel runSaveModel)
        {
            if(runSaveModel.Data.IsStarted)
                return  true;
            
            await G.BetController.BetProcess();

            var betSaveModel = G.Saves.Get<BetSaveModel>();
            betSaveModel.Data.IsFirst = false;
            betSaveModel.ForceSave();
            return  true;
            
        }
    }
}