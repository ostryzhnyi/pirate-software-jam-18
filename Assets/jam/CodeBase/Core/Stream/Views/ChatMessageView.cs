using System;
using TMPro;
using UnityEngine;

namespace jam.CodeBase.Stream.View
{
    public class ChatMessageView : MonoBehaviour
    {
        [SerializeField] private StreamSceneStarter _streamSceneStarter;
        [SerializeField] private TMP_Text _prefab;

        private void Start()
        {
            _streamSceneStarter.StreamController.ChatController.OnMessageReceived += OnMessageReceived;
        }

        private void OnMessageReceived(ChatMessage messageData)
        {
            var message = Instantiate(_prefab, transform);
            //todo: chek if type == donate
            message.text = $"{messageData.Sender} : {messageData.Message}";
            message.gameObject.SetActive(true);
        }
    }
}
