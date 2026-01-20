using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace jam.CodeBase.Tasks
{
    [Serializable]
    public class AllowTakeShower : BaseTask
    {
        public override UniTask Execute()
        {
            Debug.LogError("AllowTakeSHowe");
            return UniTask.CompletedTask;
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