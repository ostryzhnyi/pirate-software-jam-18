using System;
using Cysharp.Threading.Tasks;
using jam.CodeBase.Character;
using jam.CodeBase.Core;

namespace jam.CodeBase.Tasks
{
    [Serializable]
    public class MusicOnNightTask : BaseTask
    {
        public override async UniTask Execute()
        {
            foreach (var music in G.Room.Music)
            {
                music.Play();
            }
            
            G.Room.TVAnimator.Play(TVAnimation.MusicTime, 3f);
            
            G.CharacterAnimator.PlayAnimation(AnimationType.SetSad);

            await UniTask.WaitForSeconds(3);
        }
    }
    
    [Serializable]
    public class NotMusicOnNightTask : BaseTask
    {
        public override UniTask Execute()
        {
            return UniTask.CompletedTask;
            
        }
    }
}