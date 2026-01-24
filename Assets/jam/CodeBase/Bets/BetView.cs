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
        private bool IsLocked = false;

        private bool _isFirstBet = false;

        public void OnEnable()
        {
            IsLocked = false;

            _isFirstBet = G.Saves.Get<BetSaveModel>().Data.IsFirst;
            
            _plus.onClick.AddListener(OnPlus);
            _minus.onClick.AddListener(OnMinus);

            UpdateButtons();
        }

        public void OnDisable()
        {
            _plus.onClick.RemoveListener(OnPlus);
            _minus.onClick.RemoveListener(OnMinus);
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
            _plus.interactable =
                !IsLocked &&
                G.Economy.CanSpend(Bit + OneBit) &&
                (!_isFirstBet || Bit + OneBit + G.BetController.MyBet <= 500);

            _minus.interactable =
                !IsLocked &&
                Bit >= OneBit;
        }

        public void LockButton()
        {
            IsLocked = true;
            
            _plus.interactable = false;
            _minus.interactable = false;
        }
    }
}
