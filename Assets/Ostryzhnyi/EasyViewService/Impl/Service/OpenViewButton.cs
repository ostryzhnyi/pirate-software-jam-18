using Ostryzhnyi.EasyViewService.Api.Service;
using UnityEngine;
using UnityEngine.UI;

namespace Ostryzhnyi.EasyViewService.Impl.Service
{
    [RequireComponent(typeof(Button))]
    public class OpenViewButton : MonoBehaviour
    {
        [SerializeField] protected BaseView _viewType;

        private Button _button;

        protected virtual void Start()
        {
            _button = GetComponent<Button>();
        
            _button.onClick.AddListener(OnOpenView);
        }

        protected virtual void OnDestroy()
        {
            _button.onClick.RemoveListener(OnOpenView);
        }

        protected virtual void OnOpenView()
        {
            ViewService.Instance.ShowView(_viewType.GetType());
        }
    }
}