using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace jam.CodeBase.Tasks
{
    [Serializable]
    public class SitOnElectricChair : BaseTask
    {
        public override UniTask Execute()
        {
            Debug.LogError("SitOnElectricChair");
            return UniTask.CompletedTask;
        }
    }

    [Serializable]
    public class DoNotSitOnElectricChair : BaseTask
    {
        public override UniTask Execute()
        {
            Debug.LogError("DoNotSitOnElectricChair");
            return UniTask.CompletedTask;
        }
    }
}