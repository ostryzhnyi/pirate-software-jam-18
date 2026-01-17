using Cysharp.Threading.Tasks;
using UnityEngine;

namespace ProjectX.CodeBase.Utils
{
    public struct UniTaskHelper
    {
        public static async UniTask<bool> SmartWaitSeconds(float seconds)
        {
            var delayTask = UniTask.Delay((int)(seconds * 1000));
            var keyTask = UniTask.WaitUntil(() => Input.anyKeyDown);
            var completedTask = await UniTask.WhenAny(delayTask, keyTask);

            return completedTask == 0;
        }
        
    }
}