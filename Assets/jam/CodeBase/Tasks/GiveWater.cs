using System;
using Cysharp.Threading.Tasks;
using jam.CodeBase.Character;
using jam.CodeBase.Core;
using UnityEngine;

namespace jam.CodeBase.Tasks
{
    [Serializable]
    public class GiveWater : BaseTask
    {
        public override async UniTask Execute()
        {
            G.Room.TVAnimator.Play(TVAnimation.FoodTime, 3f);
            G.BoxAnimator.PlayAnimation(BoxAnimationType.Bottle);
            
            await UniTask.WaitForSeconds(5.5f);
            G.CharacterAnimator.PlayAnimation(AnimationType.DrinkWater);

            await UniTask.WaitForSeconds(3);
        }
    }
    
    [Serializable]
    public class DoNotGiveWater : BaseTask
    {
        public override UniTask Execute()
        {
            Debug.LogError("DoNotGiveWater");
            return UniTask.CompletedTask;
        }
    }
}