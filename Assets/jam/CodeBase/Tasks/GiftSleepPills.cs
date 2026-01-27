using System;
using Cysharp.Threading.Tasks;
using jam.CodeBase.Character;
using jam.CodeBase.Core;
using UnityEngine;

namespace jam.CodeBase.Tasks
{
    [Serializable]
    public class GiftSleepPills : BaseTask
    {
        public string ItemName;
        public override async UniTask Execute()
        {
            Debug.LogError("GiftSleepPills");
            G.Saves.Get<RunSaveModel>().Data.ObtainedItems.Add(ItemName);
            G.Saves.Get<RunSaveModel>().ForceSave();
            G.BoxAnimator.PlayAnimation(BoxAnimationType.Pils);
            
            await UniTask.WaitForSeconds(5.5f);
            
            G.CharacterAnimator.PlayAnimation(AnimationType.TakeRedPill);

            await UniTask.WaitForSeconds(3);
        }
    }

    [Serializable]
    public class DoNotGiftSleepPills : BaseTask
    {
        public override UniTask Execute()
        {
            Debug.LogError("DoNotGiftSleepPills");
            return UniTask.CompletedTask;
        }
    }
}