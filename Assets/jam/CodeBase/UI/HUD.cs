using Cysharp.Threading.Tasks;
using DG.Tweening;
using jam.CodeBase.Character;
using jam.CodeBase.Core;
using jam.CodeBase.Core.Stream.Views;
using jam.CodeBase.Tasks.DonateSystem;
using jam.CodeBase.Utils;
using TMPro;
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
        public FinishDonateNotification FinishDonateNotification;
        public AnswerOnChatMinigame AnswerOnChatMinigame;
        public Image FTUEImage;
        public TMP_Text FTUEText;
        public GameObject FTUEDonatePointer;

        private int donateButtonSiblingIndex;
        private Transform baseDonateParent;
        
        private void OnEnable()
        {
            donateButtonSiblingIndex = DonateHUDButton.transform.GetSiblingIndex();
            baseDonateParent = DonateHUDButton.transform.parent;
            Donate.onClick.AddListener(OpenDonate);
            FTUEImage.gameObject.SetActive(false);
        }
        
        private void OnDisable()
        {
            Donate.onClick.RemoveListener(OpenDonate);
        }

        private void OpenDonate()
        {
            G.Menu.ViewService.ShowView<DonateView>(new DonateViewOptions(G.Donate.TaskDefinition, G.Donate.BaseTasks));

            if (FTUEImage.gameObject.activeSelf)
            {
                StopDonateFTUE().Forget();
            }
        }

        public async UniTask PlayDonateFTUE()
        {
            FTUEImage.gameObject.SetActive(true);
            DonateHUDButton.transform.SetParent(FTUEImage.transform);

            await FTUEImage.DOFade(0.85f, .5f);
            await FTUEText.ToType("Now you have the ability to either help the streamer or harm him. Press the “Donate” button.");
            
            FTUEDonatePointer.gameObject.SetActive(true);
        }

        private async UniTask StopDonateFTUE()
        {
            FTUEDonatePointer.gameObject.SetActive(false);
            
            DonateHUDButton.transform.SetParent(baseDonateParent);
            DonateHUDButton.transform.SetSiblingIndex(donateButtonSiblingIndex);
            await FTUEImage.DOFade(0f, .5f).ToUniTask();
            FTUEImage.gameObject.SetActive(false);
        }
    }
}