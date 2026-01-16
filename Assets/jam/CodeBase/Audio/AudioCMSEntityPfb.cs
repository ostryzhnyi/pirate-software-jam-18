using Sirenix.OdinInspector;
using UnityEngine;

namespace jam.CodeBase.Audio
{
    public class AudioCMSEntityPfb : CMSEntityPfb
    {
        [Button]
        public void DebugPlay()
        {
#if UNITY_EDITOR
            if(AsEntity().Is<AudioSFXTag>(out var sfx))
            {
                if (!Application.isPlaying)
                {
                    Debug.LogError("Please start play mode");
                    return;
                }

                CmsAudioController.Play(this);
            }
#endif
        }
    }
}