using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using jam.CodeBase.Core;
using jam.CodeBase.Tasks.Interactors;
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
        
        [SerializeField] private Transform _window;
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
        
        [SerializeField] private Image[] _taskOneContours;
        [SerializeField] private Image[] _taskTwoContours;
        
        
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

        private void UpdateDotateProgress(float obj)
        {
            _donateProgress.SetAmount(obj);
        }

        protected override void Showed(ViewOption option = null)
        {
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
            _donate.interactable = G.Economy.CanSpend(Price) && Price != 0;
            _plus.interactable = G.Economy.CanSpend(Price + MinBit);
            _minus.interactable = MinBit < Price;
            _priceText.SetText("$ " + Price);
        }

        protected override void Hided()
        {
            _donate.onClick.RemoveListener(OnDonate);
        }

        private void OnDonate()
        {
            G.Economy.SpendMoney(Price);
            G.Interactors.CallAll<IDonate>((d) => d.Donate(_selected.Task, Price));
            _window.DOPunchScale(Vector3.one * 0.05f, .2f, 20);
            UpdateText();
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