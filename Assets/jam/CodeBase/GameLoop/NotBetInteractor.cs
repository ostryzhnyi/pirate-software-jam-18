using Cysharp.Threading.Tasks;
using jam.CodeBase.Core;
using jam.CodeBase.Core.Interactors;

namespace jam.CodeBase.GameLoop
{
    public class NotBetInteractor : BaseInteractor, IGameplayLoaded
    {
        public override int GetPriority()
        {
            return -9;
        }
        
        public UniTask<bool> OnLoaded(RunSaveModel runSaveModel)
        {
            if (G.BetController.MyBet <= 0)
            {
                G.Menu.ViewService.ShowView<NotStartedWindow>();
                return UniTask.FromResult(false);
            }
            
            return UniTask.FromResult(true);

        }
    }
}