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
        
        [SerializeField] private TMP_Text _text;
        [SerializeField] private Image _progress;
        [SerializeField] private Button _button;

        public void Init(BaseTask task, Action<BaseTask> onClick)
        {
            Task = task;
            
            _text.SetText(task.Name + " $" + task.Price);
            
            _button.onClick.AddListener(() => onClick?.Invoke(task));
        }
        
        public void UpdateProgress(float progress)
        {
            _progress.fillAmount = progress;
        }
        
    }
}