using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using jam.CodeBase.Core;
using jam.CodeBase.Tasks.Interactors;
using jam.CodeBase.Utils;
using Ostryzhnyi.EasyViewService.Api.Service;
using Ostryzhnyi.EasyViewService.ViewLayers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace jam.CodeBase.Tasks.DonateSystem
{
    public class DonateView : BaseOptionView<DonateViewOptions>
    {
        public override ViewLayers Layer => ViewLayers.Popup;
        
        public List<DonateButton> DonateButtons = new  List<DonateButton>();
        public float Price;
        
        [SerializeField] private RectTransform _window;
        [SerializeField] private Button _donate;
        [SerializeField] private TMP_Text _text;
        [SerializeField] private TMP_Text _priceText;
        [SerializeField] private Button _plus;
        [SerializeField] private Button _minus;
        [SerializeField] private Button _hide;
        [SerializeField] private Button _hideBack;
        [SerializeField] private DonateHUDButton _donateProgress;
        [SerializeField] private HoverDonateButton _plusHover;
        [SerializeField] private HoverDonateButton _minusHover;
        [SerializeField] private TMP_Text _ftueText;
        [SerializeField] private GameObject _plusMinTutorialPointer;
        [SerializeField] private GameObject _betCloseTutorialPointer;
        [SerializeField] private RectTransform _targetTutorialPosition;
        
        [SerializeField] private Image[] _taskOneContours;
        [SerializeField] private Image[] _taskTwoContours;

        private FTUESaveModel _ftueSaveModel;
        private DonateButton _selected;
        
        private const float MinBit = 50;

        private void Awake()
        {
            G.Donate.OnDonateProgressUpdated += UpdateDotateProgress;
            
            _plus.onClick.AddListener(() =>
            {
                Price += MinBit;
                UpdateText();
            });
            
            _minus.onClick.AddListener(() =>
            {
                Price -= MinBit;
                UpdateText();
            });
            
            _hide.onClick.AddListener(() => Hide().Forget());
            _hideBack.onClick.AddListener(() => Hide().Forget());
            
        }

        private void OnDestroy()
        {
            G.Donate.OnDonateProgressUpdated -= UpdateDotateProgress;
        }

        private void OnDisable()
        {
            _betCloseTutorialPointer.SetActive(false);
        }

        private void UpdateDotateProgress(float obj)
        {
            _donateProgress.SetAmount(obj);
        }

        protected override void Showed(ViewOption option = null)
        {
            _window.anchoredPosition = Vector3.zero;
            _ftueSaveModel = G.Saves.Get<FTUESaveModel>();
            
            if(!_ftueSaveModel.Data.ShowedDonateFTUE)
                PlayFirstFTUE().Forget();
            
            _donateProgress.SetAmount(G.Donate.DonateProgress, true);
            Price = Math.Min(G.Economy.CurrentMoney, CastedOption.TaskDefinition.BasePrice);
            
            UpdateText();
            
            _text.text = CastedOption.TaskDefinition.Description;
            
            foreach (var donateButton in DonateButtons)
            {
                donateButton.Button.interactable = true;
            }
            var sum = G.Donate.Donates.Sum(d => d.Value);
            for (var i = 0; i < CastedOption.Tasks.Count; i++)
            {
                var baseTask = CastedOption.Tasks[i];
                DonateButtons[i].Init(baseTask, OnClick);
                DonateButtons[i].UpdateProgressWithoutAnim(G.Donate.Donates[baseTask] / sum);
            }
            
            DonateButtons.First().Button.onClick.Invoke();
            _donate.interactable = G.Economy.CanSpend(Price);
            
            _donate.onClick.AddListener(OnDonate);
        }

        private void UpdateText()
        {
            _playFirstFTUEcancellationTokenSource?.Cancel();
            if(!_ftueSaveModel.Data.ShowedDonateFTUE)
                PlaySecondFTUE().Forget();
            
            _plusMinTutorialPointer.SetActive(false);
            
            _donate.interactable = G.Economy.CanSpend(Price) && Price != 0;
            _plus.interactable = G.Economy.CanSpend(Price + MinBit);
            _minus.interactable = MinBit < Price;
            _priceText.SetText("$ " + Price);
        }

        protected override void Hided()
        {
            _donate.onClick.RemoveListener(OnDonate);
            _playSecondFTUEcancellationTokenSource?.Cancel();
            _ftueSaveModel.Data.ShowedDonateFTUE = true;
            _ftueSaveModel.ForceSave();
        }

        private void OnDonate()
        {
            G.Economy.SpendMoney(Price);
            G.Interactors.CallAll<IDonate>((d) => d.Donate(_selected.Task, Price));
            _window.DOPunchScale(Vector3.one * 0.05f, .2f, 20);
            UpdateText();
            _betCloseTutorialPointer.SetActive(false);
            
            _playSecondFTUEcancellationTokenSource?.Cancel();
            _ftueSaveModel.Data.ShowedDonateFTUE = true;
            _ftueSaveModel.ForceSave();
        }

        private void OnClick(DonateButton button)
        {
            _selected = button;

            _plusHover.SetState(button.Task.Name != DonateButtons.First().Task.Name);
            _minusHover.SetState(button.Task.Name == DonateButtons.First().Task.Name);
            
            foreach (var donateButton in DonateButtons)
            {
                donateButton.SetSelected(button == donateButton);
            }
        }
        
        public void LockButtons()
        {
            foreach (var donateButton in DonateButtons)
            {
                donateButton.Button.interactable = false;
            }
        }

        public DonateButton GetDonateButton(BaseTask task)
        {
            return DonateButtons.FirstOrDefault(d => d.Task == task);
        }


        private CancellationTokenSource _playFirstFTUEcancellationTokenSource;
        
        private async UniTask PlayFirstFTUE()
        {
            await _window.DOAnchorPos(_targetTutorialPosition.anchoredPosition, .5f);
            _playFirstFTUEcancellationTokenSource = new  CancellationTokenSource();
            _ftueText.SetText("");

            await _ftueText.ToType(
                "Choose the option you want to vote for. Note that your decision can change what is happening.", 
                cancellationToken:_playFirstFTUEcancellationTokenSource.Token);

            await UniTaskHelper.SmartWaitSeconds(3f);
            
           await _ftueText.ToType(
                "Place your bet by increasing or decreasing the current pot.", 
                cancellationToken:_playFirstFTUEcancellationTokenSource.Token);
            
            _plusMinTutorialPointer.SetActive(true);
        }


        private CancellationTokenSource _playSecondFTUEcancellationTokenSource;
        
        private async UniTask PlaySecondFTUE()
        {
            _playSecondFTUEcancellationTokenSource = new  CancellationTokenSource();
            _ftueText.SetText("");

            await _ftueText.ToType(
                "After choosing the desired option and amount, you need to send it to the streamer or skip this donate if you want.", 
                cancellationToken:_playFirstFTUEcancellationTokenSource.Token);

            await UniTaskHelper.SmartWaitSeconds(3f);
            
            _betCloseTutorialPointer.SetActive(true);
        }
    }

    public class DonateViewOptions : ViewOption
    {
        public TaskDefinition TaskDefinition;
        public List<BaseTask> Tasks;

        public DonateViewOptions(TaskDefinition taskDefinition, List<BaseTask> tasks)
        {
            TaskDefinition = taskDefinition;
            Tasks = tasks;
        }
    }
}