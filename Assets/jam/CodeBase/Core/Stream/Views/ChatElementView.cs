using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace jam.CodeBase.Stream.View
{
    public class ChatElementView : MonoBehaviour
    {
        [SerializeField] private Image _senderIcon;
        [SerializeField] private Image _donateIcon;
        [SerializeField] private TMP_Text _senderText;
        [SerializeField] private TMP_Text _messageText;

        public void SetupMessage(ChatMessage messageData, SenderColorData senderColor)
        {
            if (_senderIcon != null)
                _senderIcon.sprite = senderColor.Sprite;

            if (_senderText != null)
            {
                _senderText.text = messageData.Sender;
                _senderText.color = senderColor.Color;
            }

            _messageText.text = messageData.Message;
        }
    }
}