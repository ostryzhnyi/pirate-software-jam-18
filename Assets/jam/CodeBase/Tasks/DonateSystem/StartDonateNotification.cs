using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace jam.CodeBase.Tasks.DonateSystem
{
    public class StartDonateNotification : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private float _baseX;
        [SerializeField] private float _finalX;

        private void Start()
        {
            (transform as RectTransform).anchoredPosition =new Vector2(_baseX, (transform as RectTransform).anchoredPosition.y);
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