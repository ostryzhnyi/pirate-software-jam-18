using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace jam.CodeBase.Tasks
{
    [Serializable]
    public class DrinkAllSleepPills : BaseTask
    {
        public override UniTask Execute()
        {
            Debug.LogError("DrinkAllSleepPills");
            return UniTask.CompletedTask;
        }
    }

    [Serializable]
    public class DoNotDrinkAllSleepPills : BaseTask
    {
        public override UniTask Execute()
        {
            Debug.LogError("DoNotDrinkAllSleepPills");
            return UniTask.CompletedTask;
        }
    }
}