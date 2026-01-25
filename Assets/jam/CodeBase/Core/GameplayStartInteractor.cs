using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using jam.CodeBase.Character;
using jam.CodeBase.Character.Data;
using jam.CodeBase.Core.Interactors;
using jam.CodeBase.Utils;
using UnityEngine;

namespace jam.CodeBase.Core
{
    public class GameplayStartInteractor : BaseInteractor, IGameplayLoaded
    {
        public override int GetPriority()
        {
            return int.MinValue;
        }

        public async UniTask<bool> OnLoaded(RunSaveModel runSaveModel)
        {
            var loadedCharacter =
                G.Characters.CharactersList.FirstOrDefault(c => c.Name == runSaveModel.Data.CharacterName);

            if (!runSaveModel.Data.IsStarted || loadedCharacter == null)
            {
                var aliveCharacter = G.Characters.CharactersList
                    .Where(c => !c.IsDie)
                    .OrderBy(c => UnityEngine.Random.value)
                    .FirstOrDefault();
                if (aliveCharacter == null)
                {
                    G.Saves.Get<CharactersSaveModel>().Clear();
                    G.Characters.CharactersList = new List<Character.Character>();
                    
                    aliveCharacter = G.Characters.CharactersList
                        .Where(c => !c.IsDie)
                        .OrderBy(c => UnityEngine.Random.value)
                        .FirstOrDefault();
                }
                Debug.LogError( "ALIVE CHAR :" + aliveCharacter.Name);
        
                G.Characters.CurrentCharacter = aliveCharacter;
            }
            else
            {
                G.Characters.CurrentCharacter = loadedCharacter;
                Debug.LogError("LIVED CHARACTER IS ALREADY LOADED! : " + G.Characters.CurrentCharacter.Name);
                
                UpdateHUD();
                return true;
            }
            
            UpdateHUD();

            G.Menu.ViewService.ShowView<CharacterCardView>(new CharacterCardViewOptions(G.Characters.CurrentCharacter));
            G.CharacterAnimator.ApplyTexture(G.Characters.CurrentCharacter.Texture);
            
            await UniTask.WaitForSeconds(3f);
            await UniTaskHelper.SmartWaitSeconds(15);
            
            G.Menu.ViewService.HideView<CharacterCardView>();
            return true;
        }

        private static void UpdateHUD()
        {
            G.Menu.HUD.StatsView.UpdateStress(G.Characters.CurrentCharacter.BaseStress / G.Characters.CurrentCharacter.CurrentStress, true).Forget();
            G.Menu.HUD.StatsView.UpdateHP(G.Characters.CurrentCharacter.BaseHP / G.Characters.CurrentCharacter.CurrentHealth, true).Forget();
            switch (G.Characters.CurrentCharacter.CurrentHealth)
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
    }
}