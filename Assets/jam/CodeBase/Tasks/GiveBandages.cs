using System;
using Cysharp.Threading.Tasks;
using jam.CodeBase.Character;
using jam.CodeBase.Core;
using UnityEngine;

namespace jam.CodeBase.Tasks
{
    [Serializable]
    public class GiveBandages : BaseTask
    {
        public override async UniTask Execute()
        {
            Debug.LogError("GiveBandages");
            G.BoxAnimator.PlayAnimation(BoxAnimationType.Bandage);
            
            await UniTask.WaitForSeconds(5.5f);
        }
    }

    [Serializable]
    public class DoNotGiveBandages : BaseTask
    {
        public override UniTask Execute()
        {
            Debug.LogError("DoNotGiveBandages");
            return UniTask.CompletedTask;
        }
    }
}