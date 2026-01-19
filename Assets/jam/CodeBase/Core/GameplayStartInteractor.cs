using System.Linq;
using Cysharp.Threading.Tasks;
using jam.CodeBase.Character;
using jam.CodeBase.Core.Interactors;
using jam.CodeBase.Utils;

namespace jam.CodeBase.Core
{
    public class GameplayStartInteractor : BaseInteractor, IGameplayLoaded
    {
        public override int GetPriority()
        {
            return int.MinValue;
        }

        public async UniTask OnLoaded()
        {
            var runSaveModel = G.Saves.Get<RunSaveModel>();

            var loadedCharacter =
                G.Characters.CharactersList.FirstOrDefault(c => c.Name == runSaveModel.Data.CharacterName);

            if (loadedCharacter == null)
            {
                var aliveCharacter = G.Characters.CharactersList
                    .Where(c => !c.IsDie)
                    .OrderBy(c => UnityEngine.Random.value)
                    .FirstOrDefault();

                G.Characters.CurrentCharacter = aliveCharacter;
            }
            else
            {
                G.Characters.CurrentCharacter = loadedCharacter;
            }
            
            G.Menu.HUD.StatsView.UpdateStress(G.Characters.CurrentCharacter.CurrentStress, true).Forget();
            G.Menu.HUD.StatsView.UpdateHP(G.Characters.CurrentCharacter.CurrentHealth, true).Forget();

            G.Menu.ViewService.ShowView<CharacterCardView>(new CharacterCardViewOptions(G.Characters.CurrentCharacter));

            await UniTaskHelper.SmartWaitSeconds(15);
            
            G.Menu.ViewService.HideView<CharacterCardView>();
        }
    }
}