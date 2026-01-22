using System;
using Cysharp.Threading.Tasks;
using jam.CodeBase.Character;
using jam.CodeBase.Core;

namespace jam.CodeBase.Tasks
{
    [Serializable]
    public class PlayRussianRoulette : BaseTask
    {
        public override async UniTask Execute()
        {
            var isAlive = UnityEngine.Random.Range(0, 60f) > 10;
            G.CharacterAnimator.PlayAnimation(AnimationType.RussianRullete);
            await UniTask.WaitForSeconds(2);

            if (isAlive)
            {
                G.Win();
            }
            else
            {
                G.Die();
            }
        }
    }

    [Serializable]
    public class NotPlayRussianRoulette : BaseTask
    {
        public override async UniTask Execute()
        {
            await UniTask.WaitForSeconds(2);
            G.Win();
        }
    }
}