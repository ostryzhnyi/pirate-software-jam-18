using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using jam.CodeBase.Core;
using jam.CodeBase.Utils;
using Ostryzhnyi.EasyViewService.Api.Service;
using Ostryzhnyi.EasyViewService.ViewLayers;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace jam.CodeBase.GameLoop
{
    public class ResultScreen : BaseView
    {
        public override ViewLayers Layer => ViewLayers.Popup;

        [Title("Weight")]
        [SerializeField] private Transform _weight;
        [SerializeField] private Transform _aliveCardParent;
        [SerializeField] private Transform _dieCardParent;
        [SerializeField] private float _maxRotate = 15f;
        [SerializeField] private float _rotateDuration = 2f;
        [SerializeField] private Ease _ease = Ease.OutQuad;
        
        [Title("Text")]
        [SerializeField] private TMP_Text _mainText;
        [SerializeField] private TMP_Text _value;
        [SerializeField] private GameObject _betValue;
        [SerializeField] private TMP_Text _seeYou;
        [SerializeField] private TMP_Text _tapAnyButtonForPlayNext;
        
        
        private Tween _weightTween;
        private Tween _aliveTween;
        private Tween _dieTween;
        
        public void SetState(bool isWin)
        {
            AnimationFlow(isWin, G.Characters.CurrentCharacter.IsDie).Forget();
        }

        private async UniTask AnimationFlow(bool isWin, bool isDie)
        {
            float aliveK = isDie  ? 0 : 1;
            float dieK   = isDie  ? 1 : 0;

            float diff = aliveK - dieK;                
            float angle = diff * _maxRotate;         

            _weightTween?.Kill();
            _aliveTween?.Kill();
            _dieTween?.Kill();
            await UniTask.WaitForSeconds(_rotateDuration / 2);

            _weightTween = _weight
                .DOLocalRotate(new Vector3(0f, 0f, -angle), _rotateDuration)
                .SetEase(_ease);

            _aliveTween = _aliveCardParent
                .DOLocalRotate(new Vector3(0f, 0f, angle), _rotateDuration)
                .SetEase(_ease);

            _dieTween = _dieCardParent
                .DOLocalRotate(new Vector3(0f, 0f, angle), _rotateDuration)
                .SetEase(_ease);
            
            await UniTask.WaitForSeconds(_rotateDuration);
            
            await _mainText.ToType(string.Format("He {0}, you {1}", (isDie ? "die" : "survived"), (isWin ? "won" : "lose")), 0.06f);

            var resultMoney = 0f;
            await _betValue.transform.DOScale(2.610547f, 1f).SetEase(Ease.InOutExpo).ToUniTask();
            if (isWin)
            {
                var wonMoney = G.BetController.MyBet *
                               (isDie ? G.BetController.DieBetCoefficient : G.BetController.AliveBetCoefficient);
                
                G.Economy.AddMoney(wonMoney);
            }
            else
            {
                resultMoney = G.BetController.MyBet;
            }
            await _value.DOFloatNumber(0, resultMoney, 2f, step: 10).ToUniTask();
            await _seeYou.ToType("See you on next stream...", 0.06f);
            WaitClose().Forget();
            await UniTask.WaitForSeconds(10f, cancellationToken: gameObject.GetCancellationTokenOnDestroy());
            await _tapAnyButtonForPlayNext.ToType("Tap any button to play next round...", 0.06f);
        }

        private async UniTask WaitClose()
        {
            while (!Input.anyKeyDown)
            {
                await  UniTask.Yield();
            }

            await Hide();
        }
    }
}
