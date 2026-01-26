using System;
using jam.CodeBase.Economy;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace jam.CodeBase.Stream.View
{
    public class ChatElementView : MonoBehaviour
    {
        public ParticleSystem DestroyParticles;
        
        [SerializeField] private Image _senderIcon;
        [SerializeField] private Image _donateIcon;
        [SerializeField] private TMP_Text _senderText;
        [SerializeField] private TMP_Text _messageText;
        [SerializeField] private Button _button;

        public ChatMiniGameMessage Data;
        
        public void SetupMessage(ChatMessage messageData, Color senderColor)
        {
            if (_senderIcon != null)
                _senderIcon.color = senderColor;

            if (_senderText != null)
            {
                _senderText.text = messageData.Sender;
                _senderText.color = senderColor;
            }

            _messageText.text = messageData.Message;
        }

        public void SetupMessage(ChatMiniGameMessage messageData, Color senderColor, Action<ChatElementView> onClick)
        {
            Data = messageData;
            _button.onClick.RemoveAllListeners();
            _button.onClick.AddListener(() => onClick?.Invoke(this));

            if (_senderIcon != null)
                _senderIcon.color = senderColor;

            if (_senderText != null)
            {
                _senderText.text = messageData.Author;
                _senderText.color = senderColor;
            }

            _messageText.text = messageData.Message;
        }
    }
}