using System;
using Cysharp.Threading.Tasks;
using jam.CodeBase.Core;
using UnityEngine;

namespace jam.CodeBase.Tasks
{
    [Serializable]
    public class GiftSleepPills : BaseTask
    {
        public string ItemName;
        public override UniTask Execute()
        {
            Debug.LogError("GiftSleepPills");
            G.Saves.Get<RunSaveModel>().Data.ObtainedItems.Add(ItemName);
            return UniTask.CompletedTask;
        }
    }

    [Serializable]
    public class DoNotGiftSleepPills : BaseTask
    {
        public override UniTask Execute()
        {
            Debug.LogError("DoNotGiftSleepPills");
            return UniTask.CompletedTask;
        }
    }
}