using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace jam.CodeBase.Tasks
{
    [Serializable]
    public class GiveEatTask : BaseTask
    {
        public override UniTask Execute()
        {
            Debug.LogError("give stats");
            return UniTask.CompletedTask;
            
        }
    }
    
    [Serializable]
    public class NotGiveEatTask : BaseTask
    {
        public override UniTask Execute()
        {
            Debug.LogError("Not give stats");
            return UniTask.CompletedTask;
        }
    }
}