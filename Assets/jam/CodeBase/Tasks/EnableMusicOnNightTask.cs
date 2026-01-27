using System;
using Cysharp.Threading.Tasks;
using jam.CodeBase.Core;
using UnityEngine;

namespace jam.CodeBase.Tasks
{
    [Serializable]
    public class EnableMusicOnNightTask: BaseTask
    {
        public override UniTask Execute()
        {
            Debug.LogError("EnableMusicOnNightTask");
            foreach (var music in G.Room.Music)
            {
                music.Play();
            }
            return UniTask.CompletedTask;
        }
    }
    
    [Serializable]
    public class NotEnableMusicOnNightTask: BaseTask
    {
        public override UniTask Execute()
        {
            Debug.LogError("NotEnableMusicOnNightTask");
            return UniTask.CompletedTask;
        }
    }
}