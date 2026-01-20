using System;
using Cysharp.Threading.Tasks;
using jam.CodeBase.Core;
using UnityEngine;

namespace jam.CodeBase.Tasks
{
    [Serializable]
    public class CrashGlassBottleWithHead : BaseTask
    {
        public string ItemName;
        public override UniTask Execute()
        {
            Debug.LogError("CrashGlassBottleWithHead");
            G.Saves.Get<RunSaveModel>().Data.ObtainedItems.Remove(ItemName);
            return UniTask.CompletedTask;
        }
    }

    [Serializable]
    public class DoNotCrashGlassBottleWithHead : BaseTask
    {
        public override UniTask Execute()
        {
            Debug.LogError("DoNotCrashGlassBottleWithHead");
            return UniTask.CompletedTask;
        }
    }

}