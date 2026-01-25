using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace jam.CodeBase.Character
{
    public class StatView : MonoBehaviour
    {
        [SerializeField] private Image _progressBar;
        [SerializeField] private Image _progressArrow;
        [SerializeField] private Sprite _positiveProgress;
        [SerializeField] private Sprite _negativeProgress;

        public void UpdateValue(float value)
        {
            Debug.LogError("StatView UpdateStress " + value);

            _progressArrow.gameObject.SetActive(!Mathf.Approximately(value, _progressBar.fillAmount));
            
            _progressArrow.sprite = _progressBar.fillAmount < value ? _positiveProgress : _negativeProgress;
            _progressArrow.material.SetFloat("_TextureScrollYSpeed", _progressBar.fillAmount < value ? -1f : 1f);
            _progressBar.DOFillAmount(value, 1f).SetEase(Ease.InExpo);
            Debug.LogError("DOFillAmount UpdateStress " + value);
            
        }
        
        public void UpdateWithoutNotifyValue(float value)
        {
            _progressBar.fillAmount = value;
        }

        public void DisableArrow()
        {
            _progressArrow.gameObject.SetActive(false);
        }
    }
}