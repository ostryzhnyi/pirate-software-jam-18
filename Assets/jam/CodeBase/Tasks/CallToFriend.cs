using System;
using Cysharp.Threading.Tasks;
using jam.CodeBase.Character;
using jam.CodeBase.Core;
using UnityEngine;

namespace jam.CodeBase.Tasks
{
    [Serializable]
    public class CallToFriend : BaseTask
    {
        public override async UniTask Execute()
        {
            Debug.LogError("EnableMusicOnNightTask");
            G.Room.TVAnimator.Play(TVAnimation.CallFriend, 4f);
            
            G.BoxAnimator.PlayAnimation(BoxAnimationType.Phone);
            
            await UniTask.WaitForSeconds(5.5f);
            
            G.CharacterAnimator.PlayAnimation(AnimationType.Call);

            await UniTask.WaitForSeconds(4);
        }
    }

    [Serializable]
    public class NoCallToFriend : BaseTask
    {
        public override UniTask Execute()
        {
            Debug.LogError("NotEnableMusicOnNightTask");
            return UniTask.CompletedTask;
        }
    }
}