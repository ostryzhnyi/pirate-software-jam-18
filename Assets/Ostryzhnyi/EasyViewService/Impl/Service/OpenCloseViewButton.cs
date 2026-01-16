using Ostryzhnyi.EasyViewService.Api.Service;

namespace Ostryzhnyi.EasyViewService.Impl.Service
{
    public class OpenCloseViewButton : OpenViewButton
    {
        private IView _view;

        protected override void Start()
        {
            base.Start();
            _view = ViewService.Instance.GetView(_viewType.GetType());
        }

        protected override void OnOpenView()
        {
            if(!_view.IsOpened)
            {
                ViewService.Instance.ShowView(_viewType.GetType());
            }
            else
            {
                ViewService.Instance.HideView(_viewType.GetType());
            }
        }
    }
}