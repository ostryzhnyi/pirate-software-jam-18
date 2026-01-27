using Cysharp.Threading.Tasks;
using Ostryzhnyi.EasyViewService.Api.Service;
using Ostryzhnyi.EasyViewService.ViewLayers;

namespace jam.CodeBase.UI
{
    public class CreditsView : BaseView
    {
        public override ViewLayers Layer => ViewLayers.Popup;

        public void HideView()
        {
            base.Hide().Forget();
        }
    }
}