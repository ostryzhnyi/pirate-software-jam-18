using System;
using jam.CodeBase.Stream;
using UnityEngine;

namespace jam.CodeBase.Economy
{
    [Serializable]
    public class BaseEconomyTag : EntityComponentDefinition
    {
        [Header("General")]
        public float BaseMoney;
        public Vector2 RestoreMoneyByDayRange = new Vector2(100, 1000);
        
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
        
        [Header("Stats")]
        public Vector2 RestoreStressByDayRange = new Vector2(5, 15);
        public Vector2 RestoreHealthByDayRange = new Vector2(5, 15);
    }

    [Serializable]
    public class ChatMiniGameEconomy : EntityComponentDefinition
    {
        public float OneTick = .33f;
        public int Duration = 30;
        public Vector2 OneMessagePerTickRange = new Vector2(1, 2);
        public Vector2 GoodMesssagePercentRange = new Vector2(0, 75);
        public float GoodMesssageDelete = 10f;
        public float BedMesssageSkipped = 20f;
        public float BedMesssageDeleted = 20f;
        public float BedMesssageSkippedStess = 2f;
    }

    [Serializable]
    public class ChatMiniGameMessages : EntityComponentDefinition
    {
        public ChatMiniGameMessage[] Messages;
    }

    [Serializable]
    public class ChatMiniGameMessage
    {
        public MessageDataType Type;
        public string Author;
        public string Message;
        public string CharacterAnswer;
    }
    
}