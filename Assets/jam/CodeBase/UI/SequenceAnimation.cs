using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace jam.CodeBase.UI
{
    public class SequenceAnimation : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private Sprite[] _sequence;

        [SerializeField] private float _timeTick;

        private CancellationTokenSource _cancellationTokenSource;
        
        public async UniTask Play()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            int i = 0;
            while (!_cancellationTokenSource.IsCancellationRequested)
            {
                _image.sprite = _sequence[i];
                i++;
                
                await UniTask.WaitForSeconds(_timeTick);
            }
        }

        public void Stop()
        {
            _cancellationTokenSource.Cancel();
        }

        private void OnDestroy()
        {
            Stop();
        }
    }
}