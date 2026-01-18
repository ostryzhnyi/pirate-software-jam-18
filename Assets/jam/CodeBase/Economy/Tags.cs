using System;
using UnityEngine;

namespace jam.CodeBase.Economy
{
    [Serializable]
    public class BaseEconomyTag : EntityComponentDefinition
    {
        public float BaseMoney;
        public Vector2Int DonatorsAmountMinMax;
        public Vector2Int RandomDonateRange;
    }
}