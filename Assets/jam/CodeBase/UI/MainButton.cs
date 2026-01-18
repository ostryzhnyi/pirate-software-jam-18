using System;
using UnityEngine;
using UnityEngine.UI;

namespace jam.CodeBase.UI
{
    [RequireComponent(typeof(Button))]
    public class MainButton : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private Sprite _defaultSprite;
        [SerializeField] private Sprite _disableSprite;
        
        private Button _button;

        private void Awake()
        {
            _button = GetComponent<Button>();
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(OnClick);
        }
        
        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnClick);
        }

        private void Update()
        {
            _image.sprite = _button.interactable ? _defaultSprite : _disableSprite;
        }

        private void OnClick()
        {
           //todo play sound
        }
    }
}
