using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace jam.CodeBase.Tasks.DonateSystem
{
    public class DonateButton : MonoBehaviour
    {
        public BaseTask Task { get; private set; }
        public Button Button => _button;
        public float Price;
        
        [SerializeField] private TMP_Text _text;
        [SerializeField] private Image _progress;
        [SerializeField] private Button _button;
        [SerializeField] private Button _plus;
        [SerializeField] private Button _minus;

        private const float MinBit = 50;

        public void Init(BaseTask task, Action<DonateButton> onClick)
        {
            Task = task;
            Price = task.Price;
            UpdateText();
            _button.onClick.AddListener(() => onClick?.Invoke(this));
            
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
        }

        private void UpdateText()
        {
            _minus.interactable = MinBit < Price;
            _text.SetText(Task.Name + " $" + Price);
        }
        
        public void UpdateProgress(float progress)
        {
            _progress.fillAmount = progress;
        }
        
    }
}