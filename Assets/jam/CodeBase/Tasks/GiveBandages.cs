using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace jam.CodeBase.Tasks
{
    [Serializable]
    public class GiveBandages : BaseTask
    {
        public override UniTask Execute()
        {
            Debug.LogError("GiveBandages");
            return UniTask.CompletedTask;
        }
    }

    [Serializable]
    public class DoNotGiveBandages : BaseTask
    {
        public override UniTask Execute()
        {
            Debug.LogError("DoNotGiveBandages");
            return UniTask.CompletedTask;
        }
    }
}