using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace jam.CodeBase.Tasks.DonateSystem
{
    public class HoverDonateButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Image _positive;
        [SerializeField] private Image _negative;
        [SerializeField] private Button _button;

        private bool _isPositive;

        private void Awake()
        {
            _button = GetComponent<Button>();
        }

        public void SetState(bool positive)
        {
            _isPositive = positive;
        }
        
        private void PlayAnimation(Material material)
        {
            material.SetFloat("_GlowGlobal", 4);
            var dotweenSequence = DOTween.Sequence();

            dotweenSequence
                .Append(DOTween.To(() => material.GetFloat("_GlowGlobal"), g => material.SetFloat("_GlowGlobal", g), 8f, 1f))
                .Append(DOTween.To(() => material.GetFloat("_GlowGlobal"), g => material.SetFloat("_GlowGlobal", g), 5f, 1f))
                .SetLoops(-1)
                .SetId(gameObject.GetInstanceID())
                .Play();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if(!_button.interactable)
                return;
            
            _positive.gameObject.SetActive(_isPositive);
            _negative.gameObject.SetActive(!_isPositive);
            PlayAnimation(_isPositive ? _positive.material : _negative.material);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _positive.gameObject.SetActive(false);
            _negative.gameObject.SetActive(false);
            
            DOTween.Kill(gameObject.GetInstanceID());
        }
    }
}