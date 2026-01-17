using System;
using Cysharp.Threading.Tasks;

namespace jam.CodeBase.Tasks
{
    [Serializable]
    public class MusicOnNightTask : BaseTask
    {
        public override UniTask Execute()
        {
            return UniTask.CompletedTask;
        }
    }
    
    [Serializable]
    public class NotMusicOnNightTask : BaseTask
    {
        public override UniTask Execute()
        {
            return UniTask.CompletedTask;
            
        }
    }
}