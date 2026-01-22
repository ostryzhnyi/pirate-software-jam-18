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
            G.Saves.Get<RunSaveModel>().Data.ObtainedItems.Add(ItemName);
            G.Saves.Get<RunSaveModel>().ForceSave();
            G.CharacterAnimator.PlayAnimation(AnimationType.DrinkLimonade);

            await UniTask.WaitForSeconds(3);
        }
    }


    [Serializable]
    public class DoNotGiftSodaGlassBottle : BaseTask
    {
        public override UniTask Execute()
        {
            Debug.LogError("DoNotGiftSodaGlassBottle");
            return UniTask.CompletedTask;
        }
    }
}