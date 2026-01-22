using System;
using Cysharp.Threading.Tasks;
using jam.CodeBase.Core;
using jam.CodeBase.Core.Interactors;
using UnityEngine;

namespace jam.CodeBase
{
    public class RunInitializatorInteractor : BaseInteractor, IGameplayLoaded
    {
        public override int GetPriority()
        {
            return -5;
        }

        public async UniTask OnLoaded(RunSaveModel runSaveModel1)
        {
            var runSaveModel = G.Saves.Get<RunSaveModel>();


            if(!runSaveModel.Data.IsStarted)
            {
                runSaveModel.Data.CharacterName = G.Characters.CurrentCharacter.Name;
                runSaveModel.Data.IsStarted = true;
                runSaveModel.Data.DayNumber = 1;
                
                runSaveModel.ForceSave();
            }
            try
            {
                G.DaysController.SetDay(runSaveModel.Data.DayNumber);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                G.DaysController.SetDay(1);
            }
            await UniTask.WaitForSeconds(3);
        }
    }
}