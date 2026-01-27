using System;
using System.Collections.Generic;
using System.Linq;
using jam.CodeBase.Core;
using jam.CodeBase.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace jam.CodeBase
{
    public class OptionsView : MonoBehaviour
    {
        [SerializeField] private Toggle _toggle;
        [SerializeField] private Button _settingsButton;
        [SerializeField] private Button _creditsButton;
        [SerializeField] private Button _exitButton;

        private void Start()
        {
            _settingsButton.onClick.AddListener(OpenSettings);
            _creditsButton.onClick.AddListener(OpenCredits);
            _exitButton.onClick.AddListener(Exit);
        }

        private void OpenSettings()
        {
            G.GlobalViewService.ShowView<SettingsView>();
            _toggle.isOn = false;
        }

        private void OpenCredits()
        {
            G.GlobalViewService.ShowView<CreditsView>();
            _toggle.isOn = false;
        }

        private void Exit()
        {
            Debug.Log("Exit");
            _toggle.isOn = false;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0) 
                && !IsOverGameObject(gameObject)
                && !IsOverGameObject(_toggle.gameObject))
            {
                _toggle.isOn = false;
            }
        }
        
        private bool IsOverGameObject(GameObject gameObject)
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