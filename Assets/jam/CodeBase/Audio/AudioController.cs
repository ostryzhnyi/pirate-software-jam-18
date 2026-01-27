using System.Collections.Generic;
using DG.Tweening;
using jam.CodeBase.Core;
using UnityEngine;
using UnityEngine.Audio;

namespace jam.CodeBase.Audio
{
    public class AudioController : MonoBehaviour
    {
        public const float GBC_INTERIOR_BGM_LOWEREDVOLUME = 0.35f;
        public const float GBC_INTERIOR_BGM_FULLVOLUME    = 0.55f;

        [Header("Mixers")]
        [SerializeField] private AudioMixer       masterMixer;
        [SerializeField] private AudioMixerGroup  sfxMixerGroup;
        [SerializeField] private AudioMixerGroup  musicMixerGroup;

        [Header("Loop settings")]
        [SerializeField] private float defaultCrossFadeDuration = 1.0f;
        
        private readonly Dictionary<AudioSource, Coroutine> repeatingSFX = new Dictionary<AudioSource, Coroutine>();

        private readonly List<AudioSource> loopSources = new List<AudioSource>();
        private List<AudioSource> Loops => loopSources;

        public AudioSource BaseLoopSource => loopSources[0];

        private List<AudioClip> sfx   = new List<AudioClip>();
        private List<AudioClip> loops = new List<AudioClip>();

        private List<AudioSource> activeSFX = new List<AudioSource>();
        private List<AudioSource> ActiveSFXSources
        {
            get
            {
                activeSFX.RemoveAll(x => x == null);
                return activeSFX;
            }
        }

        public bool Fading { get; private set; }
        public float GlobalVolume
        {
            get => _globalVolume;
            set
            {
                _globalVolume = value;
                SetMusicVolume(_musicVolume);
                SetSFXVolume(_sfxVolume);
            }
        }
        public float MusicVolume => _musicVolume;
        public float SoundVolume => _sfxVolume;

        private Dictionary<string, float> limitedFrequencySounds = new Dictionary<string, float>();
        private Dictionary<string, int>   lastPlayedSounds       = new Dictionary<string, int>(); 
        private Dictionary<string, int>   lastPlayedLoops        = new Dictionary<string, int>(); 

        private float _sfxVolume  = 1f;
        private float _musicVolume = 1f;
        private float _globalVolume = 1f;

        private const string SOUNDID_REPEAT_DELIMITER = "#";
        private const float  DEFAULT_SPATIAL_BLEND    = 0.75f;

        private readonly int[] DEFAULT_LOOPSOURCE_INDICES = new int[] { 0 };

        private bool sfxMuted  = false;
        private bool loopMuted = false;

        private int  _currentLoopSourceIndex = 0;
        private int  _nextLoopSourceIndex    => 1 - _currentLoopSourceIndex;
        private string _currentLoopCategory  = null;

        void Awake()
        {
            G.Audio = this;

            transform.parent = null;
            DontDestroyOnLoad(gameObject);

            foreach (object o in Resources.LoadAll("Audio/SFX"))
                sfx.Add((AudioClip)o);

            foreach (object o in Resources.LoadAll("Audio/Loops"))
                loops.Add((AudioClip)o);

            CreateLoopSource();
            CreateLoopSource();

            foreach (var ls in loopSources)
            {
                ls.outputAudioMixerGroup = musicMixerGroup;
                ls.loop = true;
            }

            SetSFXVolume(.5f);
            SetMusicVolume(.5f);
        }

        private void CreateLoopSource()
        {
            var go = new GameObject("MusicLoopSource_" + loopSources.Count);
            go.transform.SetParent(transform);
            var src = go.AddComponent<AudioSource>();
            src.playOnAwake = false;
            src.spatialBlend = 0f; 
            src.loop = true;
            loopSources.Add(src);
        }

        #region Volume / Mute

        public void SetSFXVolume(float sfxVolume)
        {
            _sfxVolume = sfxVolume;
          
            if (sfxMixerGroup != null && masterMixer != null)
                masterMixer.SetFloat("SFXVolume", LinearToMixerDb(sfxVolume  * _globalVolume));
        }

        public void SetMusicVolume(float musicVolume)
        {
            _musicVolume = musicVolume;
          
            if (musicMixerGroup != null && masterMixer != null)
                    masterMixer.SetFloat("MusicVolume", LinearToMixerDb(musicVolume * _globalVolume));
        }

        private float LinearToMixerDb(float v)
        {
            if (v <= 0.0001f) return -80f;
            return Mathf.Log10(v) * 20f;
        }

        public void UnmuteLoop()
        {
            if (!loopMuted) return;
            loopMuted = false;
            foreach (var loopSource in loopSources)
                loopSource.mute = false;
        }

        #endregion

        #region SFX

        private AudioMixerGroup currentSFXMixer => sfxMixerGroup;

        public AudioSource PlaySound2D(string soundId, float? volume = null, float skipToTime = 0f,
            AudioParams.Pitch pitch = null,
            AudioParams.Repetition repetition = null,
            AudioParams.Randomization randomization = null,
            AudioParams.Distortion distortion = null,
            bool looping = false)
        {
            var source = PlaySound3D(soundId, Vector3.zero, volume, skipToTime, pitch, repetition, randomization, distortion, looping);
            if (source != null)
            {
                source.spatialBlend = 0f;
                DontDestroyOnLoad(source.gameObject);
            }
            return source;
        }

        public AudioSource PlaySound3D(string soundId, Vector3 position, float? volume = 1f, float skipToTime = 0f,
            AudioParams.Pitch pitch = null,
            AudioParams.Repetition repetition = null,
            AudioParams.Randomization randomization = null,
            AudioParams.Distortion distortion = null,
            bool looping = false)
        {
            if (sfxMuted)
                return null;

            var targetVolume = volume ?? _sfxVolume;
            targetVolume *= _globalVolume;
            
            if (repetition != null)
            {
                if (RepetitionIsTooFrequent(soundId, repetition.minRepetitionFrequency, repetition.entryId))
                    return null;
            }

            string randomVariationId = soundId;
            if (randomization != null)
                randomVariationId = GetRandomVariationOfSound(soundId, randomization.noRepeating);

            var source = CreateAudioSourceForSound(randomVariationId, position, looping);
            if (source != null)
            {
                source.volume = targetVolume;
                source.time = source.clip.length * skipToTime;
                if (pitch != null) source.pitch = pitch.pitch;
                if (distortion != null && distortion.muffled) MuffleSource(source);
            }

            activeSFX.Add(source);
            return source;
        }
        

        private AudioSource CreateAudioSourceForSound(string soundId, Vector3 position, bool looping)
        {
            if (string.IsNullOrEmpty(soundId)) return null;
            AudioClip sound = GetAudioClip(soundId);
            if (sound == null) return null;
            return InstantiateAudioObject(sound, position, looping);
        }

        private AudioSource InstantiateAudioObject(AudioClip clip, Vector3 pos, bool looping)
        {
            GameObject tempGO = new GameObject("Audio_" + clip.name);
            tempGO.transform.position = pos;

            AudioSource aSource = tempGO.AddComponent<AudioSource>();
            aSource.clip = clip;
            aSource.outputAudioMixerGroup = currentSFXMixer;
            aSource.spatialBlend = DEFAULT_SPATIAL_BLEND;

            aSource.Play();
            if (looping)
            {
                aSource.loop = true;
            }
            else
            {
                Destroy(tempGO, clip.length * 3f);
            }
            return aSource;
        }

        #endregion

        #region Utils

        public AudioClip GetLoopClip(string loopId)
            => loops.Find(x => x.name.ToLowerInvariant() == loopId.ToLowerInvariant());

        public AudioClip GetAudioClip(string soundId)
            => sfx.Find(x => x.name.ToLowerInvariant() == soundId.ToLowerInvariant());

        private bool RepetitionIsTooFrequent(string soundId, float frequencyMin, string entrySuffix = "")
        {
            float time = Time.unscaledTime;
            string soundKey = soundId + entrySuffix;

            if (limitedFrequencySounds.TryGetValue(soundKey, out var lastTime))
            {
                if (time - frequencyMin > lastTime)
                {
                    limitedFrequencySounds[soundKey] = time;
                    return false;
                }
            }
            else
            {
                limitedFrequencySounds.Add(soundKey, time);
                return false;
            }
            return true;
        }

        private string GetRandomVariationOfSound(string soundPrefix, bool noRepeating)
        {
            string soundId = "";

            if (!string.IsNullOrEmpty(soundPrefix))
            {
                List<AudioClip> variations = sfx.FindAll(x => x != null &&
                                                              x.name.ToLowerInvariant()
                                                                  .StartsWith(soundPrefix.ToLowerInvariant() + SOUNDID_REPEAT_DELIMITER));
                if (variations.Count > 0)
                {
                    int index = Random.Range(0, variations.Count) + 1;
                    if (noRepeating)
                    {
                        if (!lastPlayedSounds.ContainsKey(soundPrefix))
                        {
                            lastPlayedSounds.Add(soundPrefix, index);
                        }
                        else
                        {
                            int breakOutCounter = 0;
                            const int BREAK_OUT_THRESHOLD = 100;
                            while (lastPlayedSounds[soundPrefix] == index && breakOutCounter < BREAK_OUT_THRESHOLD)
                            {
                                index = Random.Range(0, variations.Count) + 1;
                                breakOutCounter++;
                            }
                            lastPlayedSounds[soundPrefix] = index;
                        }
                    }
                    soundId = soundPrefix + SOUNDID_REPEAT_DELIMITER + index;
                }
                else
                {
                    soundId = soundPrefix;
                }
            }

            return soundId;
        }

        private void MuffleSource(AudioSource source, float cutoff = 300f)
        {
            var filter = source.gameObject.AddComponent<AudioLowPassFilter>();
            filter.cutoffFrequency = cutoff;
        }

        private void UnMuffleSource(AudioSource source)
        {
            var lowPassFilter = source.GetComponent<AudioLowPassFilter>();
            if (lowPassFilter != null)
                Destroy(lowPassFilter);
        }

        #endregion

        #region Loops (Music)

        public void StopAllLoops()
        {
            CancelFades();
            foreach (AudioSource loopSource in loopSources)
                loopSource.Stop();
            _currentLoopCategory = null;
        }

        public void StopLoop(int sourceIndex = 0)
        {
            if (sourceIndex < 0 || sourceIndex >= loopSources.Count) return;
            loopSources[sourceIndex].Stop();
        }

        public void SetLoopAndPlay(string categoryPrefix, float targetVolume = .5f, float crossFadeDuration = -1f)
        {
            if (crossFadeDuration < 0f)
                crossFadeDuration = defaultCrossFadeDuration;

            bool nothingPlaying = string.IsNullOrEmpty(_currentLoopCategory) ||
                                  !loopSources[_currentLoopSourceIndex].isPlaying ||
                                  loopSources[_currentLoopSourceIndex].clip == null;

            int newClipIndex;
            var newClip = GetRandomLoopForCategory(categoryPrefix, out newClipIndex);
            if (newClip == null)
            {
                Debug.LogWarning($"No loop clip found for category {categoryPrefix}");
                return;
            }

            if (nothingPlaying)
            {
                var src = loopSources[_currentLoopSourceIndex];
                src.clip = newClip;
                src.time = 0f;
                src.volume = targetVolume * _globalVolume;
                src.loop = true;
                src.Play();

                _currentLoopCategory = categoryPrefix;
                lastPlayedLoops[categoryPrefix] = newClipIndex;
                return;
            }

            int nextIndex = _nextLoopSourceIndex;
            var nextSrc = loopSources[nextIndex];
            nextSrc.clip = newClip;
            nextSrc.time = 0f;
            nextSrc.volume = 0f;
            nextSrc.loop = true;
            nextSrc.Play();

            CancelFades();

            Fading = true;

            var currentSrc = loopSources[_currentLoopSourceIndex];

            currentSrc.DOFade(0f, crossFadeDuration).SetEase(Ease.Linear);
            nextSrc.DOFade(targetVolume, crossFadeDuration).SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    currentSrc.Stop();
                    currentSrc.volume = 0f;
                    _currentLoopSourceIndex = nextIndex;
                    _currentLoopCategory = categoryPrefix;
                    lastPlayedLoops[categoryPrefix] = newClipIndex;
                    Fading = false;
                });
        }

        private AudioClip GetRandomLoopForCategory(string categoryPrefix, out int chosenIndex)
        {
            chosenIndex = -1;

            var candidates = new List<AudioClip>();
            for (int i = 0; i < loops.Count; i++)
            {
                var clip = loops[i];
                if (clip == null) continue;
                if (clip.name.Contains(categoryPrefix))
                    candidates.Add(clip);
            }

            if (candidates.Count == 0)
                return null;

            int newIndex = Random.Range(0, candidates.Count);

            if (candidates.Count > 1)
            {
                if (lastPlayedLoops.TryGetValue(categoryPrefix, out var lastIndex))
                {
                    int safety = 0;
                    while (newIndex == lastIndex && safety < 50)
                    {
                        newIndex = Random.Range(0, candidates.Count);
                        safety++;
                    }
                }
            }

            chosenIndex = newIndex;
            return candidates[newIndex];
        }

        public void SetLoopTimeNormalized(float normalizedTime)
        {
            var src = loopSources[_currentLoopSourceIndex];
            if (src.clip == null) return;
            src.time = Mathf.Clamp(normalizedTime * src.clip.length, 0f, src.clip.length - 0.1f);
        }

        private void CancelFades()
        {
            StopAllCoroutines();
            foreach (AudioSource loopSource in loopSources)
                loopSource.DOKill();
            Fading = false;
        }

        #endregion
    }
}
