using System;
using Cysharp.Threading.Tasks;
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
            
            await UniTask.WaitForSeconds(4);
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