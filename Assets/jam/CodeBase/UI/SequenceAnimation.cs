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

        private bool _pause;

        private CancellationTokenSource _cancellationTokenSource;
        
        public async UniTask Play()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            int i = 0;
            while (!_cancellationTokenSource.IsCancellationRequested)
            {
                while(_pause)
                {
                    await UniTask.WaitForSeconds(_timeTick);
                }
                    
                _image.sprite = _sequence[i];
                i++;
                
                await UniTask.WaitForSeconds(_timeTick);
            }
        }

        public void Stop()
        {
            _cancellationTokenSource.Cancel();
        }

        public void SetPause(bool pause)
        {
            _pause =  pause;
        }

        private void OnDestroy()
        {
            Stop();
        }
    }
}