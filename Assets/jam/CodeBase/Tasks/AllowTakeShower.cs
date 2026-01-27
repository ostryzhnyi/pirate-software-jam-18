using System;
using Cysharp.Threading.Tasks;
using jam.CodeBase.Character;
using jam.CodeBase.Core;
using UnityEngine;

namespace jam.CodeBase.Tasks
{
    [Serializable]
    public class AllowTakeShower : BaseTask
    {
        public override async UniTask Execute()
        {
            Debug.LogError("AllowTakeSHowe");
            G.Room.TVAnimator.Play(TVAnimation.ShowerTime, 4f);
            await G.CharacterAnimator.PlayMoveAnim(2f);
            G.CharacterAnimator.PlayAnimation(AnimationType.WetHair);
            G.Room.Steam.Play();
            await UniTask.WaitForSeconds(1f);
        }
    }
    
    [Serializable]
    public class DoNotAllowTakeShower : BaseTask
    {
        public override UniTask Execute()
        {
            Debug.LogError("DoNotAllowTakeSHowe");
            return UniTask.CompletedTask;
        }
    }
}