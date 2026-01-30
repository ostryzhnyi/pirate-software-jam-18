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
            
            G.BoxAnimator.PlayAnimation(BoxAnimationType.Pils);
            await UniTask.WaitForSeconds(6f);
            G.CharacterAnimator.PlayAnimation(AnimationType.TakeBluePill);
            
            await UniTask.WaitForSeconds(2f);
            
        }
    }

    [Serializable]
    public class SelectRightPill : BaseTask
    {
        public override async UniTask Execute()
        {
            Debug.LogError("TakeRedPill");
            G.Room.TVAnimator.Play(TVAnimation.ChooseRedOrBlue, 8f);
            
            G.BoxAnimator.PlayAnimation(BoxAnimationType.Pils);
            await UniTask.WaitForSeconds(6f);
            G.CharacterAnimator.PlayAnimation(AnimationType.TakeRedPill);
            
            await UniTask.WaitForSeconds(2f);
        }
    }
}