using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace jam.CodeBase.Audio
{
    [Serializable]
    public class AudioSFXTag: EntityComponentDefinition
    {
        public string Sound;
        public bool IsLoop;
        [MinMaxSlider(0, 1)] public float Volume = 0.5f;
        [MinMaxSlider(-3, 3)] public float Pitch = 1f;
        [MinMaxSlider(-1, 1)] public Vector2 MinMaxVolume = new Vector2(-0.1f, 0.1f);
        [MinMaxSlider(-3, 3)] public Vector2 MinMaxPitch = new Vector2(-0.1f, 0.1f);
    }
    
    [Serializable]
    public class AudioRandomSFXTag: EntityComponentDefinition
    {
        public string[] Sounds;
        public bool IsLoop;
        [MinMaxSlider(0, 1)] public float Volume = 0.5f;
        [MinMaxSlider(-3, 3)] public float Pitch = 1f;
        [MinMaxSlider(-1, 1)] public Vector2 MinMaxVolume = new Vector2(-0.1f, 0.1f);
        [MinMaxSlider(-3, 3)] public Vector2 MinMaxPitch = new Vector2(-0.1f, 0.1f);
    }
    
    [Serializable]
    public class AudioMusicTag: EntityComponentDefinition
    {
        public string Category;
        
        [MinMaxSlider(0, 1)] public float Volume = 0.5f;
        [MinMaxSlider(-3, 3)] public float Pitch = 1f;
        [MinMaxSlider(-1, 1)] public Vector2 MinMaxVolume = new Vector2(-0.1f, 0.1f);
        [MinMaxSlider(-3, 3)] public Vector2 MinMaxPitch = new Vector2(-0.1f, 0.1f);
    }
    
}