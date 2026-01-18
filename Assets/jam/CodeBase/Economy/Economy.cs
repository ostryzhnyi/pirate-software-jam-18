using System;
using jam.CodeBase.Core;
using UnityEngine;

namespace jam.CodeBase.Economy
{
    public class Economy
    {
        public event Action<float> OnMoneyChanged;
        
        public float CurrentMoney => _economySaveModel.Data.Money;

        private EconomySaveModel _economySaveModel;
        
        public Economy()
        {
            _economySaveModel = G.Saves.Get<EconomySaveModel>();

            if (_economySaveModel.Data.Money <= 0)
            {
                _economySaveModel.Data.Money = GameResources.CMS.BaseEconomy.AsEntity().Get<BaseEconomyTag>().BaseMoney;
                _economySaveModel.ForceSave();
            }
        }

        public bool CanSpend(float money)
        {
            return _economySaveModel.Data.Money >= money;
        }
        
        public void SpendMoney(float money)
        {
            if (!CanSpend(money))
            {
                Debug.LogError("Use CanSpend");
                return;
            }

            Debug.LogError("SPEND: " + money);
            _economySaveModel.Data.Money -= money;
            OnMoneyChanged?.Invoke(_economySaveModel.Data.Money);
            _economySaveModel.ForceSave();
        }
        
        public void AddMoney(float money)
        {
            _economySaveModel.Data.Money += money;
            OnMoneyChanged?.Invoke(_economySaveModel.Data.Money);
            
            _economySaveModel.ForceSave();
        }
        
    }
}