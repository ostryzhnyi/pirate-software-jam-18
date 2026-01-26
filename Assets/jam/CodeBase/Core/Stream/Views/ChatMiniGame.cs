using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using jam.CodeBase.Character;
using jam.CodeBase.Economy;
using jam.CodeBase.Stream;
using jam.CodeBase.Stream.View;
using jam.CodeBase.Utils;
using ProjectX.CodeBase.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace jam.CodeBase.Core.Stream.Views
{
    public class ChatMiniGame : MonoBehaviour
    {
        public Transform Parent;
        public TMP_Text _text;
        public Transform Chat;
        public ChatMessageView ChatMessageView;
        public Image _background;

        private Transform _baseParent;
        private Vector3 _baseScale;
        private Vector3 _basePos;
        private int _baseTransformIndex;
        private ChatMiniGameEconomy balance;
        private ChatMiniGameMessage[] messages;
        private List<ChatMiniGameMessage> goodMessage;
        private List<ChatMiniGameMessage> badMessage;

        private float _minusMoney;
        private float _plusMoney;
        private float _stress;
        
        
        private void Awake()
        {
            G.ChatMiniGame = this;
            _baseParent = Chat.parent;
            _baseScale = Chat.localScale;
            _basePos = Chat.localPosition;
            _baseTransformIndex = transform.GetSiblingIndex();
            balance = GameResources.CMS.ChatMiniGame.ChatMiniGameBalance.As<ChatMiniGameEconomy>();
            
            messages = GameResources.CMS.ChatMiniGame.ChatMiniMessages.As<ChatMiniGameMessages>().Messages;
            goodMessage = messages.Where(m => m.Type == MessageDataType.Positive).ToList();
            badMessage = messages.Where(m => m.Type == MessageDataType.Negative).ToList();
            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            G.ChatMiniGame = null;
        }

        public async UniTask Play()
        {
            _text.SetText("");
            gameObject.SetActive(true);
            ChatMessageView.Unsubscribe();
            ChatMessageView.Clear();
            ChatMessageView.SetScrollState(false);
            Chat.parent = Parent;
            (Chat as RectTransform).DOAnchorPos(Vector3.one, .5f);
            Chat.DOScale(1.12f, .5f);
            await UniTask.WaitForSeconds(.5f);
            _background.DOFade(.85f, .5f);
            var ftueSaveModel = G.Saves.Get<FTUESaveModel>();
            if (!ftueSaveModel.Data.ShowedChatMinigameFTUE)
            {
                await PlayFTUE(ftueSaveModel);
            }
            
            var duration = (float)balance.Duration;
            while (duration > 0)
            {
                for (int i = 0; i < balance.OneMessagePerTickRange.GetRandomRange(); i++)
                {
                    var message = balance.GoodMesssagePercentRange.GetRandomRange() > 50 ? goodMessage.GetRandom() : badMessage.GetRandom();
                    await ChatMessageView.OnMessageReceived(message, OnClick);
                }
                await UniTask.WaitForSeconds(balance.OneTick);
                duration -= balance.OneTick;
            }
            
            var lostBedMessageCount = ChatMessageView.Elements.Count(e => e.Data.Type == MessageDataType.Negative);
            _minusMoney += lostBedMessageCount * balance.BedMesssageSkipped;
            _stress +=  lostBedMessageCount * balance.BedMesssageSkippedStess;

            Debug.Log("Mini game minus money: " + _minusMoney);
            Debug.Log("Mini game add stress: " + _stress);
            Debug.Log("Mini game plus money: " + _plusMoney);

            G.Characters.CurrentCharacter.ChangeStress(_stress, StatsChangeMethod.Add).Forget();
            var totalMoney = _plusMoney -  _minusMoney;
            G.Economy.AddMoney(totalMoney);
            
            _background.DOFade(0f, .5f);
            
            Chat.parent = _baseParent;
            Chat.SetSiblingIndex(_baseTransformIndex);
            Chat.DOLocalMove(_basePos, .5f);
            Chat.DOScale(_baseScale, .5f);
            await UniTask.WaitForSeconds(.5f);
            ChatMessageView.Subscribe();
            ChatMessageView.Clear();
            gameObject.SetActive(false);
            ChatMessageView.SetScrollState(true);
        }

        
        public async UniTask PlayFTUE(FTUESaveModel saveModel)
        {
            await _text.ToType(
                "The chat contains good messages <color=\"green\">(green)</color> and bad messages <color=\"red\">(red)</color>. Your goal is to delete the bad messages.",
                .06f);
            
            await UniTaskHelper.SmartWaitSeconds(4f);
            
            await _text.ToType(
                "For each bad message you delete, you'll earn money, and for each bad message you miss, the target will experience a stress increase",
                .06f);
            
            await UniTaskHelper.SmartWaitSeconds(3f);
            await _text.ToType(
                "For each good message you delete, you'll lose money.",
                .06f);
            await UniTaskHelper.SmartWaitSeconds(1f);
            saveModel.Data.ShowedChatMinigameFTUE = true;
            saveModel.ForceSave();
        }

        private void OnClick(ChatElementView message)
        {
            if (message.Data.Type == MessageDataType.Negative)
            {
                _plusMoney += balance.BedMesssageDeleted;
            }
            else if (message.Data.Type == MessageDataType.Positive)
            {
                _minusMoney += balance.GoodMesssageDeleteMoney;
            }
            
        }
    }
}