using System;
using UnityEngine;
using UnityEngine.UI;

namespace jam.CodeBase.Audio
{
    public class AudioButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private AudioCMSEntityPfb _entityPfb;
        [SerializeField] private string _entityPfbPath;

        private void Awake()
        {
            _button = GetComponent<Button>();
            if (_entityPfb == null)
            {
                _entityPfb = Resources.Load<AudioCMSEntityPfb>(_entityPfbPath);
            }
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(OnClick);
        }

        private void OnDisable()
        {
            if(_button != null)
                _button.onClick.RemoveListener(OnClick);
        }

        private void OnClick()
        {
           CmsAudioController.Play(_entityPfb);
        }
    }
}