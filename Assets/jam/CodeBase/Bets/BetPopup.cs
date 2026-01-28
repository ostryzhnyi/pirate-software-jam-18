using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using jam.CodeBase.Core;
using Ostryzhnyi.EasyViewService.Api.Service;
using Ostryzhnyi.EasyViewService.ViewLayers;
using UnityEngine;
using DG.Tweening;
using jam.CodeBase.Economy;
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
        [SerializeField] private TMP_Text _ftueText;
        [SerializeField] private BetView _aliveBetView;
        [SerializeField] private BetView _dieBetView;
        [SerializeField] private Button _bet;
        [SerializeField] private TMP_Text _totalBet;
        [SerializeField] private SequenceAnimation _timerAnimation;
        [SerializeField] private float _maxRotate = 15f;
        [SerializeField] private float _rotateDuration = 0.25f;
        [SerializeField] private Ease _ease = Ease.OutQuad;
        [SerializeField] private GameObject _tutorialPointerPlusMin;
        [SerializeField] private GameObject _tutorialPointerBet;

        private CancellationTokenSource _playFirstFTUEcancellationTokenSource;
        private CancellationTokenSource _playSecondFTUEcancellationTokenSource;

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
        private FTUESaveModel _ftueSaveModel;
        
        private void OnEnable()
        {
            _ftueSaveModel = G.Saves.Get<FTUESaveModel>();
            _ftueText.SetText("");
            G.BetController.OnChangeAliveCoefficient += Redraw;
            G.BetController.OnChangeDieCoefficient += Redraw;
            G.BetController.StartFtueAwait += OnStartFtuteAwait;
            G.BetController.StopFtueAwait += OnStopFtuteAwait;
            
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
            G.BetController.StartFtueAwait -= OnStartFtuteAwait;
            G.BetController.StopFtueAwait -= OnStopFtuteAwait;
            
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
            if(!_ftueSaveModel.Data.ShowedBetFTUE)
                PlayFirstFTUE().Forget();
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
            
            float aliveK = G.BetController.DieBetCoefficient;
            float dieK   = G.BetController.AliveBetCoefficient;

            var cfg = GameResources.CMS.BaseEconomy.As<BaseEconomyTag>();

            float diff = dieK / cfg.CoefficientToDieMultiplier - aliveK / cfg.CoefficientToAliveMultiplier;                
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
            
            _totalBet.DOFloatNumber(G.BetController.CurrentBet, _rotateDuration, "{0:0}", 10);
        }
        
        private void OnDieBitChange(float bit)
        {
            _aliveBetView.SetBit(0, true);
            
            _bet.interactable = Math.Max(_aliveBetView.Bit, _dieBetView.Bit) > 0;
            
            _tutorialPointerPlusMin.gameObject.SetActive(false);
            _playFirstFTUEcancellationTokenSource?.Cancel();
            
            if (_playSecondFTUEcancellationTokenSource == null && !_ftueSaveModel.Data.ShowedBetFTUE)
            {
                PlaySecondFTUE().Forget();
            }
        }

        private void OnAliveBitChange(float bit)
        {
            _dieBetView.SetBit(0, true);

            _bet.interactable = Math.Max(_aliveBetView.Bit, _dieBetView.Bit) > 0;
            _tutorialPointerPlusMin.gameObject.SetActive(false);
            _playFirstFTUEcancellationTokenSource?.Cancel();

            if (_playSecondFTUEcancellationTokenSource == null && !_ftueSaveModel.Data.ShowedBetFTUE)
            {
                PlaySecondFTUE().Forget();
            }
        }

        private void OnBet()
        {
            _playSecondFTUEcancellationTokenSource?.Cancel();
            
            if(_aliveBetView.Bit > 0)
            {
                G.BetController.BetToAlive(_aliveBetView.Bit);
                G.Economy.SpendMoney(_aliveBetView.Bit);
                _aliveBetView.SetBit(0);
                _dieBetView.LockButton();
            }
            
            if(_dieBetView.Bit > 0)
            {
                G.BetController.BetToDie(_dieBetView.Bit);
                G.Economy.SpendMoney(_dieBetView.Bit);
                _dieBetView.SetBit(0);
                _aliveBetView.LockButton();
            }
            
            _ftueSaveModel.Data.ShowedBetFTUE = true;
            _ftueSaveModel.ForceSave();
            
            _tutorialPointerBet.SetActive(false);
            _dieBetView.UpdateButtons();
            _aliveBetView.UpdateButtons();
        }

        private async UniTask PlayFirstFTUE()
        {
            _playFirstFTUEcancellationTokenSource = new  CancellationTokenSource();
            _ftueText.SetText("");

            await UniTask.WaitForSeconds(10);
            await _ftueText.ToType(
                "Welcome to the stream. A dangerous stream. You can place a bet on the main character's life, " +
                "whether he will survive or not. Here you can place a bet. Increase the pot.", 
                cancellationToken:_playFirstFTUEcancellationTokenSource.Token);
            if(!_playFirstFTUEcancellationTokenSource.IsCancellationRequested)
                _tutorialPointerPlusMin.SetActive(true);
        }
        
        private async UniTask PlaySecondFTUE()
        {
            _playSecondFTUEcancellationTokenSource = new  CancellationTokenSource();
            _ftueText.SetText("");
            await UniTask.WaitForSeconds(5);
            await _ftueText.ToType(
                "You have chosen the amount of money. Now you need to place your bet.", 
                cancellationToken:_playSecondFTUEcancellationTokenSource.Token);
            if(!_playSecondFTUEcancellationTokenSource.IsCancellationRequested)
                _tutorialPointerBet.SetActive(true);
        }
        
        
        private void OnStopFtuteAwait()
        {
            _timerAnimation.SetPause(false);
        }

        private void OnStartFtuteAwait()
        {
            _timerAnimation.SetPause(true);
        }
    }
}
