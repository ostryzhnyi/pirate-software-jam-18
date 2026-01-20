using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace jam.CodeBase.Tasks
{
    [Serializable]
    public class GiveFood : BaseTask
    {
        public override UniTask Execute()
        {
            Debug.LogError("GiveFood");
            return UniTask.CompletedTask;
        }
    }

    [Serializable]
    public class DoNotGiveFood : BaseTask
    {
        public override UniTask Execute()
        {
            Debug.LogError("DoNotGiveFood");
            return UniTask.CompletedTask;
        }
    }
}