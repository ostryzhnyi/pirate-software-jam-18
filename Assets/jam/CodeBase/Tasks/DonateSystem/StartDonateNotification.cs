using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using jam.CodeBase.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace jam.CodeBase.Tasks.DonateSystem
{
    public class StartDonateNotification : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private float _baseX;
        [SerializeField] private float _finalX;
        [SerializeField] private Button _button;

        private void Start()
        {
            (transform as RectTransform).anchoredPosition =new Vector2(_baseX, (transform as RectTransform).anchoredPosition.y);
            
            _button.onClick.AddListener(() => G.Menu.ViewService.ShowView<DonateView>(new DonateViewOptions(G.Donate.TaskDefinition, G.Donate.BaseTasks)));
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveAllListeners();
        }

        public async UniTask Play(string donateName)
        {
            _text.text = donateName;

            (transform as RectTransform).DOAnchorPosX(_finalX, .5f).SetEase(Ease.OutBounce);

            await UniTask.WaitForSeconds(5);
            
            (transform as RectTransform).DOAnchorPosX(_baseX, .5f).SetEase(Ease.OutBounce);
        }
    }
}