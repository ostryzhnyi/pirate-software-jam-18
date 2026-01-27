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
            G.BoxAnimator.PlayAnimation(BoxAnimationType.Pils);
            
            await UniTask.WaitForSeconds(5.5f);
            
            G.Room.TVAnimator.Play(TVAnimation.ChooseRedOrBlue, 3f);

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
            G.Room.TVAnimator.Play(TVAnimation.ChooseRedOrBlue, 3f);

            await UniTask.WaitForSeconds(3);
        }
    }
}