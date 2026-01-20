using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace jam.CodeBase.Tasks
{
    [Serializable]
    public class SelectLeftPill : BaseTask
    {
        public override UniTask Execute()
        {
            Debug.LogError("GiveFavoriteFood");
            return UniTask.CompletedTask;
        }
    }

    [Serializable]
    public class SelectRightPill : BaseTask
    {
        public override UniTask Execute()
        {
            Debug.LogError("DoNotGiveFavoriteFood");
            return UniTask.CompletedTask;
        }
    }
}