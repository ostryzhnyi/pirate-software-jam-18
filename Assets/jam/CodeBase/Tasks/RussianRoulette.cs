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
            G.Room.TVAnimator.Play(TVAnimation.ShootTime, 6f);
            await UniTask.WaitForSeconds(1);

            var isAlive = UnityEngine.Random.Range(0, 60f) > 10;
            G.CharacterAnimator.PlayAnimation(AnimationType.RussianRullete);
            await UniTask.WaitForSeconds(2);

         
            if (isAlive)
            {
                var alives = G.Interactors.GetAll<IAliveCharacter>();
                foreach (var alive in alives)
                {
                    await alive.OnAlive(G.Characters.CurrentCharacter);
                }
            }
            else
            {
                G.Characters.CurrentCharacter.ChangeHP(int.MaxValue, StatsChangeMethod.Remove).Forget();
            }
        }
    }

    [Serializable]
    public class NotPlayRussianRoulette : BaseTask
    {
        public override async UniTask Execute()
        {
            await UniTask.WaitForSeconds(2);
            var alives = G.Interactors.GetAll<IAliveCharacter>();
            foreach (var alive in alives)
            {
                await alive.OnAlive(G.Characters.CurrentCharacter);
            }
        }
    }
}