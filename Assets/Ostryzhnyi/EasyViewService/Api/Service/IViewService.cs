using System;

namespace Ostryzhnyi.EasyViewService.Api.Service
{
    public interface IViewService
    { 
        public event Action<IView> OnViewOpen;
        public event Action<IView> OnViewHide;
        public bool AnyViewOpen { get; }

        public void ShowView<TType>(ViewOption option = null) where TType : IView;
        public void HideView<TType>() where TType : IView;

        public void ShowView(Type type, ViewOption option = null);
        public void HideView(Type type);
        public IView GetView<TType>() where TType : IView;
        public IView GetView(Type type);
        public void HideAllViews();
    }
}