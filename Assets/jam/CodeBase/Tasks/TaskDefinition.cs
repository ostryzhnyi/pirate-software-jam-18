using System;
using Cysharp.Threading.Tasks;

namespace jam.CodeBase.Tasks
{
    [Serializable]
    public class TaskDefinition : EntityComponentDefinition
    {
        public float Duration;
    }

    [Serializable]
    public abstract class BaseTask : EntityComponentDefinition
    {
        public string Name;
        public float Price;

        public abstract UniTask Execute();
    }
}