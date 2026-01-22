using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using System.Threading;
using System.Threading.Tasks;
using jam.CodeBase.Core.Tags;
using UnityEngine;
using Random = UnityEngine.Random;

namespace jam.CodeBase.Stream
{
    public class ChatController
    {
        public event Action<ChatMessage> OnMessageReceived;
        public event Action<ChatMessage> OnDayUpdated;

        private static Vector2 DEFAULT_TIME_RANGE = new Vector2(0.1f, 5f);
        private static Vector2 REACTIONS_TIME_RANGE = new Vector2(0.1f, 0.5f);

        private List<ChatMessage> _dailyMessages = new();
        private RepeatableMessages _repeatableMessages = new();
        private CMSEntity _targetEntity = new();
        private CancellationTokenSource _cst;

        public void InitializeData(CMSEntity entity, int day)
        {
            _targetEntity = entity;
            _dailyMessages = CMS.GetAll<CMSEntity>().First(e => e.Is<DailyMessages>() && e.Get<DailyMessages>().Day == day).Get<DailyMessages>().Messages;
            _repeatableMessages = CMS.GetAll<CMSEntity>().First(e => e.Is<RepeatableMessages>()).Get<RepeatableMessages>();
            _cst = new CancellationTokenSource(); 
        }

        public void Dispose()
        {
            _cst?.Cancel();
        }

        public async UniTask StartMessaging()
        {
            foreach (var message in _dailyMessages.OrderBy(x => Random.value))
            {
                var data = GetData(message.Type);
                var messageData = new ChatMessage(message.Sender, string.Format(message.Message, data?.ToLower()));
                OnMessageReceived?.Invoke(messageData);
                Debug.Log($"Send message: {messageData.Sender} : {messageData.Message}");
                var waitTIme = Random.Range(DEFAULT_TIME_RANGE.x, DEFAULT_TIME_RANGE.y);
                await UniTask.Delay(TimeSpan.FromSeconds(waitTIme), cancellationToken: _cst.Token);
            }
        }

        private string GetData(MessageDataType messageType)
        {
            return messageType switch
            {
                MessageDataType.DescriptionTag => _targetEntity.Get<DescribeTag>()?.Description,
                MessageDataType.NameTag => _targetEntity.Get<NameTag>()?.Name,
                _ => null
            };
        }

        public void ShowDonateMessage(int value, string goal)
        {
            var chatMessage = new ChatMessage("", $"Someone donated ${value} to {goal}", MessageDataType.Donate);
            Debug.Log($"Send message: {chatMessage.Message}");
            OnMessageReceived?.Invoke(chatMessage);
        }

        public async void ShowReactionMessage(int actionType)
        {
            var senders = _repeatableMessages.Senders;
            var messages = _repeatableMessages.Messages.Where(x => x.FeedbackType == (FeedbackType)actionType).ToList();

            var reactionsCount = Random.Range(1, senders.Count);

            foreach (var sender in senders.OrderBy(x => Random.value).Take(reactionsCount))
            {
                var chatMessage = new ChatMessage(sender, messages.ElementAt(Random.Range(0, messages.Count)).Message);
                OnMessageReceived?.Invoke(chatMessage);
                Debug.Log($"Send message: {chatMessage.Sender} : {chatMessage.Message}");
                var waitTIme = Random.Range(REACTIONS_TIME_RANGE.x, REACTIONS_TIME_RANGE.y);
                await UniTask.Delay(TimeSpan.FromSeconds(waitTIme), cancellationToken: _cst.Token);
            }
        }
    }

    [Serializable]
    public class DailyMessages : EntityComponentDefinition
    {
        public int Day;
        public List<ChatMessage> Messages = new();
    }

    [Serializable]
    public class ChatMessage
    {
        public MessageDataType Type;
        public string Sender;
        public string Message;

        public ChatMessage(string sender, string message)
        {
            Sender = sender;
            Message = message;
        }

        public ChatMessage(string sender, string message, MessageDataType type)
        {
            Type = type;
            Sender = sender;
            Message = message;
        }
    }

    public enum MessageDataType
    {
        None,
        DescriptionTag,
        NameTag,
        Donate
    }

    [Serializable]
    public class RepeatableMessages : EntityComponentDefinition
    {
        public List<string> Senders = new();
        public List<ChatReaction> Messages = new();
    }

    [Serializable]
    public class ChatReaction
    {
        public FeedbackType FeedbackType;
        public string Message;
    }

    public enum FeedbackType
    {
        Neutral = 0,
        Posittive = 1,
        Negative = 2,
    }
}