using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace jam.CodeBase.Tasks
{
    [Serializable]
    public class SitOnBladesChair : BaseTask
    {
        public override UniTask Execute()
        {
            Debug.LogError("SitOnBladesChair");
            return UniTask.CompletedTask;
        }
    }

    [Serializable]
    public class DoNotSitOnBladesChair : BaseTask
    {
        public override UniTask Execute()
        {
            Debug.LogError("DoNotSitOnBladesChair");
            return UniTask.CompletedTask;
        }
    }
}