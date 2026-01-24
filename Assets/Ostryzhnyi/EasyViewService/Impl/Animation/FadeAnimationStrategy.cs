using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Ostryzhnyi.EasyViewService.Api.Animation;
using UnityEngine;

namespace Ostryzhnyi.EasyViewService.Impl.Animation
{
    public class FadeStrategy: BaseAnimationStrategy
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private float _fadeTime = .5f;
        
        public override async UniTask Show()
        {
            gameObject.SetActive(true);

            if(_canvasGroup == null)
                _canvasGroup = gameObject.AddComponent<CanvasGroup>();

            _canvasGroup.alpha = 0;
            await _canvasGroup.DOFade(1, _fadeTime)
                .SetLink(gameObject, LinkBehaviour.KillOnDestroy)
                .WithCancellation(this.GetCancellationTokenOnDestroy());
        }

        public override async UniTask Hide()
        {
            var token = this.GetCancellationTokenOnDestroy();
    
            if(_canvasGroup == null)
                _canvasGroup = gameObject.AddComponent<CanvasGroup>();
            
            _canvasGroup.alpha = 1;
            
            await _canvasGroup.DOFade(0, _fadeTime)
                .SetLink(gameObject, LinkBehaviour.KillOnDestroy)
                .WithCancellation(token);
            
            if(gameObject != null && gameObject.activeSelf)
                gameObject.SetActive(false);
        }
    }
}