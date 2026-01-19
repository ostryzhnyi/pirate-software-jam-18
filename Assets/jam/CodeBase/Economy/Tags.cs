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
        public Vector2Int DonatorsAmountMinMax;
        public Vector2Int RandomDonateRange;
        
        [Header("Bets")]
        public Vector2 BetBaseDieCoefficientRange;
        public Vector2 BetBaseAliveCoefficientRange;
        [Tooltip("Amount donators human")]
        public Vector2 AmountBetsRange = new Vector2(80, 120);
        public Vector2 BaseBet = new Vector2(8000, 12000);
        public Vector2 PercentsRangeFromPlayerMoney = new Vector2(400, 600);
        [Tooltip("X - Die, Y - Alive")]
        public Vector2 BiddersProporion = new Vector2(0.6f, 0.4f);
        
        public float CoefficientScale = 3;
    }
}