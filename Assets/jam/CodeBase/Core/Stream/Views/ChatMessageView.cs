using System;
using System.Collections.Generic;
using jam.CodeBase.Core;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace jam.CodeBase.Stream.View
{
    [Serializable]
    public struct SenderColorData
    {
        public Color Color;
        [PreviewField]public Sprite Sprite;
    }

    public class ChatMessageView : MonoBehaviour
    {
        [SerializeField] private ChatElementView _prefab;
        [SerializeField] private ChatElementView _donateMessage;
        [SerializeField] private Transform _parent;
        [SerializeField] private ScrollRect _scrollRect;
        [SerializeField] private SenderColorData[] _senderIcons;

        private List<ChatElementView> _elements = new List<ChatElementView>();
        private Dictionary<string, SenderColorData> _userSenderIconDictionary = new();

        private void Start()
        {
            G.StreamController.ChatController.OnMessageReceived += OnMessageReceived;
            G.StreamController.DaysController.OnDayUpdated += OnDayUpdated;
        }

        private void OnDayUpdated(int day)
        {
            Clear();
        }

        private void Clear()
        {
            foreach (var chatElementView in _elements)
            {
                Destroy(chatElementView.gameObject);
            }

            _elements.Clear();
        }

        private void OnMessageReceived(ChatMessage messageData)
        {
            var message = messageData.Type switch
            {
                MessageDataType.Donate => Instantiate(_donateMessage, _parent),
                _ => Instantiate(_prefab, _parent)
            };
            message.SetupMessage(messageData, GetSenderColor(messageData.Sender));
            message.gameObject.SetActive(true);
            _scrollRect.verticalNormalizedPosition = 0;
            _elements.Add(message);
        }

        private SenderColorData GetSenderColor(string messageDataSender)
        {
            if (string.IsNullOrWhiteSpace(messageDataSender))
                return _senderIcons[Random.Range(0, _senderIcons.Length)];
            if (_userSenderIconDictionary.TryGetValue(messageDataSender, out var color))
                return color;
            _userSenderIconDictionary.Add(messageDataSender, _senderIcons[Random.Range(0, _senderIcons.Length)]);
            return _userSenderIconDictionary[messageDataSender];
        }
    }
}