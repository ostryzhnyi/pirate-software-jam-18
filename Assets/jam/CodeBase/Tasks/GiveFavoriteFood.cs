using System;
using Cysharp.Threading.Tasks;
using jam.CodeBase.Character;
using jam.CodeBase.Core;
using UnityEngine;

namespace jam.CodeBase.Tasks
{
    [Serializable]
    public class GiveFavoriteFood : BaseTask
    {
        public override async UniTask Execute()
        {
            Debug.LogError("GiveFavoriteFood");
            G.Room.TVAnimator.Play(TVAnimation.FoodTime, 3f);
            
            G.CharacterAnimator.PlayAnimation(AnimationType.Eat);

            await UniTask.WaitForSeconds(3);
        }
    }
    
    [Serializable]
    public class DoNotGiveFavoriteFood: BaseTask
    {
        public override UniTask Execute()
        {
            G.Room.TVAnimator.Play(TVAnimation.FoodTime, 3f);
            
            Debug.LogError("DoNotGiveFavoriteFood");
            return UniTask.CompletedTask;
        }
    }
}