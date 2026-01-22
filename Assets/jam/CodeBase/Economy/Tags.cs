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
        public Vector2Int DonatorsAmountMinMax = new Vector2Int(2, 5);
        public Vector2Int MinRandomDonateRange = new Vector2Int(1, 10);
        public Vector2 AdditionalDonatorsMultiplierFromPlayerMoney = new Vector2(0.01f, 0.1f);
        
        [Header("Bets")]
        public Vector2 BaseBet = new Vector2(8000, 12000);
        public Vector2 PercentsRangeFromPlayerMoney = new Vector2(4, 6);
        public Vector2 DieBiddersProporionRange = new Vector2(0.7f, 0.5f);
        public Vector2 AliveBiddersProporionRange = new Vector2(0.5f, 0.3f);
    }
}