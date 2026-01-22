using System;
using Cysharp.Threading.Tasks;
using jam.CodeBase.Character;
using jam.CodeBase.Core;
using UnityEngine;

namespace jam.CodeBase.Tasks
{
    [Serializable]
    public class SelectLeftPill : BaseTask
    {
        public override async UniTask Execute()
        {
            Debug.LogError("TakeBluePill");
            G.CharacterAnimator.PlayAnimation(AnimationType.TakeBluePill);

            await UniTask.WaitForSeconds(3);
        }
    }

    [Serializable]
    public class SelectRightPill : BaseTask
    {
        public override async UniTask Execute()
        {
            Debug.LogError("TakeRedPill");
            G.CharacterAnimator.PlayAnimation(AnimationType.TakeRedPill);

            await UniTask.WaitForSeconds(3);
        }
    }
}