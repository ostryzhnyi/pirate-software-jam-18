using System;
using Cysharp.Threading.Tasks;
using jam.CodeBase.Character;
using jam.CodeBase.Core;
using UnityEngine;

namespace jam.CodeBase.Tasks
{
    [Serializable]
    public class EnableAC : BaseTask
    {
        public override async UniTask Execute()
        {
            Debug.LogError("EnableAC");
            G.Room.FanAnimator.Play(false);
            
            G.CharacterAnimator.PlayAnimation(AnimationType.SetHappy);
            
            await UniTask.WaitForSeconds(4);
            G.Room.FanAnimator.Stop();
        }
    }

    [Serializable]
    public class DoNotEnableAC : BaseTask
    {
        public override async UniTask Execute()
        {
            Debug.LogError("DoNotEnableAC");
            
            G.CharacterAnimator.PlayAnimation(AnimationType.Idle);
            
            G.Room.FanAnimator.Play(true);
            
            await UniTask.WaitForSeconds(4);
            G.Room.FanAnimator.Stop();
        }
    }
}