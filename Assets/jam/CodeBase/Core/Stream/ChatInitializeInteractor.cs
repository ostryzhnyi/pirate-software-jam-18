using System;
using Cysharp.Threading.Tasks;
using jam.CodeBase.Core.Interactors;

namespace jam.CodeBase.Core.Stream
{
    public class ChatInitializeInteractor : BaseInteractor, IGameplayLoaded, IGameplayUnloaded
    {
        public async UniTask OnLoaded()
        {
            G.StreamController.DaysController.OnDayEnded += OnDayChangedTransition;
            
            G.StreamController.StartStream(G.Characters.CurrentCharacter.Entity);
            
            G.StreamController.OnCharActionExecuted(1);
        }

        private void OnDayChangedTransition(int obj)
        {
            
        }

        public UniTask OnUnloaded()
        {
            try
            {
                G.StreamController.DaysController.OnDayEnded -= OnDayChangedTransition;
            }
            catch
            {
                // ignored
            }

            return UniTask.CompletedTask;
        }
    }
}