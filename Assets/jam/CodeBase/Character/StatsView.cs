using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace jam.CodeBase.Character
{
    public class StatsView : MonoBehaviour
    {
        [SerializeField] private StatView _HPView;
        [SerializeField] private StatView _stressView;

        private int _pendingUpdateCount;
        private CancellationTokenSource _showCts;

        [Sirenix.OdinInspector.Button]
        public async UniTask UpdateStress(float value, bool withoutNotify = false)
        {
            if (withoutNotify)
            {
                _stressView.UpdateWithoutNotifyValue(value);
                return;
            }
            _pendingUpdateCount++;

            if (!gameObject.activeSelf)
                await Show();

            await UniTask.Delay(1000);
            _stressView.UpdateValue(value);

            _pendingUpdateCount--;
            StartHideTimer();
        }

        [Sirenix.OdinInspector.Button]
        public async UniTask UpdateHP(float value, bool withoutNotify = false)
        {
            if (withoutNotify)
            {
                _HPView.UpdateWithoutNotifyValue(value);
                return;
            }
            
            _pendingUpdateCount++;

            if (!gameObject.activeSelf)
                await Show();

            await UniTask.Delay(400);
            _HPView.UpdateValue(value);

            _pendingUpdateCount--;
            StartHideTimer();
        }

        private async UniTask Show()
        {
            _showCts?.Cancel();
            _showCts?.Dispose();
            _showCts = new CancellationTokenSource();

            _HPView.DisableArrow();
            _stressView.DisableArrow();
            gameObject.SetActive(true);
            await transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutBounce).ToUniTask();
        }

        private void StartHideTimer()
        {
            _showCts?.Cancel();
            _showCts?.Dispose();
            _showCts = new CancellationTokenSource();

            if (_pendingUpdateCount == 0)
            {
                _ = RunHideTimer(_showCts.Token);
            }
        }

        private async UniTask RunHideTimer(CancellationToken ct)
        {
            try
            {
                await UniTask.Delay(5000, cancellationToken: ct);
                await Hide();
            }
            catch (OperationCanceledException)
            {
            }
        }

        private async UniTask Hide()
        {
            _showCts?.Cancel();

            _pendingUpdateCount = 0;

            await transform.DOScale(Vector3.zero, 0.2f).ToUniTask();
            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            _showCts?.Cancel();
            _showCts?.Dispose();
        }
    }
}