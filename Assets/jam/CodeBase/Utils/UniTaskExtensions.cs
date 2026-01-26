using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace jam.CodeBase.Utils
{
    public struct UniTaskHelper
    {
        public static async UniTask<bool> SmartWaitSeconds(
            float seconds,
            CancellationToken cancellationToken = default)
        {
            var delayTask = UniTask.Delay(
                (int)(seconds * 1000),
                cancellationToken: cancellationToken);

            var keyTask = UniTask.WaitUntil(
                () => Input.anyKeyDown,
                cancellationToken: cancellationToken);

            var completedTask = await UniTask.WhenAny(delayTask, keyTask);

            if (cancellationToken.IsCancellationRequested)
                return false;

            return completedTask == 0;
        }
    }
}