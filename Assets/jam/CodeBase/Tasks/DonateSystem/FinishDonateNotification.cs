using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace jam.CodeBase.Tasks.DonateSystem
{
    public class FinishDonateNotification: MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private float _baseY;
        [SerializeField] private float _finalY;

        private void Start()
        {
            (transform as RectTransform).anchoredPosition =new Vector2( (transform as RectTransform).anchoredPosition.x, _baseY);
        }

        public async UniTask Play(string donateName)
        {
            _text.text = donateName;

            (transform as RectTransform).DOAnchorPosY(_finalY, .5f).SetEase(Ease.OutBounce);

            await UniTask.WaitForSeconds(3);
            
            (transform as RectTransform).DOAnchorPosY(_baseY, .5f).SetEase(Ease.OutBounce);
        }
    }
}