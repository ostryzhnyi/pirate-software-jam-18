using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace jam.CodeBase.Tasks
{
    [Serializable]
    public class GiftValerianRoot : BaseTask
    {
        public override UniTask Execute()
        {
            Debug.LogError("GiftValerianRoot");
            return UniTask.CompletedTask;
        }
    }

    [Serializable]
    public class DoNotGiftValerianRoot : BaseTask
    {
        public override UniTask Execute()
        {
            Debug.LogError("GiftValerianRoot");
            return UniTask.CompletedTask;
        }
    }
}