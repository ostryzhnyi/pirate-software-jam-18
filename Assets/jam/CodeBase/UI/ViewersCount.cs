using TMPro;
using UnityEngine;
using Cysharp.Threading.Tasks;
using jam.CodeBase.Utils;

namespace jam.CodeBase.UI
{
    public class ViewersCount : MonoBehaviour
    {
        [SerializeField] private TMP_Text _viewersCount;

        [SerializeField] private int _min = 300;
        [SerializeField] private int _max = 600;
        [SerializeField] private float _updateInterval = 2f;
        [SerializeField] private float _changeDuration = 1.2f;

        int _currentValue;
        int _targetValue;
        bool _running;

        void OnEnable()
        {
            _currentValue = Random.Range(_min, _max + 1);
            _targetValue = _currentValue;
            _viewersCount.text = _currentValue.ToString();
            _running = true;
            RunLoop().Forget();
        }

        void OnDisable()
        {
            _running = false;
        }

        async UniTaskVoid RunLoop()
        {
            while (_running)
            {
                await UniTask.Delay(System.TimeSpan.FromSeconds(_updateInterval));

                var next = Random.Range(_min, _max + 1);
                _targetValue = next;

                var t = 0f;
                var start = _currentValue;

                while (t < 1f && _running)
                {
                    t += Time.deltaTime / _changeDuration;
                    var v = Mathf.RoundToInt(Mathf.Lerp(start, _targetValue, t));
                    if (v != _currentValue)
                    {
                        _currentValue = v;
                        _viewersCount.DOFloatNumber(_currentValue, .5f);
                    }

                    await UniTask.Yield(PlayerLoopTiming.Update);
                }

                _currentValue = _targetValue;
                _viewersCount.text = _currentValue.ToString();
            }
        }
    }
}