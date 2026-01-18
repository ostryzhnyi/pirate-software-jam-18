using Cysharp.Threading.Tasks;
using Ostryzhnyi.EasyViewService.Api.Animation;
using Ostryzhnyi.EasyViewService.Impl.Animation;
using UnityEngine;

namespace Ostryzhnyi.EasyViewService.Api.Service
{
    public abstract class BaseView : MonoBehaviour, IView
    {
        public bool IsOpened => gameObject.activeSelf;
        public abstract ViewLayers.ViewLayers Layer { get; }
        
        protected ViewOption _option;
        private BaseAnimationStrategy _baseAnimationStrategy;
        
        public virtual void Show(ViewOption option = null)
        {
            _option = option;

            if (_baseAnimationStrategy == null)
            {
                if (TryGetComponent<BaseAnimationStrategy>(out var animationStrategy))
                {
                    _baseAnimationStrategy = animationStrategy;
                }
                else
                {
                    _baseAnimationStrategy = gameObject.AddComponent<InstantAnimationStrategy>();
                }
            }
            
            _baseAnimationStrategy.Show().Forget();

            Showed(option);
        }

        protected virtual void Showed(ViewOption option = null) { }

        public virtual async UniTask Hide()
        {
            if(_baseAnimationStrategy != null)
            {
                await _baseAnimationStrategy.Hide();
            }
            else
            {
                gameObject.SetActive(false);
            }
            Hided();
            _option = null;
        }

        protected virtual void Hided() { }
    }
}