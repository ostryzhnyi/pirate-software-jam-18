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
                  
            G.CharacterAnimator.PlayAnimation(AnimationType.Idle);
            
            G.Room.FanAnimator.Play(false);

       
            await UniTask.WaitForSeconds(4);
            G.Room.FanAnimator.Stop();
        }
    }

    [Serializable]
    public class DoNotEnableAC : BaseTask
    {
        public override async UniTask Execute()
        {
            Debug.LogError("TurnColdest");
            G.Room.FanAnimator.Play(true);
            
            await UniTask.WaitForSeconds(4);
            G.Room.FanAnimator.Stop();
        }
    }
}