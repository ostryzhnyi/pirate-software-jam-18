using System;
using jam.CodeBase.Core;
using Ostryzhnyi.EasyViewService.Api.Service;
using Ostryzhnyi.EasyViewService.ViewLayers;
using UnityEngine;
using DG.Tweening;

namespace jam.CodeBase.Bets
{
    public class BetPopup : BaseView
    {
        public override ViewLayers Layer => ViewLayers.Popup;

        [SerializeField] private Transform _weight;
        [SerializeField] private Transform _aliveCardParent;
        [SerializeField] private Transform _dieCardParent;
        [SerializeField] private float _maxRotate = 15f;
        [SerializeField] private float _rotateDuration = 0.25f;
        [SerializeField] private Ease _ease = Ease.OutQuad;

        private Tween _weightTween;
        private Tween _aliveTween;
        private Tween _dieTween;

        private void OnEnable()
        {
            G.BetController.OnChangeAliveCoefficient += Redraw;
            G.BetController.OnChangeDieCoefficient += Redraw;
        }

        private void OnDisable()
        {
            G.BetController.OnChangeAliveCoefficient -= Redraw;
            G.BetController.OnChangeDieCoefficient -= Redraw;

            _weightTween?.Kill();
            _aliveTween?.Kill();
            _dieTween?.Kill();
        }

        protected override void Showed(ViewOption option = null)
        {
            base.Showed(option);
            Redraw(0f);
        }

        private void Redraw(float _)
        {
            float aliveK = G.BetController.AliveBetCoefficient;
            float dieK   = G.BetController.DieBetCoefficient;

            float diff = aliveK - dieK;                
            float angle = diff * _maxRotate;         

            _weightTween?.Kill();
            _aliveTween?.Kill();
            _dieTween?.Kill();

            _weightTween = _weight
                .DOLocalRotate(new Vector3(0f, 0f, angle), _rotateDuration)
                .SetEase(_ease);

            _aliveTween = _aliveCardParent
                .DOLocalRotate(new Vector3(0f, 0f, -angle), _rotateDuration)
                .SetEase(_ease);

            _dieTween = _dieCardParent
                .DOLocalRotate(new Vector3(0f, 0f, -angle), _rotateDuration)
                .SetEase(_ease);
        }
    }
}
