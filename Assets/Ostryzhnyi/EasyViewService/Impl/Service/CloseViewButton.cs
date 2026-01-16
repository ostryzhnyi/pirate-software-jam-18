using Ostryzhnyi.EasyViewService.Api.Service;
using UnityEngine;
using UnityEngine.UI;

namespace Ostryzhnyi.EasyViewService.Impl.Service
{
    [RequireComponent(typeof(Button))]
    public class CloseViewButton: MonoBehaviour
    {
        [SerializeField] protected BaseView _ViewType;

        private Button _button;

        protected virtual void Start()
        {
            _button = GetComponent<Button>();
        
            _button.onClick.AddListener(OnOpenBuilderModeView);
        }

        protected virtual void OnOpenBuilderModeView()
        {
            ViewService.Instance.HideView(_ViewType.GetType());
        }
    }
}