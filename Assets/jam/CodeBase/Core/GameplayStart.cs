using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using jam.CodeBase.Core.Interactors;

namespace jam.CodeBase.Core
{
    public class GameplayStart : BaseInteractor, IGameplayLoaded
    {
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

            //todo:show card pop up
        }
    }
}