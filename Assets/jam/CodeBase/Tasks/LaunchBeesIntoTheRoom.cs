using System;
using Cysharp.Threading.Tasks;
using jam.CodeBase.Character;
using jam.CodeBase.Core;
using UnityEngine;

namespace jam.CodeBase.Tasks
{
    [Serializable]
    public class LaunchBeesIntoTheRoom : BaseTask
    {
        public override async UniTask Execute()
        {
            Debug.LogError("LaunchBeesIntoTheRoom");
            
            G.CharacterAnimator.PlayAnimation(AnimationType.SetSad);
            G.Room.Bee.Play();
            await UniTask.WaitForSeconds(3f);
        }
    }

    [Serializable]
    public class NotLaunchBeesIntoTheRoom : BaseTask
    {
        public override UniTask Execute()
        {
            Debug.LogError("NotLaunchBeesIntoTheRoom");
            return UniTask.CompletedTask;
        }
    }
}