using System;
using jam.CodeBase.Core.SavesGeneral;

namespace jam.CodeBase.Economy
{
    public class EconomySaveModel : SaveModel<EconomySave>
    {
        protected override void SetDefault()
        {
            Data = new EconomySave();
            Data.Money = -1;
            
            ForceSave();
        }
    }

    [Serializable]
    public class EconomySave
    {
        public float Money { get; set; }
    }
}