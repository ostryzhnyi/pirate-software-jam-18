using System;
using jam.CodeBase.Core.SavesGeneral;
using UnityEngine;

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

        public override void ForceSave()
        {
            base.ForceSave();
        }
    }

    [Serializable]
    public class BetSaveData
    {
        public float DieBet;
        public float AliveBet;
    }
}