using Cysharp.Threading.Tasks;
using jam.CodeBase.Core;
using jam.CodeBase.Core.Interactors;

namespace jam.CodeBase
{
    public class RunInitializatorInteractor : BaseInteractor, IGameplayLoaded
    {
        public override int GetPriority()
        {
            return int.MaxValue - 1;
        }

        public UniTask OnLoaded(RunSaveModel runSaveModel1)
        {
            var runSaveModel = G.Saves.Get<RunSaveModel>();
            
            if(!runSaveModel.Data.IsStarted)
            {
                runSaveModel.Data.CharacterName = G.Characters.CurrentCharacter.Name;
                runSaveModel.Data.IsStarted = true;
                runSaveModel.Data.DayNumber = 1;
                
                runSaveModel.ForceSave();
            }
            G.DaysController.SetDay(runSaveModel.Data.DayNumber);
            
            return UniTask.CompletedTask;
        }
    }
}