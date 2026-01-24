using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace jam.CodeBase.Tasks.DonateSystem
{
    public class HoverPay:MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Image _image;
        [SerializeField] private Button _button;
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            if(!_button.interactable)
                return;
            
            _image.gameObject.SetActive(true);
            
            PlayAnimation(_image.material);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _image.gameObject.SetActive(false);
            DOTween.Kill(gameObject.GetInstanceID());
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
        

    }
}