using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace jam.CodeBase.Tasks
{
    [Serializable]
    public class EnableAC : BaseTask
    {
        public override UniTask Execute()
        {
            Debug.LogError("EnableAC");
            return UniTask.CompletedTask;
        }
    }

    [Serializable]
    public class DoNotEnableAC : BaseTask
    {
        public override UniTask Execute()
        {
            Debug.LogError("DoNotEnableAC");
            return UniTask.CompletedTask;
        }
    }
}