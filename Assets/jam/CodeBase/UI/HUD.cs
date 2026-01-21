using jam.CodeBase.Character;
using jam.CodeBase.Core;
using jam.CodeBase.Tasks.DonateSystem;
using UnityEngine;
using UnityEngine.UI;

namespace jam.CodeBase.UI
{
    public class HUD : MonoBehaviour
    {
        public Button Donate;
        public StatsView StatsView;
        public DonateHUDButton DonateHUDButton;
        public StartDonateNotification DonateNotification;

        private void OnEnable()
        {
            Donate.onClick.AddListener(OpenDonate);
        }
        
        private void OnDisable()
        {
            Donate.onClick.RemoveListener(OpenDonate);
        }

        private void OpenDonate()
        {
            G.Menu.ViewService.ShowView<DonateView>(new DonateViewOptions(G.Donate.TaskDefinition, G.Donate.BaseTasks));
        }
    }
}