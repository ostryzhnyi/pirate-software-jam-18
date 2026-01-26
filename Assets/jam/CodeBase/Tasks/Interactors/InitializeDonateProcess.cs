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
            if (GameResources.CMS.DebugRun.AsEntity().Is<DebugRunTag>(out var tag) && tag.OnlyChat)
            {
                G.ChatMiniGame.Play().Forget();
                return UniTask.FromResult(true);
                
            }

            G.Donate.DonateExecuteProcess().Forget();
            
            if (!G.Saves.Get<FTUESaveModel>().Data.ShowedDonateFTUE)
            {
                G.Menu.HUD.PlayDonateFTUE().Forget();
            }
            return UniTask.FromResult(true);
        }
    }
}