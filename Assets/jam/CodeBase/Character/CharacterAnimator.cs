using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using jam.CodeBase.Core;
using Live2D.Cubism.Core;
using Live2D.Cubism.Framework.Expression;
using Live2D.Cubism.Rendering;
using Sirenix.OdinInspector;
using UnityEngine;

namespace jam.CodeBase.Character
{
    [RequireComponent(typeof(Animator), typeof(CubismExpressionController))]
    public class CharacterAnimator : SerializedMonoBehaviour
    {
        public CubismExpressionList ExpressionsList;
        public CubismModel CubismModel;

        [SerializeField] private List<CharacterAnimation> _characterAnimationStructures = new  List<CharacterAnimation>();
        [SerializeField] private CubismExpressionController _defaultExpressionController;
        [SerializeField] private CubismExpressionController _stressExpressionController;
        [SerializeField] private CubismExpressionController _HPExpressionController;
        [SerializeField] private CubismRenderController _cubismRenderController;
        [SerializeField] private SpriteRenderer _move;

        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            G.CharacterAnimator = this;
        }

        private void OnDestroy()
        {
            G.CharacterAnimator = null;
        }

        public void ApplyTexture(Texture2D texture)
        {
            var renders = GetComponentsInChildren<CubismRenderer>();
            
            foreach (var render in renders)
                render.MainTexture = texture;
        }

        [Button]
        public void PlayAnimation(AnimationType animationType)
        {
            var structure = _characterAnimationStructures.FirstOrDefault(x => x.AnimationType == animationType);
            
            if (animationType is AnimationType.BadHP or AnimationType.FineHP or AnimationType.NormalHP)
            {
                _HPExpressionController.CurrentExpressionIndex =
                    ExpressionsList.CubismExpressionObjects.ToList().IndexOf(structure.Structure.Data);
                return;
            }
            else if (animationType is AnimationType.NormalStress or AnimationType.BadStress or AnimationType.FineStress)
            {
                _stressExpressionController.CurrentExpressionIndex =
                    ExpressionsList.CubismExpressionObjects.ToList().IndexOf(structure.Structure.Data);
                return;
            }
            else if (animationType == AnimationType.Move)
            {
                PlayMoveAnim().Forget();
                return;
            }
            PlayAnimation(structure.Structure);
        }
        
        
        public void PlayAnimation(CharacterAnimationStructure structure)
        {
            if(structure.AnimationTrigger != null)
                SetTrigger(structure.AnimationTrigger);
            if(structure.Data != null)
                SetParameter(structure.Data);
        }

        public void SetTrigger(string trigger)
        {
            _animator.SetTrigger(trigger);
        }

        public void DisableExpressions()
        {
            _defaultExpressionController.CurrentExpressionIndex = -1;
        }

        [Button]
        public void SetParameter(CubismExpressionData data)
        {
            _defaultExpressionController.CurrentExpressionIndex =
                ExpressionsList.CubismExpressionObjects.ToList().IndexOf(data);
        }

        public async UniTask PlayMoveAnim(float delay = .8f)
        {
            _move.gameObject.SetActive(true);
            DOTween.To(() => _cubismRenderController.Opacity, (o) => _cubismRenderController.Opacity = o, 0, .5f);
            await _move.DOColor(new Color(1, 1, 1, 1), 0.5f).ToUniTask();

            await UniTask.WaitForSeconds(delay);
            
            DOTween.To(() => _cubismRenderController.Opacity, (o) => _cubismRenderController.Opacity = o, 1, .5f);
            await _move.DOColor(new Color(1, 1, 1, 0), 0.5f).ToUniTask();
            _move.gameObject.SetActive(false);
        }
    }

    public class CharacterAnimation
    {
        public AnimationType AnimationType;
        public CharacterAnimationStructure Structure;
    }

    [Serializable]
    public struct CharacterAnimationStructure
    {
        public string AnimationTrigger;
        public CubismExpressionData Data;
    }

    public enum AnimationType
    {
        Idle,
        TakeRedPill,
        TakeBluePill,
        Call,
        DrinkWater,
        DrinkLimonade,
        RussianRullete,
        SetHappy,
        SetSad,
        BrokeGlass,
        Smoking,
        Eat,
        NormalStress,
        FineStress,
        BadStress,
        NormalHP,
        FineHP,
        BadHP,
        Move,
        WetHair
    }
}