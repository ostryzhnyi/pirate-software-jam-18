using System;
using Cysharp.Threading.Tasks;
using jam.CodeBase.Core;
using UnityEngine;

namespace jam.CodeBase.Tasks
{
    [Serializable]
    public class GiftSodaGlassBottle : BaseTask
    {
        public string ItemName;

        public override UniTask Execute()
        {
            Debug.LogError("GiftSodaGlassBottle");
            G.Saves.Get<RunSaveModel>().Data.ObtainedItems.Add(ItemName);
            return UniTask.CompletedTask;
        }
    }


    [Serializable]
    public class DoNotGiftSodaGlassBottle : BaseTask
    {
        public override UniTask Execute()
        {
            Debug.LogError("DoNotGiftSodaGlassBottle");
            return UniTask.CompletedTask;
        }
    }
}