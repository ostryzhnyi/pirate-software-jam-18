using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace jam.CodeBase.Tasks
{
    [Serializable]
    public class PlayMovie : BaseTask
    {
        public override UniTask Execute()
        {
            Debug.LogError("PlayMovie");
            return UniTask.CompletedTask;
        }
    }
    
    [Serializable]
    public class DoNotPlayMovie : BaseTask
    {
        public override UniTask Execute()
        {
            Debug.LogError("DoNotPlayMovie");
            return UniTask.CompletedTask;
        }
    }
}