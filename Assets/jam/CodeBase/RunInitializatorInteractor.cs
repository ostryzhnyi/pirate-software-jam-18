using System;
using Cysharp.Threading.Tasks;
using jam.CodeBase.Character;
using jam.CodeBase.Core;
using jam.CodeBase.Core.Interactors;
using UnityEngine;

namespace jam.CodeBase
{
    public class RunInitializatorInteractor : BaseInteractor, IGameplayLoaded, IAliveCharacter, IDieStressCharacter, IDieHealthCharacter
    {
        public override int GetPriority()
        {
            return -5;
        }

        public async UniTask<bool> OnLoaded(RunSaveModel runSaveModel1)
        {
            var runSaveModel = G.Saves.Get<RunSaveModel>();


            if(!runSaveModel.Data.IsStarted)
            {
                runSaveModel.Data.CharacterName = G.Characters.CurrentCharacter.Name;
                runSaveModel.Data.IsStarted = true;
                runSaveModel.Data.DayNumber = 1;
                
                runSaveModel.ForceSave();
            }
            
            if (GameResources.CMS.DebugRun.AsEntity().Is<DebugRunTag>(out var tag))
            {
                if (tag.OverrideDay != -1)
                {
                    runSaveModel.Data.DayNumber = tag.OverrideDay;
                    Debug.LogError("OVVERIDE CURRENT  DAY: " + runSaveModel.Data.DayNumber);
                    
                }
                if (tag.OverrideCurentDonate != -1)
                {
                    runSaveModel.Data.CurrentDonateNumberInDay = tag.OverrideCurentDonate;
                    Debug.LogError("OVVERIDE OverrideCurentDonate: " +  tag.OverrideCurentDonate);
                }
                runSaveModel.ForceSave();
            }
            
            try
            {
                G.DaysController.SetDay(runSaveModel.Data.DayNumber).Forget();
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                G.DaysController.SetDay(1).Forget();
            }
            
            G.Room.TVAnimator.Play(TVAnimation.WorldIsWatching, int.MaxValue);
            
            await UniTask.WaitForSeconds(3);
            return true;
        }

        public UniTask OnAlive(Character.Character character)
        {
            G.Alive();
            return UniTask.CompletedTask;
        }

        public async UniTask OnDie(Character.Character character)
        {
            G.Room.TVAnimator.Play(TVAnimation.GameOver, 4f);
            G.CharacterAnimator.PlayMoveAnim(8).Forget();
            
            await UniTask.WaitForSeconds(4f);        

            G.Die();
        }

        async UniTask  IDieHealthCharacter.OnDie(Character.Character character)
        {
            G.Room.TVAnimator.Play(TVAnimation.GameOver, 4f);
            G.CharacterAnimator.PlayMoveAnim(8).Forget();
            
            await UniTask.WaitForSeconds(4f);        
            
            G.Die();
        }
    }
}