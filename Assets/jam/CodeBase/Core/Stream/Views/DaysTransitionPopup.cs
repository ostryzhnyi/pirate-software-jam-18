using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using jam.CodeBase.Core;
using UnityEngine;
using UnityEngine.UI;

namespace jam.CodeBase.Stream.View
{
    public class DaysTransitionPopup : MonoBehaviour
    {
        [Serializable]
        public class DaysUIData
        {
            public int Day;
            public Sprite DefaultSprite;
            public Sprite SelectedSprite;
            public Image Image;
        }

        [SerializeField] private CanvasGroup _globalCanvasGroup;
        [SerializeField] private float _fadeDuration = 1f;
        [SerializeField] private float _sliderDelay = 2f;
        [SerializeField] private Slider _slider;
        [SerializeField] private List<DaysUIData> _daysData;

        private void OnEnable()
        {
            ResetImages();
            var day = G.StreamController.DaysController.CurrentDay;
            if (day > 2)
                return;
            var currentData = _daysData.First(x => x.Day == day);
            var nextData = _daysData.First(x => x.Day == day + 1);
            currentData.Image.sprite = currentData.SelectedSprite;
            _slider.value = currentData.Day - 1;
            _globalCanvasGroup.DOFade(1, _fadeDuration);
            _slider.DOValue(nextData.Day - 1, 1).OnComplete(() =>
            {
                currentData.Image.sprite = currentData.DefaultSprite;
                nextData.Image.sprite = nextData.SelectedSprite;
            }).SetDelay(_sliderDelay);
            _globalCanvasGroup.DOFade(0, _fadeDuration).SetDelay(_sliderDelay);
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