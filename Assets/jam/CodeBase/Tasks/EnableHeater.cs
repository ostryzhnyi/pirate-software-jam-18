using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace jam.CodeBase.Tasks
{
    [Serializable]
    public class EnableHeater : BaseTask
    {
        public override UniTask Execute()
        {
            Debug.LogError("EnableHeater");
            return UniTask.CompletedTask;
        }
    }
    
    [Serializable]
    public class DoNotEnableHeater : BaseTask
    {
        public override UniTask Execute()
        {
            Debug.LogError("DoNotEnableHeater");
            return UniTask.CompletedTask;
        }
    }
}