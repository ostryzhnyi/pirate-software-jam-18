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
                AliveBet = 0,
                MyBetLive = 0,
                MyBetDie = 0,
                IsFirst = true
            };
        }
    }

    [Serializable]
    public class BetSaveData
    {
        public float DieBet;
        public float AliveBet;
        public float MyBetDie;
        public float MyBetLive;
        public bool IsFirst;
    }
}