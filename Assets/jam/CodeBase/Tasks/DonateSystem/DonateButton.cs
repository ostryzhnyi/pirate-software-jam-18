using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace jam.CodeBase.Tasks.DonateSystem
{
    public class DonateButton : MonoBehaviour
    {
        public BaseTask Task { get; private set; }
        public Button Button => _button;
        
        [SerializeField] private TMP_Text _text;
        [SerializeField] private Image _progress;
        [SerializeField] private Button _button;
        [SerializeField] private Image _selectedMark;
        [SerializeField] private Sprite _selected;
        [SerializeField] private Sprite _disabled;

        public void Init(BaseTask task, Action<DonateButton> onClick)
        {
            Task = task;
            UpdateText();
            _button.onClick.RemoveAllListeners();
            _button.onClick.AddListener(() =>
            {
                onClick?.Invoke(this);
            });
        }

        private void UpdateText()
        {
            _text.SetText(Task.Name);
        }
        
        public void UpdateProgress(float progress)
        {
            _progress.DOFillAmount(progress, .2f).SetEase(Ease.OutSine);
        }
        
        public void UpdateProgressWithoutAnim(float progress)
        {
            _progress.fillAmount = progress;
        }

        public void SetSelected(bool isSelected)
        {
            _selectedMark.sprite = isSelected ? _selected : _disabled;
        }

    }
}