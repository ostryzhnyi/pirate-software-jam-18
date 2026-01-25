using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using jam.CodeBase.Core;
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
        [SerializeField] private Transform _parent;
        [SerializeField] private ScrollRect _scrollRect;
        [SerializeField] private TMP_Text _streamDayText;
        [SerializeField] private Color[] _senderColors;

        private List<ChatElementView> _elements = new List<ChatElementView>();
        private Dictionary<string, Color> _userSenderIconDictionary = new();

        private void Start()
        {
            G.StreamController.ChatController.OnMessageReceived += OnMessageReceived;
            G.StreamController.DaysController.OnDayUpdated += OnDayUpdated;
        }

        private void OnDestroy()
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
            _streamDayText.text = $"STREAM DAY: {G.StreamController.DaysController.CurrentDay} / 3";
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

        private async void OnMessageReceived(ChatMessage messageData)
        {
            var message = messageData.Type switch
            {
                MessageDataType.Donate => Instantiate(_donateMessage, _parent),
                _ => Instantiate(_prefab, _parent)
            };
            message.SetupMessage(messageData, GetSenderColor(messageData.Sender));
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