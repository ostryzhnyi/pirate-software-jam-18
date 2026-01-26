using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using jam.CodeBase.Core;
using jam.CodeBase.Economy;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace jam.CodeBase.Stream.View
{
    public class ChatMessageView : MonoBehaviour
    {
        [SerializeField] private ChatElementView _prefab;
        [SerializeField] private ChatElementView _donateMessage;
        [SerializeField] private ChatElementView _positiveMessage;
        [SerializeField] private ChatElementView _negativeMessage;
        [SerializeField] private Transform _parent;
        [SerializeField] private ScrollRect _scrollRect;
        [SerializeField] private TMP_Text _streamDayText;
        [SerializeField] private Color[] _senderColors;
        
        public IReadOnlyList<ChatElementView> Elements => _elements;

        private List<ChatElementView> _elements = new List<ChatElementView>();
        private Dictionary<string, Color> _userSenderIconDictionary = new();

        private void Start()
        {
            Subscribe();
        }


        private void OnDestroy()
        {
            Unsubscribe();
        }

        public void Subscribe()
        {
            G.StreamController.ChatController.OnMessageReceived += OnMessageReceived;
            G.StreamController.DaysController.OnDayUpdated += OnDayUpdated;
        }
        
        public void Unsubscribe()
        {
            try
            {
                G.StreamController.ChatController.OnMessageReceived -= OnMessageReceived;
                G.StreamController.DaysController.OnDayUpdated -= OnDayUpdated;
            }
            catch (Exception e)
            {
            }
        }

        private void Update()
        {
            var day = G.StreamController.DaysController.CurrentDay > 0
                ? G.StreamController.DaysController.CurrentDay
                : 1;
            _streamDayText.text = $"STREAM DAY: {day} / 3";
        }

        private void OnDayUpdated(int day)
        {
            Clear();
        }

        public void Clear()
        {
            foreach (var chatElementView in _elements)
            {
                Destroy(chatElementView.gameObject);
            }

            _elements.Clear();
        }

        public void SetScrollState(bool active)
        {
            _scrollRect.enabled = active;
        }

        public async void OnMessageReceived(ChatMessage messageData)
        {
            var message = messageData.Type switch
            {
                MessageDataType.Donate => Instantiate(_donateMessage, _parent),
                MessageDataType.Positive => Instantiate(_positiveMessage, _parent),
                MessageDataType.Negative => Instantiate(_negativeMessage, _parent),
                _ => Instantiate(_prefab, _parent)
            };
            message.SetupMessage(messageData, GetSenderColor(messageData.Sender));
            message.gameObject.SetActive(true);
            _elements.Add(message);
            await UniTask.DelayFrame(1);
            _scrollRect.verticalNormalizedPosition = 0;
        }

        public async UniTask OnMessageReceived(ChatMiniGameMessage messageData, Action<ChatElementView> onClick)
        {
            var message = messageData.Type switch
            {
                MessageDataType.Donate => Instantiate(_donateMessage, _parent),
                MessageDataType.Positive => Instantiate(_positiveMessage, _parent),
                MessageDataType.Negative => Instantiate(_negativeMessage, _parent),
                _ => Instantiate(_prefab, _parent)
            };
            message.SetupMessage(messageData, GetSenderColor(messageData.Author), (elementView) =>
            {
                onClick?.Invoke(elementView);
                Destroy(elementView.gameObject);
                _elements.Remove(elementView);
            });
            message.gameObject.SetActive(true);
            _elements.Add(message);
            await UniTask.DelayFrame(1);
            _scrollRect.verticalNormalizedPosition = 0;
        }

        private Color GetSenderColor(string messageDataSender)
        {
            if (string.IsNullOrWhiteSpace(messageDataSender))
                return _senderColors[Random.Range(0, _senderColors.Length)];
            if (_userSenderIconDictionary.TryGetValue(messageDataSender, out var color))
                return color;
            _userSenderIconDictionary.Add(messageDataSender, _senderColors[Random.Range(0, _senderColors.Length)]);
            return _userSenderIconDictionary[messageDataSender];
        }
    }
}