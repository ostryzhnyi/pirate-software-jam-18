using UnityEngine;
using DG.Tweening;

namespace jam.CodeBase.Room
{
    public class FanAnimator : MonoBehaviour
    {
        public enum FanState
        {
            Passive,
            VerySlow,
            Fast,
            VeryFast
        }

        [SerializeField] private Transform fan;
        [Header("Speeds (RPM)")]
        [SerializeField] float verySlowRpm = 30f;
        [SerializeField] float fastRpm = 400f;
        [SerializeField] float veryFastRpm = 800f;

        [Header("Acceleration")]
        [SerializeField] float changeSpeedDuration = 1f;
        [SerializeField] Ease changeSpeedEase = Ease.OutCubic;
        [SerializeField] private ParticleSystem _particleSystem;

        FanState _state = FanState.VerySlow;
        Tweener _rotationTween;
        Tweener _speedTween;
        float _currentRpm;

        void Awake()
        {
            _currentRpm = verySlowRpm;
            SetState(FanState.VerySlow);
            StartRotationTween();
        }

        public void Play(bool fullSpeed)
        {
            if (fullSpeed)
                SetState(FanState.VeryFast);
            else
                SetState(FanState.Fast);
        }

        public void Stop()
        {
            SetState(FanState.VerySlow);
            _particleSystem.gameObject.SetActive(false);
        }

        void SetState(FanState newState)
        {
            _state = newState;

            float targetRpm = _state switch
            {
                FanState.Passive   => 0f,
                FanState.VerySlow  => verySlowRpm,
                FanState.Fast      => fastRpm,
                FanState.VeryFast  => veryFastRpm,
                _ => verySlowRpm
            };

            _particleSystem.gameObject.SetActive(newState == FanState.VeryFast);
            
            StartSpeedTween(targetRpm);
        }

        void StartRotationTween()
        {
            _rotationTween?.Kill();

            _rotationTween = fan
                .DORotate(new Vector3(0f, 0f, 360f), 1f, RotateMode.FastBeyond360)
                .SetRelative(true)
                .SetEase(Ease.Linear)
                .SetLoops(-1)
                .SetSpeedBased();            
        }

        void StartSpeedTween(float targetRpm)
        {
            _speedTween?.Kill();

            _speedTween = DOTween
                .To(() => _currentRpm, v => _currentRpm = v, targetRpm, changeSpeedDuration)
                .SetEase(changeSpeedEase)
                .OnUpdate(UpdateRotationSpeed);    
        }

        void UpdateRotationSpeed()
        {
            if (_rotationTween == null) return;

            float degPerSec = _currentRpm / 60f * 360f;
            _rotationTween.timeScale = degPerSec;   
        }

        void OnDestroy()
        {
            _rotationTween?.Kill();
            _speedTween?.Kill();
        }
    }
}
