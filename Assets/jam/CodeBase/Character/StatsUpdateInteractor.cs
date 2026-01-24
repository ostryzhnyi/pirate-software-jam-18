using Cysharp.Threading.Tasks;
using jam.CodeBase.Core;
using jam.CodeBase.Core.Interactors;

namespace jam.CodeBase.Character
{
    public class StatsUpdateInteractor : BaseInteractor, IGameplayLoaded, IGameplayUnloaded
    {
        public override int GetPriority()
        {
            return int.MaxValue;
        }

        public UniTask<bool> OnLoaded(RunSaveModel runSaveModel)
        {
            G.Characters.CurrentCharacter.OnHealthUpdated += OnHealthUpdated;
            G.Characters.CurrentCharacter.OnStressUpdated += OnStressUpdated;
            return UniTask.FromResult(true);
        }

        private void OnStressUpdated(float value)
        {
            G.Menu.HUD.StatsView.UpdateStress(value / G.Characters.CurrentCharacter.MaxStress).Forget();
            
            switch (value)
            {
                case >= 66:
                    G.CharacterAnimator.PlayAnimation(AnimationType.BadStress);
                    break;
                case < 66 and >= 33:
                    G.CharacterAnimator.PlayAnimation(AnimationType.NormalStress);
                    break;
                default:
                    G.CharacterAnimator.PlayAnimation(AnimationType.FineStress);
                    break;
            }
        }

        private void OnHealthUpdated(float value)
        {
            G.Menu.HUD.StatsView.UpdateHP(value / G.Characters.CurrentCharacter.BaseHP).Forget();

            switch (value)
            {
                case >= 66:
                    G.CharacterAnimator.PlayAnimation(AnimationType.FineHP);
                    break;
                case < 66 and >= 33:
                    G.CharacterAnimator.PlayAnimation(AnimationType.NormalHP);
                    break;
                default:
                    G.CharacterAnimator.PlayAnimation(AnimationType.BadHP);
                    break;
            }
        }

        public UniTask OnUnloaded()
        {
            try
            {
                G.Characters.CurrentCharacter.OnHealthUpdated -= OnHealthUpdated;
                G.Characters.CurrentCharacter.OnStressUpdated -= OnStressUpdated;
            }
            catch
            {
                //ignore
            }
            return UniTask.CompletedTask;
        }
    }
}