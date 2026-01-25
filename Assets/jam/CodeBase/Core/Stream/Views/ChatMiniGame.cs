using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using jam.CodeBase.Stream.View;
using UnityEngine;

namespace jam.CodeBase.Core.Stream.Views
{
    public class ChatMiniGame : MonoBehaviour
    {
        public Transform Parent;
        public Transform Chat;
        public ChatMessageView ChatMessageView;

        private Transform _baseParent;
        private Vector3 _baseScale;
        private Vector3 _basePos;
        
        private void Awake()
        {
            _baseParent = Chat.parent;
            _baseScale = Chat.localScale;
            _basePos = Chat.localPosition;
        }

        public async UniTask Play()
        {
            // ChatMessageView = Parent;
            // Chat.transform.DOMove() Parent.position;
        }
    }
}