using UnityEngine;
using System.Collections;
using DG.Tweening;
using Sirenix.OdinInspector;

namespace jam.CodeBase.Room
{
    public class FanAnimator : MonoBehaviour
    {
        [Header("Fan Settings")]
        [SerializeField] private float accelerateTime = 2f;
        [SerializeField] private float fullSpeedRPS = 10f; 
        [SerializeField] private Transform fan; 
        [SerializeField] private ParticleSystem _particleSystem; 
        
        [Header("Speeds")]
        [SerializeField] private float lowSpeedRPS = 2f;
        [SerializeField] private float highSpeedRPS = 20f;
        
        private Tweener rotationTween;
        private Coroutine accelerationCoroutine;
        
        [Button]
        public void Play(bool fullSpeed)
        {
            Stop();
            
            float targetRPS = fullSpeed ? highSpeedRPS : lowSpeedRPS;
            float targetAngularSpeed = targetRPS * 360f; 
            
            rotationTween = fan.DOLocalRotate(new Vector3(0, 0, 360f), 1f / targetRPS, RotateMode.FastBeyond360)
                .SetRelative()
                .SetEase(Ease.Linear)
                .SetLoops(-1)
                .SetId("FanRotation");
            
            _particleSystem.gameObject.SetActive(fullSpeed);
            
            accelerationCoroutine = StartCoroutine(AccelerateToSpeed(targetRPS));
        }
        
        [Button]
        public void Stop()
        {
            if (rotationTween != null)
            {
                DOTween.Kill("FanRotation");
                rotationTween = null;
            }
            
            if (accelerationCoroutine != null)
            {
                StopCoroutine(accelerationCoroutine);
                accelerationCoroutine = null;
            }
            
            _particleSystem.gameObject.SetActive(false);
            
            fan.DOLocalRotate(Vector3.zero, 0.5f).SetEase(Ease.OutQuad);
        }
        
        private IEnumerator AccelerateToSpeed(float targetRPS)
        {
            float startRPS = 0f;
            float elapsed = 0f;
            
            while (elapsed < accelerateTime)
            {
                elapsed += Time.deltaTime;
                float progress = elapsed / accelerateTime;
                float currentRPS = Mathf.Lerp(startRPS, targetRPS, progress);
                
                if (rotationTween != null)
                {
                    float targetAngularSpeed = targetRPS * 360f;
                    float currentAngularSpeed = currentRPS * 360f;
                    rotationTween.timeScale = currentAngularSpeed / targetAngularSpeed;
                }
                
                yield return null;
            }
        }
        
        private void OnDestroy()
        {
            Stop();
        }
    }
}
