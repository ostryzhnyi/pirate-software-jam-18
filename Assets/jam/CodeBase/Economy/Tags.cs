using System;
using UnityEngine;

namespace jam.CodeBase.Economy
{
    [Serializable]
    public class BaseEconomyTag : EntityComponentDefinition
    {
        [Header("General")]
        public float BaseMoney;
        
        [Header("Donators")]
        public float BaseDonate = 5000;
        public Vector2 DonateMultiplier = new Vector2(0.7f, 1.3f);
        public float BaseBetMultiplier = 0.25f;
        public Vector2Int DonatorsAmountMinMax = new Vector2Int(40, 60);
        public Vector2 DonateСhanceOppositeByPlayer = new Vector2(0.45f, .65f);
        public Vector2 OneDonateRandMultiplier = new Vector2(0.2f, 2f);
        
        [Header("Bets")]
        public Vector2 BaseBet = new Vector2(8000, 12000);
        public Vector2 PercentsRangeFromPlayerMoney = new Vector2(4, 6);
        public Vector2 DieBiddersProporionRange = new Vector2(0.7f, 0.5f);
        public Vector2 AliveBiddersProporionRange = new Vector2(0.5f, 0.3f);
    }
}