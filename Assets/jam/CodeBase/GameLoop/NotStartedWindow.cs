using Cysharp.Threading.Tasks;
using jam.CodeBase.Core;
using jam.CodeBase.Utils;
using Ostryzhnyi.EasyViewService.Api.Service;
using Ostryzhnyi.EasyViewService.ViewLayers;
using TMPro;
using UnityEngine;

namespace jam.CodeBase.GameLoop
{
    public class NotStartedWindow : BaseView
    {
        public override ViewLayers Layer => ViewLayers.Popup;

        [SerializeField] private TMP_Text _text;

        protected override void Showed(ViewOption option = null)
        {
            base.Showed(option);
            _text.SetText("");
            
            Play().Forget();
        }

        private async UniTask Play()
        {
            await _text.ToType(
                "I came across something strange, but I chose not to get involved — it wasn’t my business.", 0.06f);
            await UniTaskHelper.SmartWaitSeconds(5);
            
            await _text.ToType(
                "A few days later, I stumbled upon the same thing again.", 0.06f);
            
            await UniTaskHelper.SmartWaitSeconds(3);
            Hide().Forget();
            
            G.RestartRun().Forget();
        }
    }
}