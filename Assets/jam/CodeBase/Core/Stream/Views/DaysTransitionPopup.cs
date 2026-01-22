using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using jam.CodeBase.Core;
using Ostryzhnyi.EasyViewService.Api.Service;
using Ostryzhnyi.EasyViewService.ViewLayers;
using UnityEngine;
using UnityEngine.UI;

namespace jam.CodeBase.Stream.View
{
    public class DaysTransitionPopup : BaseView
    {
        [Serializable]
        public class DaysUIData
        {
            public int Day;
            public Sprite DefaultSprite;
            public Sprite SelectedSprite;
            public Image Image;
        }

        public override ViewLayers Layer => ViewLayers.Popup;

        [SerializeField] private CanvasGroup _globalCanvasGroup;
        [SerializeField] private float _fadeDuration = 1f;
        [SerializeField] private float _sliderDelay = 2f;
        [SerializeField] private Slider _slider;
        [SerializeField] private List<DaysUIData> _daysData;

        public void SetupNextDay(int day)
        {
            ResetImages();
            if (day is > 3 or < 0)
                return;
            day--;
            var currentData = _daysData.First(x => x.Day == day - 1);
            var nextData = day - 1 == _daysData.Last().Day
                ? _daysData.Last()
                : _daysData.First(x => x.Day == day );
            currentData.Image.sprite = currentData.SelectedSprite;
            _slider.value = currentData.Day * 10;
            var sequence = DOTween.Sequence();
            sequence.Append(_globalCanvasGroup.DOFade(1, _fadeDuration).From(0));
            sequence.AppendInterval(_sliderDelay);
            sequence.Append(DOVirtual.DelayedCall(0, () =>
            {
                _slider.DOValue(nextData.Day * 10, _sliderDelay);
                currentData.Image.sprite = currentData.DefaultSprite;
                nextData.Image.sprite = nextData.SelectedSprite;
            }));
            sequence.AppendInterval(_sliderDelay + _fadeDuration);
            sequence.Append(_globalCanvasGroup.DOFade(0, _fadeDuration));
            sequence.OnComplete(() => gameObject.SetActive(false));
            sequence.Play();
        }

        public void SetupCurrent(int day)
        {
            ResetImages();
            if (day is > 3 or < 0)
                return;
            day -= 1;
            var currentData = _daysData.First(x => x.Day == day);
          
            currentData.Image.sprite = currentData.SelectedSprite;
            _slider.value = currentData.Day * 10;
            var sequence = DOTween.Sequence();
            sequence.Append(_globalCanvasGroup.DOFade(1, _fadeDuration).From(0));
            sequence.AppendInterval(_sliderDelay);
            sequence.Append(_globalCanvasGroup.DOFade(0, _fadeDuration));
            sequence.OnComplete(() => gameObject.SetActive(false));
            sequence.Play();
        }

        private void ResetImages()
        {
            foreach (var day in _daysData)
            {
                day.Image.sprite = day.DefaultSprite;
            }
        }
    }
}