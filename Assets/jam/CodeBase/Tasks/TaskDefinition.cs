using System;
using Cysharp.Threading.Tasks;

namespace jam.CodeBase.Tasks
{
    [Serializable]
    public class TaskDefinition : EntityComponentDefinition
    {
        public string Description;
        public float Duration;
        public float BasePrice;
    }

    [Serializable]
    public abstract class BaseTask : EntityComponentDefinition
    {
        public string Name;

        public abstract UniTask Execute();
    }
}