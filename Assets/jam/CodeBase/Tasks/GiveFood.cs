using System;
using Cysharp.Threading.Tasks;
using jam.CodeBase.Character;
using jam.CodeBase.Core;
using UnityEngine;

namespace jam.CodeBase.Tasks
{
    [Serializable]
    public class GiveFood : BaseTask
    {
        public override async UniTask Execute()
        {
            Debug.LogError("GiveFood");
            
            G.Room.TVAnimator.Play(TVAnimation.FoodTime, 3f);

            G.CharacterAnimator.PlayAnimation(AnimationType.Eat);

            await UniTask.WaitForSeconds(3);
        }
    }

    [Serializable]
    public class DoNotGiveFood : BaseTask
    {
        public override UniTask Execute()
        {
            G.Room.TVAnimator.Play(TVAnimation.FoodTime, 3f);
            
            Debug.LogError("DoNotGiveFood");
            return UniTask.CompletedTask;
        }
    }
}