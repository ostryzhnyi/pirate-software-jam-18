using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace jam.CodeBase.Tasks
{
    [Serializable]
    public class TaskDefinition : EntityComponentDefinition
    {
    }

    [Serializable]
    public abstract class BaseTask : EntityComponentDefinition
    {
        public string Name;
        public float Price;

        public abstract UniTask Execute();
    }
}