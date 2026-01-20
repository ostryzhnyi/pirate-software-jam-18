using System;
using jam.CodeBase.Core;
using jam.CodeBase.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace jam.CodeBase.Bets
{
    public class BetView : MonoBehaviour
    {
        public float Bit { private set; get; } = 0;
        public event Action<float> OnBitChange;
        
        [SerializeField] private Button _plus;
        [SerializeField] private Button _minus;
        [SerializeField] private TMP_Text _currentBit;
        
        private const float OneBit = 50;

        public void OnEnable()
        {
            _plus.onClick.AddListener(OnPlus);
            _minus.onClick.AddListener(OnMinus);
        }

        public void OnDisable()
        {
            _plus.onClick.AddListener(OnPlus);
            _minus.onClick.AddListener(OnMinus);
        }

        private void OnPlus()
        {
            SetBit(Bit + OneBit);
        }

        private void OnMinus()
        {
            if(Bit <= 0)
                return;
            
            SetBit(Bit - OneBit);
        }

        public void SetBit(float bit, bool withoutNotify = false)
        {
            Bit = bit;
            UpdateButtons();
            _currentBit.DOFloatNumber(Bit, .2f, "${0:0}", 5);
            if(!withoutNotify)
                OnBitChange?.Invoke(bit);
        }

        public void UpdateButtons()
        {
            _plus.interactable = G.Economy.CanSpend(Bit + OneBit);
            _minus.interactable = Bit >= OneBit;
        }
    }
}
