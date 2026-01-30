using Cysharp.Threading.Tasks;
using jam.CodeBase.Core;
using Ostryzhnyi.EasyViewService.Api.Service;
using Ostryzhnyi.EasyViewService.ViewLayers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace jam.CodeBase.Glitches
{
    public class GlithcesView : BaseOptionView<GlithcesViewOption>
    {
        public override ViewLayers Layer => ViewLayers.Popup;

        [SerializeField] private TMP_Text _text;
        [SerializeField] private Button _hide;
        [SerializeField] private Button _restartGame;

        protected override void Showed(ViewOption option = null)
        {
            base.Showed(option);
            
            _text.SetText(CastedOption.Message);
            _hide.gameObject.SetActive(!CastedOption.IsLast);
            
            _restartGame.onClick.RemoveAllListeners();
            _restartGame.onClick.AddListener(OnRestartGame);
            
            _hide.onClick.RemoveAllListeners();
            _hide.onClick.AddListener(()=>Hide().Forget());
        }

        private void OnRestartGame()
        {
            G.RestartGame().Forget();
        }
    }

    public class GlithcesViewOption : ViewOption
    {
        public bool IsLast;

        public string Message;

        public GlithcesViewOption(bool isLast, string message)
        {
            IsLast = isLast;
            Message = message; 
        }
    }
}