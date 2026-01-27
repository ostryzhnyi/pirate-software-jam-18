using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace jam.CodeBase.Core.Stream.Views
{
    public class AnswerOnChatMinigame : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private float _baseX;
        [SerializeField] private float _finalX;

        private void Start()
        {
            (transform as RectTransform).anchoredPosition =new Vector2((transform as RectTransform).anchoredPosition.x, _baseX);
        }

        public async UniTask Play(string answer)
        {
            _text.text = answer;

            (transform as RectTransform).DOAnchorPosY(_finalX, .5f).SetEase(Ease.OutBounce);

            await UniTask.WaitForSeconds(5);
            
            (transform as RectTransform).DOAnchorPosY(_baseX, .5f).SetEase(Ease.OutBounce);
            
        }
    }
}