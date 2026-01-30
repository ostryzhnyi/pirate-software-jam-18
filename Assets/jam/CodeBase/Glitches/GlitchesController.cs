using System;
using DG.Tweening;
using UnityEngine;
using jam.CodeBase.Core;
using Ostryzhnyi.EasyViewService.Api.Service;

namespace jam.CodeBase.Glitches
{
    public class GlitchesController
    {
        private readonly GlitchesTag _data;
        private readonly GlitchesSaveModel _saveModel;
        private Material _material;

        private readonly int _handDrawnId;
        private readonly int _chromAbberId;
        private readonly int _glitchId;
        private readonly int _flickerId;
        private readonly int _fadeId;

        private Tween _handDrawnTween;
        private Tween _chromAbberTween;
        private Tween _glitchTween;
        private Tween _flickerTween;
        private Tween _fadeTween;

        private const float TweenDuration = 0.3f;

        public GlitchesController()
        {
            _saveModel = G.Saves.Get<GlitchesSaveModel>();
            _data = GameResources.CMS.Glitches.AsEntity().Get<GlitchesTag>();

            _handDrawnId = Shader.PropertyToID(HandDrawn.Param);
            _chromAbberId = Shader.PropertyToID(ChromaticAberration.Param);
            _glitchId = Shader.PropertyToID(Glitch.Param);
            _flickerId = Shader.PropertyToID(Flicker.Param);
            _fadeId = Shader.PropertyToID(Fade.Param);
        }

        public void UpdateCharacter(Character.Character character)
        {
            _material = G.Menu.HUD.StreamScreenGlitches.material;

            character.OnHealthChanges += OnHealthUpdated;
            character.OnStressChanges += OnStressUpdated;

            ApplyGlitchesInstant();
            TryShowPopupForCurrentState();
        }

        private void OnStressUpdated(float value)
        {
            Debug.LogError("OnStressUpdated " + value);
            
            if (value > 0)
                return;

            float calculatedValue = value;

            if (!G.Donate.LastDonateToAlive.HasValue)
            {
                calculatedValue *= _data.BotsGlitchDamageMuliplier;
            }
            else if (G.Donate.LastDonateToAlive.HasValue)
            {
                calculatedValue = G.Donate.LastDonateToAlive.Value ? value * _data.BotsGlitchDamageMuliplier : value;
            }

            _saveModel.Data.TotalHarm += Math.Abs(calculatedValue);
            _saveModel.ForceSave();

            UpdateGlitches();
            TryShowPopupForCurrentState();
        }

        private void OnHealthUpdated(float value)
        {
            Debug.LogError("OnHealthUpdated " + value);
            
            if (value > 0)
                return;

            float calculatedValue = value;

            if (!G.Donate.LastDonateToAlive.HasValue)
            {
                calculatedValue *= _data.BotsGlitchDamageMuliplier;
            }
            else if (G.Donate.LastDonateToAlive.HasValue)
            {
                calculatedValue = G.Donate.LastDonateToAlive.Value ? value * _data.BotsGlitchDamageMuliplier : value;
            }

            _saveModel.Data.TotalHarm += Math.Abs(calculatedValue);
            _saveModel.ForceSave();

            UpdateGlitches();
            TryShowPopupForCurrentState();
        }

        public void UpdateGlitches()
        {
            if (_material == null)
                return;

            float harm = _saveModel.Data.TotalHarm;
            Debug.LogError("TOTAL HARM: " + harm + " player donated to live: " + (G.Donate.LastDonateToAlive.HasValue && G.Donate.LastDonateToAlive.Value));

            float handDrawnValue = CalculateValue(harm, _data.HandDrawnProportion.TotalHarm, _data.HandDrawnProportion.HandDrawnAmount);
            float chromAbberValue = CalculateValue(harm, _data.ChromaticAberrationProportion.TotalHarm, _data.ChromaticAberrationProportion.Amount);
            float glitchValue = CalculateValue(harm, _data.GlitchProportion.TotalHarm, _data.GlitchProportion.Amount);
            float flickerValue = CalculateValue(harm, _data.FlickerProportion.TotalHarm, _data.FlickerProportion.Percent);
            float fadeValue = CalculateValue(harm, _data.FadeProportion.TotalHarm, _data.FadeProportion.Amount);

            TweenFloat(ref _handDrawnTween, _handDrawnId, handDrawnValue);
            TweenFloat(ref _chromAbberTween, _chromAbberId, chromAbberValue);
            TweenFloat(ref _glitchTween, _glitchId, glitchValue);
            TweenFloat(ref _flickerTween, _flickerId, flickerValue);
            TweenFloat(ref _fadeTween, _fadeId, fadeValue);
        }

        private void ApplyGlitchesInstant()
        {
            if (_material == null)
                return;

            float harm = _saveModel.Data.TotalHarm;

            float handDrawnValue = CalculateValue(harm, _data.HandDrawnProportion.TotalHarm, _data.HandDrawnProportion.HandDrawnAmount);
            float chromAbberValue = CalculateValue(harm, _data.ChromaticAberrationProportion.TotalHarm, _data.ChromaticAberrationProportion.Amount);
            float glitchValue = CalculateValue(harm, _data.GlitchProportion.TotalHarm, _data.GlitchProportion.Amount);
            float flickerValue = CalculateValue(harm, _data.FlickerProportion.TotalHarm, _data.FlickerProportion.Percent);
            float fadeValue = CalculateValue(harm, _data.FadeProportion.TotalHarm, _data.FadeProportion.Amount);

            _material.SetFloat(_handDrawnId, handDrawnValue);
            _material.SetFloat(_chromAbberId, chromAbberValue);
            _material.SetFloat(_glitchId, glitchValue);
            _material.SetFloat(_flickerId, flickerValue);
            _material.SetFloat(_fadeId, fadeValue);
        }

        private static float CalculateValue(float harm, Vector2 harmRange, Vector2 valueRange)
        {
            float t = Mathf.InverseLerp(harmRange.x, harmRange.y, harm);
            return Mathf.Lerp(valueRange.x, valueRange.y, t);
        }

        private void TweenFloat(ref Tween tween, int propertyId, float targetValue)
        {
            if (_material == null)
                return;

            tween?.Kill();

            float startValue = _material.GetFloat(propertyId);

            tween = DOTween.To(
                () => startValue,
                v =>
                {
                    startValue = v;
                    _material.SetFloat(propertyId, v);
                },
                targetValue,
                TweenDuration
            );
        }

        private void TryShowPopupForCurrentState()
        {
            float harm = _saveModel.Data.TotalHarm;

            FadeState fadeState = GetFadeState(harm);

            if (!_saveModel.Data.HandDrawnShown && harm >= _data.HandDrawnProportion.TotalHarm.x)
            {
                if (fadeState == FadeState.None)
                    ShowGlitchPopup(_data.HandDrawnProportion.Messages, false, false);

                _saveModel.Data.HandDrawnShown = true;
                _saveModel.ForceSave();
            }

            if (!_saveModel.Data.ChromaticShown && harm >= _data.ChromaticAberrationProportion.TotalHarm.x)
            {
                if (fadeState == FadeState.None)
                    ShowGlitchPopup(_data.ChromaticAberrationProportion.Messages, false, false);

                _saveModel.Data.ChromaticShown = true;
                _saveModel.ForceSave();
            }

            if (!_saveModel.Data.GlitchShown && harm >= _data.GlitchProportion.TotalHarm.x)
            {
                if (fadeState == FadeState.None)
                    ShowGlitchPopup(_data.GlitchProportion.Messages, false, true);

                _saveModel.Data.GlitchShown = true;
                _saveModel.ForceSave();
            }

            if (!_saveModel.Data.FlickerShown && harm >= _data.FlickerProportion.TotalHarm.x)
            {
                if (fadeState == FadeState.None)
                    ShowGlitchPopup(_data.FlickerProportion.Messages, false, true);

                _saveModel.Data.FlickerShown = true;
                _saveModel.ForceSave();
            }

            if (fadeState == FadeState.First && !_saveModel.Data.FadeFirstShown)
            {
                ShowGlitchPopup(_data.FadeProportion.Messages, false, true);
                _saveModel.Data.FadeFirstShown = true;
                _saveModel.ForceSave();
            }

            if (fadeState == FadeState.Last && !_saveModel.Data.FadeLastShown)
            {
                ShowGlitchPopup(_data.FadeProportion.Messages, true, true);
                _saveModel.Data.FadeLastShown = true;
                _saveModel.ForceSave();
            }
        }

        private FadeState GetFadeState(float harm)
        {
            if (harm >= _data.FadeProportion.TotalHarm.y)
                return FadeState.Last;

            if (harm >= _data.FadeProportion.TotalHarm.x)
                return FadeState.First;

            return FadeState.None;
        }

        private void ShowGlitchPopup(string[] messages, bool isLast, bool isShowRestart)
        {
            if (messages == null || messages.Length == 0)
                return;

            int index = UnityEngine.Random.Range(0, messages.Length);
            string message = messages[index];

            G.Menu.ViewService.ShowView<GlithcesView>(new GlithcesViewOption(isLast, isShowRestart, message));
        }

        private enum FadeState
        {
            None,
            First,
            Last
        }
    }
}
