using System;
using Cysharp.Threading.Tasks;
using jam.CodeBase.Core;
using Ostryzhnyi.EasyViewService.Api.Service;
using Ostryzhnyi.EasyViewService.ViewLayers;
using UnityEngine;
using DG.Tweening;
using jam.CodeBase.Utils;
using TMPro;

namespace jam.CodeBase.Bets
{
    public class BetPopup : BaseView
    {
        public override ViewLayers Layer => ViewLayers.Popup;

        [SerializeField] private Transform _weight;
        [SerializeField] private Transform _aliveCardParent;
        [SerializeField] private Transform _dieCardParent;
        [SerializeField] private TMP_Text _dieKoef;
        [SerializeField] private TMP_Text _aliveKoef;
        [SerializeField] private TMP_Text _aliveSum;
        [SerializeField] private TMP_Text _dieSum;
        [SerializeField] private float _maxRotate = 15f;
        [SerializeField] private float _rotateDuration = 0.25f;
        [SerializeField] private Ease _ease = Ease.OutQuad;

        private TMP_Text AliveKoef
        {
            set
            {
                Debug.LogError("UPDATE:"+value);
                _aliveKoef = value;
            }

            get => _aliveKoef;
        }

        private Tween _weightTween;
        private Tween _aliveTween;
        private Tween _dieTween;

        private float _aliveSumValue = 0;
        private float _dieValue = 0;
        
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
            Redraw().Forget();
        }

        private async UniTask Redraw()
        {
            await UniTask.SwitchToMainThread();
            Debug.LogError(AliveKoef);
            Debug.LogError(_rotateDuration);
            AliveKoef.DOFloatNumber(G.BetController.AliveBetCoefficient, _rotateDuration, "{0:0.00}", .01f);
            _dieKoef.DOFloatNumber(G.BetController.DieBetCoefficient, _rotateDuration, "{0:0.00}", .01f);
            
            _aliveSum.DOFloatNumber(_aliveSumValue, G.BetController.AliveBet, _rotateDuration, "${0:0}", 10);
            _dieSum.DOFloatNumber(_dieValue, G.BetController.DieBet, _rotateDuration, "${0:0}", 10);
            
            // AliveKoef.SetText("{0:0.00}", G.BetController.AliveBetCoefficient);
            // _dieKoef.SetText("{0:0.00}", G.BetController.DieBetCoefficient);
            //
            // _aliveSum.SetText( "${0:0}", G.BetController.AliveBet);
            // _dieSum.SetText( "${0:0}", G.BetController.DieBet);

            _aliveSumValue = G.BetController.AliveBet;
            _dieValue = G.BetController.DieBet;
            
            float aliveK = G.BetController.AliveBetCoefficient;
            float dieK   = G.BetController.DieBetCoefficient;

            float diff = aliveK - dieK;                
            float angle = diff * _maxRotate;         

            _weightTween?.Kill();
            _aliveTween?.Kill();
            _dieTween?.Kill();

            _weightTween = _weight
                .DOLocalRotate(new Vector3(0f, 0f, -angle), _rotateDuration)
                .SetEase(_ease);

            _aliveTween = _aliveCardParent
                .DOLocalRotate(new Vector3(0f, 0f, angle), _rotateDuration)
                .SetEase(_ease);

            _dieTween = _dieCardParent
                .DOLocalRotate(new Vector3(0f, 0f, angle), _rotateDuration)
                .SetEase(_ease);
        }
    }
}
