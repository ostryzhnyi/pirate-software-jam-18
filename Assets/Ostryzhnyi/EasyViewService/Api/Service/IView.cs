using Cysharp.Threading.Tasks;

namespace Ostryzhnyi.EasyViewService.Api.Service
{
    public interface IView
    {
        public bool IsOpened { get; }
        public ViewLayers.ViewLayers Layer { get; }

        public void Show(ViewOption option = null);
        public UniTask Hide();
    }
}