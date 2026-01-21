using System;
using jam.CodeBase.Core.SavesGeneral;

namespace jam.CodeBase.Bets
{
    public class BetSaveModel : SaveModel<BetSaveData>
    {
        protected override void SetDefault()
        {
            Data = new BetSaveData()
            {
                DieBet = 0,
                AliveBet = 0
            };
        }
    }

    [Serializable]
    public class BetSaveData
    {
        public float DieBet;
        public float AliveBet;
    }
}