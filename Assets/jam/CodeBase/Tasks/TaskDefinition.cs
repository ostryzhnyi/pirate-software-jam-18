using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using jam.CodeBase.Character;

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
        public List<StatsAfforded> StatsAfforded;

        public abstract UniTask Execute();
    }
    
    [Serializable]
    public class RequireItem : EntityComponentDefinition
    {
        public string ItemName;
    }

    [Serializable]
    public class StatsAfforded
    {
        public StatsType StatsType;
        public float Value;
        public StatsChangeMethod Method;
    }

    public enum StatsType
    {
        Health,
        Stress,
    }
}