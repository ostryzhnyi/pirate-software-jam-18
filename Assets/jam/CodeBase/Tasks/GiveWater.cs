using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace jam.CodeBase.Tasks
{
    [Serializable]
    public class GiveWater : BaseTask
    {
        public override UniTask Execute()
        {
            Debug.LogError("GiveWater");
            return UniTask.CompletedTask;
        }
    }
    
    [Serializable]
    public class DoNotGiveWater : BaseTask
    {
        public override UniTask Execute()
        {
            Debug.LogError("DoNotGiveWater");
            return UniTask.CompletedTask;
        }
    }
}