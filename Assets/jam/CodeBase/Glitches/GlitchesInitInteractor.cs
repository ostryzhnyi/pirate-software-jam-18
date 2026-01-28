using Cysharp.Threading.Tasks;
using jam.CodeBase.Core;
using jam.CodeBase.Core.Interactors;

namespace jam.CodeBase.Glitches
{
    public class GlitchesInitInteractor : BaseInteractor, IGameplayLoaded
    {
        public override int GetPriority()
        {
            return int.MinValue + 2;
        }

        public UniTask<bool> OnLoaded(RunSaveModel runSaveModel)
        {
            G.Glitches.UpdateCharacter(G.Characters.CurrentCharacter);
            return UniTask.FromResult(true);
        }
    }
}