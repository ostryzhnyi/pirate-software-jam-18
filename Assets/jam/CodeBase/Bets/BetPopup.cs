using System;
using Cysharp.Threading.Tasks;
using jam.CodeBase.Core;
using Ostryzhnyi.EasyViewService.Api.Service;
using Ostryzhnyi.EasyViewService.ViewLayers;
using UnityEngine;
using DG.Tweening;
using jam.CodeBase.UI;
using jam.CodeBase.Utils;
using TMPro;
using UnityEngine.UI;

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
        [SerializeField] private BetView _aliveBetView;
        [SerializeField] private BetView _dieBetView;
        [SerializeField] private Button _bet;
        [SerializeField] private TMP_Text _totalBet;
        [SerializeField] private SequenceAnimation _timerAnimation;
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
            
            _aliveBetView.SetBit(0);
            _dieBetView.SetBit(0);
            
            _aliveBetView.OnBitChange += OnAliveBitChange;
            _dieBetView.OnBitChange += OnDieBitChange;
            _bet.onClick.AddListener(OnBet);
            
            _timerAnimation.Play().Forget();

            _bet.interactable = false;
        }

        private void OnDisable()
        {
            G.BetController.OnChangeAliveCoefficient -= Redraw;
            G.BetController.OnChangeDieCoefficient -= Redraw;
            
            _aliveBetView.OnBitChange -= OnAliveBitChange;
            _dieBetView.OnBitChange -= OnDieBitChange;
            _bet.onClick.RemoveListener(OnBet);
            
            _weightTween?.Kill();
            _aliveTween?.Kill();
            _dieTween?.Kill();
            _timerAnimation.Stop();
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
            
            _totalBet.DOFloatNumber(G.BetController.CurrentBet, _rotateDuration, "${0:0}", 10);
        }
        
        private void OnDieBitChange(float bit)
        {
            _aliveBetView.SetBit(0, true);
            
            _bet.interactable = Math.Max(_aliveBetView.Bit, _dieBetView.Bit) > 0;
        }

        private void OnAliveBitChange(float bit)
        {
            _dieBetView.SetBit(0, true);

            _bet.interactable = Math.Max(_aliveBetView.Bit, _dieBetView.Bit) > 0;
        }

        private void OnBet()
        {
            if(_aliveBetView.Bit > 0)
            {
                G.BetController.BetToAlive(_aliveBetView.Bit);
                G.Economy.SpendMoney(_aliveBetView.Bit);
                _aliveBetView.SetBit(0);
            }
            
            if(_dieBetView.Bit > 0)
            {
                G.BetController.BetToDie(_dieBetView.Bit);
                G.Economy.SpendMoney(_dieBetView.Bit);
                _dieBetView.SetBit(0);
            }
            
            _dieBetView.UpdateButtons();
            _aliveBetView.UpdateButtons();
        }
    }
}
