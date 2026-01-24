using System;
using System.Collections.Generic;
using System.Linq;
using jam.CodeBase.Core;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace jam.CodeBase.Stream.View
{
    public class VolumeControlView : MonoBehaviour
    {
        [SerializeField] private Button _volumeToggle;
        [SerializeField] private Slider _volumeSlider;
        [SerializeField] private Image _volumeImage;
        [SerializeField] private Sprite _volumeOn;
        [SerializeField] private Sprite _volumeOff;

        [SerializeField] private GameObject _volumePanel;

        private float Volume
        {
            get => G.Audio.GlobalVolume;
            set => G.Audio.GlobalVolume = value;
        }

        private void Awake()
        {
            _volumeSlider.onValueChanged.AddListener(SetVolume);
            _volumeToggle.onClick.AddListener(OpenVolumePanel);
        }

        private void Start()
        {
            Debug.LogError(Volume);
            _volumeSlider.SetValueWithoutNotify(Volume);
            _volumeImage.color = Color.Lerp(Color.black, Color.white, Volume);
            _volumeImage.sprite = Volume > 0 ? _volumeOn : _volumeOff;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0) && !IsOverGameObject(gameObject))
            {
                _volumePanel.SetActive(false);
            }
        }

        private void OnDestroy()
        {
            _volumeSlider.onValueChanged.RemoveListener(SetVolume);
            _volumeSlider.onValueChanged.RemoveAllListeners();
        }

        private void SetVolume(float value)
        {
            Volume = value;
            _volumeImage.color = Color.Lerp(Color.black, Color.white, Volume);
            _volumeImage.sprite = Volume > 0 ? _volumeOn : _volumeOff;
        }

        private void OpenVolumePanel()
        {
            _volumePanel.SetActive(!_volumePanel.gameObject.activeSelf);
        }

        public bool IsOverGameObject(GameObject gameObject)
        {
            var eventData = new PointerEventData(EventSystem.current) { position = Input.mousePosition };
            var raycastResults = new List<RaycastResult>();
            EventSystem.current?.RaycastAll(eventData, raycastResults);
            var containsObject = raycastResults.Count != 0 && raycastResults.Any(x => x.gameObject == gameObject
                                                                                      || IsChild(x.gameObject, gameObject.transform));
            return containsObject;
        }

        private static bool IsChild(GameObject current, Transform parent)
        {
            var parentTransform = current.transform.parent;
            while (parentTransform)
            {
                if (parentTransform == parent)
                {
                    return true;
                }

                parentTransform = parentTransform.parent;
            }

            return false;
        }
    }
}