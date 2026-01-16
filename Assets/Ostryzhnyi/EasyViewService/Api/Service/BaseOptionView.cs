using Ostryzhnyi.EasyViewService.Api.Service;

namespace Ostryzhnyi.EasyViewService.Api.Service
{
    public abstract class BaseOptionView<TOption> : BaseView where TOption : ViewOption
    {
        protected TOption CastedOption => (TOption)_option;
    }
}