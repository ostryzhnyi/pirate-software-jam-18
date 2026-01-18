using Cysharp.Threading.Tasks;
using jam.CodeBase.Core;
using jam.CodeBase.UI;
using Ostryzhnyi.EasyViewService.Impl.Service;
using UnityEngine;

namespace jam.CodeBase
{
    public class M : MonoBehaviour
    {
        public ViewService ViewService;
        public HUD HUD;

        private void Awake()
        {
            G.Menu = this;
        }

        private void Start()
        {
            G.Donate.DonateExecuteProcess().Forget();
        }

        private void OnDestroy()
        {
            G.Menu = null;
        }
    }
}