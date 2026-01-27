using System;
using Cysharp.Threading.Tasks;
using jam.CodeBase.Core;
using Ostryzhnyi.EasyViewService.Api.Service;
using Ostryzhnyi.EasyViewService.ViewLayers;
using UnityEngine;
using UnityEngine.UI;

namespace jam.CodeBase.UI
{
    public class SettingsView : BaseView
    {
        public override ViewLayers Layer => ViewLayers.Popup;

        [SerializeField] private Slider _globalVolume;
        [SerializeField] private Slider _musicVolume;
        [SerializeField] private Slider _soundVolume;
        [SerializeField] private Toggle _screenEffects;

        private void Start()
        {
            _globalVolume.onValueChanged.AddListener(OnGlobalVolumeChanged);
            _musicVolume.onValueChanged.AddListener(OnMusicVolumeChanged);
            _soundVolume.onValueChanged.AddListener(OnSoundVolumeChanged);
            _screenEffects.onValueChanged.AddListener(OnScreenEffectValueChanged);
        }

        private void OnEnable()
        {
            _globalVolume.SetValueWithoutNotify(G.Audio.GlobalVolume);
            _musicVolume.SetValueWithoutNotify(G.Audio.MusicVolume);
            _soundVolume.SetValueWithoutNotify(G.Audio.SoundVolume);
        }

        private void OnDestroy()
        {
            _globalVolume.onValueChanged.RemoveListener(OnGlobalVolumeChanged);
            _musicVolume.onValueChanged.RemoveListener(OnMusicVolumeChanged);
            _soundVolume.onValueChanged.RemoveListener(OnSoundVolumeChanged);
            _screenEffects.onValueChanged.RemoveListener(OnScreenEffectValueChanged);
        }

        private void OnGlobalVolumeChanged(float value)
        {
            G.Audio.GlobalVolume = value;
        }

        private void OnMusicVolumeChanged(float value)
        {
            G.Audio.SetMusicVolume(value);
        }

        private void OnSoundVolumeChanged(float value)
        {
            G.Audio.SetSFXVolume(value);
        }

        private void OnScreenEffectValueChanged(bool value)
        {
            G.Menu.HUD.ScreenEffect.SetActive(value);
        }

        public void HideView()
        {
            base.Hide().Forget();
        }
    }
}