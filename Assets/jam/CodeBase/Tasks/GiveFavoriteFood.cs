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
            
            G.CharacterAnimator.PlayAnimation(AnimationType.Eat);

            await UniTask.WaitForSeconds(3);
        }
    }
    
    [Serializable]
    public class DoNotGiveFavoriteFood: BaseTask
    {
        public override UniTask Execute()
        {
            Debug.LogError("DoNotGiveFavoriteFood");
            return UniTask.CompletedTask;
        }
    }
}