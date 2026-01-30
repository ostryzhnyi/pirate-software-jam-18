using Cysharp.Threading.Tasks;
using jam.CodeBase.Core;
using jam.CodeBase.Utils;
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
        [SerializeField] private Button _continue;

        protected override void Showed(ViewOption option = null)
        {
            base.Showed(option);
            
            _text.ToType(CastedOption.Message).Forget();
            _hide.gameObject.SetActive(!CastedOption.IsLast);
            
            _restartGame.gameObject.SetActive(CastedOption.IsShowRestart);
            _continue.gameObject.SetActive(!CastedOption.IsShowRestart);
            
            _restartGame.onClick.RemoveAllListeners();
            _restartGame.onClick.AddListener(OnRestartGame);
            
            _continue.onClick.RemoveAllListeners();
            _continue.onClick.AddListener(() => Hide().Forget());
            
            _hide.onClick.RemoveAllListeners();
            _hide.onClick.AddListener(() => Hide().Forget());
        }

        private void OnRestartGame()
        {
            G.RestartGame().Forget();
        }
    }

    public class GlithcesViewOption : ViewOption
    {
        public bool IsLast;
        public bool IsShowRestart;

        public string Message;

        public GlithcesViewOption(bool isLast, bool isShowRestart, string message)
        {
            IsLast = isLast;
            Message = message;
            IsShowRestart = isShowRestart;
        }
    }
}