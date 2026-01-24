using System;
using Cysharp.Threading.Tasks;
using jam.CodeBase.Character;
using jam.CodeBase.Core;
using UnityEngine;

namespace jam.CodeBase.Tasks
{
    [Serializable]
    public class CrashGlassBottleWithHead : BaseTask
    {
        public string ItemName;
        public override async UniTask Execute()
        {
            Debug.LogError("CrashGlassBottleWithHead");
            
            G.Room.TVAnimator.Play(TVAnimation.BrakeIt, 3f);
            
            G.Saves.Get<RunSaveModel>().Data.ObtainedItems.Remove(ItemName);
            G.CharacterAnimator.PlayAnimation(AnimationType.BrokeGlass);
            await UniTask.WaitForSeconds(3f);
        }
    }

    [Serializable]
    public class DoNotCrashGlassBottleWithHead : BaseTask
    {
        public override UniTask Execute()
        {
            G.Room.TVAnimator.Play(TVAnimation.BrakeIt, 3f);
            
            Debug.LogError("DoNotCrashGlassBottleWithHead");
            return UniTask.CompletedTask;
        }
    }

}