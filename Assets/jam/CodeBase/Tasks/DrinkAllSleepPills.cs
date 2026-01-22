using System;
using Cysharp.Threading.Tasks;
using jam.CodeBase.Character;
using jam.CodeBase.Core;
using UnityEngine;

namespace jam.CodeBase.Tasks
{
    [Serializable]
    public class DrinkAllSleepPills : BaseTask
    {
        public string ItemName;
        public override async UniTask Execute()
        {
            Debug.LogError("DrinkAllSleepPills");
            G.Saves.Get<RunSaveModel>().Data.ObtainedItems.Remove(ItemName);
            G.CharacterAnimator.PlayAnimation(AnimationType.TakeRedPill);

            await UniTask.WaitForSeconds(3);
        }
    }

    [Serializable]
    public class DoNotDrinkAllSleepPills : BaseTask
    {
        public override UniTask Execute()
        {
            Debug.LogError("DoNotDrinkAllSleepPills");
            return UniTask.CompletedTask;
        }
    }
}