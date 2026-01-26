using System;
using Cysharp.Threading.Tasks;
using jam.CodeBase.Character;
using jam.CodeBase.Core;
using UnityEngine;

namespace jam.CodeBase.Tasks
{
    [Serializable]
    public class GiftSodaGlassBottle : BaseTask
    {
        public string ItemName;

        public override async UniTask Execute()
        {
            Debug.LogError("GiftSodaGlassBottle");
            G.Room.TVAnimator.Play(TVAnimation.FoodTime, 3f);
            
            G.Saves.Get<RunSaveModel>().Data.ObtainedItems.Add(ItemName);
            G.Saves.Get<RunSaveModel>().ForceSave();
            
            G.BoxAnimator.PlayAnimation(BoxAnimationType.Bottle);
            
            await UniTask.WaitForSeconds(5.5f);
            
            G.CharacterAnimator.PlayAnimation(AnimationType.DrinkLimonade);

            await UniTask.WaitForSeconds(3);
        }
    }


    [Serializable]
    public class DoNotGiftSodaGlassBottle : BaseTask
    {
        public override UniTask Execute()
        {
            G.Room.TVAnimator.Play(TVAnimation.FoodTime, 3f);
            
            Debug.LogError("DoNotGiftSodaGlassBottle");
            return UniTask.CompletedTask;
        }
    }
}