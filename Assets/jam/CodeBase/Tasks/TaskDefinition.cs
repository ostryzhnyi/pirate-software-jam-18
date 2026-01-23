using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using jam.CodeBase.Character;
using UnityEngine;

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
        public TaskTarget TaskTarget; 

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
        public Vector2 ValueRange = new Vector2(5, 15);
        public StatsChangeMethod Method;
    }

    public enum StatsType
    {
        Health,
        Stress,
    }
    public enum TaskTarget
    {
        Die,
        Live,
    }
}