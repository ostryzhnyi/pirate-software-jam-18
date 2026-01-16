using jam.CodeBase.Core;
using UnityEngine;

namespace jam.CodeBase.Audio
{
    public class CmsAudioController
    {
        public static AudioSource Play(CMSEntityPfb entityPfb)
        {
            if(entityPfb.AsEntity().Is<AudioSFXTag>(out var audioSfxTag))
            {
                return G.Audio.PlaySound2D(audioSfxTag.Sound, 
                    audioSfxTag.Volume + Random.Range(audioSfxTag.MinMaxVolume.x, audioSfxTag.MinMaxVolume.y),
                    0,
                    new AudioParams.Pitch(audioSfxTag.Pitch  + Random.Range(audioSfxTag.MinMaxPitch.x, audioSfxTag.MinMaxPitch.y))
                    ,looping: audioSfxTag.IsLoop);
            }
            else if(entityPfb.AsEntity().Is<AudioRandomSFXTag>(out var randAudioSfxTag))
            {
                var roundSound = randAudioSfxTag.Sounds[Random.Range(0, randAudioSfxTag.Sounds.Length)];
                
                return G.Audio.PlaySound2D(roundSound, 
                    randAudioSfxTag.Volume + Random.Range(randAudioSfxTag.MinMaxVolume.x, randAudioSfxTag.MinMaxVolume.y),
                    0,
                    new AudioParams.Pitch(randAudioSfxTag.Pitch  + Random.Range(randAudioSfxTag.MinMaxPitch.x, randAudioSfxTag.MinMaxPitch.y))
                    ,looping: randAudioSfxTag.IsLoop);
            }
            else if (entityPfb.AsEntity().Is<AudioMusicTag>(out var musicTag))
            {
                G.Audio.SetLoopAndPlay(musicTag.Category, musicTag.Volume);
            }

            return null;
        }

    }
}