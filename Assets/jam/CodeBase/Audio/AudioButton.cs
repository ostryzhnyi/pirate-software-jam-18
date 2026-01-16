using UnityEngine;
using UnityEngine.UI;

namespace jam.CodeBase.Audio
{
    public class AudioButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private AudioCMSEntityPfb _entityPfb;

        private void OnEnable()
        {
            _button.onClick.AddListener(OnClick);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnClick);
        }

        private void OnClick()
        {
           CmsAudioController.Play(_entityPfb);
        }
    }
}