using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace jam.CodeBase.Tasks
{
    [Serializable]
    public class GiveFavoriteFood : BaseTask
    {
        public override UniTask Execute()
        {
            Debug.LogError("GiveFavoriteFood");
            return UniTask.CompletedTask;
        }
    }
    
    [Serializable]
    public class DoNotGiveFavoriteFood: BaseTask
    {
        public override UniTask Execute()
        {
            Debug.LogError("DoNotGiveFavoriteFood");
            return UniTask.CompletedTask;
        }
    }
}